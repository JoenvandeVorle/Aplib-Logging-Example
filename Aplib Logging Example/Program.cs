using Aplib_Logging_Example.GameExample;

namespace Aplib_Logging_Example {
    class Program {
        static void Main(string[] args) {

            SimpleGame game = new();
            game.Setup();

            SimplePlayer player = game.GetPlayer();

            while (!game.GameEnded) {

                // Make the player move to the enemy
                if (player.CurrentLocation != game.GetEnemy().CurrentLocation)
                    player.MoveTo(game.GetEnemy().CurrentLocation);

                // Try to attack the enemy
                player.TryAttack(game.GetEnemy());

                game.Update();
            }
        }
    }
}
