
namespace Aplib_Logging_Example.GameExample
{
    public static class GameRunner
    {
        public static void Run() 
        {
            SimpleGame game = new();
            game.Setup();

            SimplePlayer player = game.GetPlayer();

            while (!game.GameEnded) {
                
                game.Update();
                
                if (game.GameWon) break;

                // Make the player move to the enemy
                if (player.CurrentLocation != game.GetEnemy().CurrentLocation)
                    player.MoveTo(game.GetEnemy().CurrentLocation);

                // Try to attack the enemy
                if (game.GetEnemy().IsAlive)
                    player.TryAttack(game.GetEnemy());

                // Move back home if enemy is dead
                if (!game.GetEnemy().IsAlive)
                    player.MoveTo(player.Home);
            }
        }   
    }
}