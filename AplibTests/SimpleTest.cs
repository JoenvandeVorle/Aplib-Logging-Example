using Aplib.Core;
using Aplib.Core.Belief.Beliefs;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Tactics;
using Aplib.Logging;
using Aplib_Logging_Example.AplibInterface;
using Aplib_Logging_Example.GameExample;
using Serilog;
using Serilog.Core;
using Xunit.Abstractions;
using Action = Aplib.Core.Intent.Actions.Action<AplibTests.SimpleTest.SimpleBeliefSet>;

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
        private SimpleBeliefSet _beliefSet;
        private Logger _logger;

        public SimpleTest(ITestOutputHelper output) 
        {
            _simpleGame.Setup();
            _beliefSet = new SimpleBeliefSet();

            Logger log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output)
                .CreateLogger();

            log.Information("Game set up!");
            _logger = log;
        }

        [Fact]
        public void KillEnemyTest() 
        {
            // Action: Attack the enemy
            Action attackEnemy = new(
                beliefset =>
                {
                    SimplePlayer player = beliefset.Player;
                    player.TryAttack(beliefset.Enemy);
                    _logger.Information("Player attacked enemy!");
                }
            );

            // Action: Move to the enemy
            Action moveToEnemy = new(
                beliefset =>
                {
                    _logger.Information("Player moving to enemy!");
                    SimplePlayer player = beliefset.Player;
                    player.MoveTo(beliefset.EnemyLocation);
                }
            );

            // Tactic: Kill enemy
            // First move to it, then attack it
            PrimitiveTactic<SimpleBeliefSet> attackEnemyTactic = new(new Metadata("attacking enemy"), attackEnemy, AtEnemyPositionPredicate);
            PrimitiveTactic<SimpleBeliefSet> moveToEnemyTactic = new(new Metadata("moving to enemy"), moveToEnemy);
            FirstOfTactic<SimpleBeliefSet> killEnemy = new(new Metadata("attacking or moving to enemy"), attackEnemyTactic, moveToEnemyTactic);

            // Goalstructure
            Goal<SimpleBeliefSet> enemyDeadGoal = new(killEnemy, EnemyAliveAndGameRunningPredicate);
            PrimitiveGoalStructure<SimpleBeliefSet> enemyDeadGoalStructure = new(enemyDeadGoal);

            // Desire set
            DesireSet<SimpleBeliefSet> desireSet = new(new Metadata("kill enemy desireset"), enemyDeadGoalStructure);

            // Agent
            LoggableBdiAgent<SimpleBeliefSet> agent = new(_beliefSet, desireSet);

            AplibRunner<SimpleBeliefSet> testRunner = new(agent, _logger);

            bool testResult = testRunner.Test(_simpleGame);

            Assert.True(testResult);

            bool AtEnemyPositionPredicate(SimpleBeliefSet beliefset)
            {
                Location playerLocation = beliefset.PlayerLocation;
                Location enemyLocation = beliefset.EnemyLocation;
                return playerLocation == enemyLocation;
            }

            bool EnemyAliveAndGameRunningPredicate(SimpleBeliefSet beliefset) => beliefset.EnemyHealth > 0 && !beliefset.GameEnded;
        }
    }
}