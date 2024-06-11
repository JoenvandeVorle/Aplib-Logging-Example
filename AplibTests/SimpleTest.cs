using Aplib.Core;
using Aplib.Core.Belief.Beliefs;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Tactics;
using Aplib.Logging;
using Aplib_Logging_Example.AplibInterface;
using Aplib_Logging_Example.GameExample;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;
using Xunit.Abstractions;

using Action = Aplib.Core.Intent.Actions.Action<AplibTests.SimpleTest.SimpleBeliefSet>;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AplibTests 
{
    public class SimpleTest 
    {
        public class SimpleBeliefSet : BeliefSet 
        {
            public Belief<SimpleEnemy, SimpleEnemy> Enemy = new(_simpleGame.GetEnemy(), e => e);

            public Belief<SimpleEnemy, Location> EnemyLocation = new(_simpleGame.GetEnemy(), e => e.CurrentLocation);

            public Belief<SimpleEnemy, int> EnemyHealth = new(_simpleGame.GetEnemy(), e => e.Health);

            public Belief<SimplePlayer, SimplePlayer> Player = new(_simpleGame.GetPlayer(), p => p);

            public Belief<SimplePlayer, Location> PlayerLocation = new(_simpleGame.GetPlayer(), p => p.CurrentLocation);

            public Belief<SimpleGame, bool> GameEnded = new(_simpleGame, g => g.GameEnded);
        }

        private static SimpleGame _simpleGame = new();
        private readonly SimpleBeliefSet _beliefSet;
        private readonly ILogger _logger;

        /// <summary>
        /// Set up a Serilog logger, that acts as a Microsoft.Extensions.Logging.ILogger
        /// It writes to the test output via the ITestOutputHelper, so the logs are visible in the test output
        /// </summary>
        public SimpleTest(ITestOutputHelper output) 
        {
            _simpleGame.Setup();
            _beliefSet = new SimpleBeliefSet();

            Logger log = new LoggerConfiguration()
                .WriteTo.TestOutput(output)
                .WriteTo.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddSerilog(log);
            });

            ILogger microsoftLogger = loggerFactory.CreateLogger<SimpleTest>();

            microsoftLogger.LogInformation("Game set up!");
            _logger = microsoftLogger;
        }

        [Fact]
        public void PlayGameTest() 
        {
            Action attackEnemy = new(
                new Metadata("Attack enemy", "Attacks the enemy"),
                beliefset =>
                {
                    SimplePlayer player = beliefset.Player;
                    player.TryAttack(beliefset.Enemy);
                }
            );

            Action moveToEnemy = new(
                new Metadata("Move to enemy", "Moves to the enemy"),
                beliefset =>
                {
                    SimplePlayer player = beliefset.Player;
                    player.MoveTo(beliefset.EnemyLocation);
                }
            );

            Action moveBackHome = new(
                new Metadata("Move back home", "Moves back to the home location"),
                beliefset =>
                {
                    SimplePlayer player = beliefset.Player;
                    player.MoveTo(player.Home);
                }
            );

            // Tactics: Kill enemy
            // First move to it, then attack it
            PrimitiveTactic<SimpleBeliefSet> attackEnemyTactic = new(new Metadata("Attack enemy tactic"), attackEnemy, AtEnemyPositionPredicate);
            FirstOfTactic<SimpleBeliefSet> killEnemy = new(
                new Metadata("Kill enemy tactic", "attack the enemy, or move closer when not in range"), 
                attackEnemyTactic, 
                moveToEnemy.Lift()
            );

            // Goals
            // The game is won if the enemy is dead and the player is back home
            Goal<SimpleBeliefSet> enemyDeadGoal = new(new Metadata("Enemy dead goal"), killEnemy, EnemyDeadPredicate);
            Goal<SimpleBeliefSet> backHomeGoal = new(new Metadata("Back home goal"), moveBackHome.Lift(new Metadata("Move back home tactic")), PlayerAtHomePredicate);
            SequentialGoalStructure<SimpleBeliefSet> gameWonGoalStructure = new(
                new Metadata("Game won goal structure"), 
                enemyDeadGoal.Lift(new Metadata("Enemy dead goal structure")),
                backHomeGoal.Lift(new Metadata("Back home goal structure"))
            );

            // Desire set
            LoggableDesireSet<SimpleBeliefSet> desireSet = new(new Metadata("Play game DesireSet", "kill the enemy and move back home"), gameWonGoalStructure);

            // Agent
            LoggableBdiAgent<SimpleBeliefSet> agent = new(_beliefSet, desireSet);

            SimpleGameAplibRunner<SimpleBeliefSet> testRunner = new(agent, _logger, _simpleGame);

            bool testResult = testRunner.Test();

            Assert.True(testResult);

            bool AtEnemyPositionPredicate(SimpleBeliefSet beliefset)
            {
                Location playerLocation = beliefset.PlayerLocation;
                Location enemyLocation = beliefset.EnemyLocation;
                return playerLocation == enemyLocation;
            }

            bool EnemyDeadPredicate(SimpleBeliefSet beliefset)
            {
                int enemyHealth = beliefset.EnemyHealth;
                return enemyHealth < 0;
            }

            bool PlayerAtHomePredicate(SimpleBeliefSet beliefset)
            {
                Location playerLocation = beliefset.PlayerLocation;
                SimplePlayer player = beliefset.Player;
                return playerLocation == player.Home;
            }
        }
    }
}