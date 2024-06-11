using System;

namespace Aplib_Logging_Example.GameExample 
{
    public class SimpleGame 
    {
        public bool GameEnded => !_gameRunning;

        public bool GameWon {get; private set;}

        private bool _gameRunning = false;

        private SimplePlayer _player;
        private SimpleEnemy _enemy;

        private int _turn = 0;

        /// <summary>
        /// Start the game
        /// </summary>
        public void Setup() 
        {
            _gameRunning = true;
            GameWon = false;

            _player = new SimplePlayer();
            _enemy = new SimpleEnemy(30, Location.Forest);
        }

        public void Restart() 
        {
            Setup();
        }

        /// <summary>
        /// Main game update
        /// </summary>
        public void Update() 
        {
            Console.WriteLine($"Turn: {_turn++}");
            _player.Update();

            if (_player.CurrentLocation == Location.Home && !_enemy.IsAlive)
            {
                Console.WriteLine("You made it home safely and won!");
                GameWon = true;
                EndGame();
            }
        }

        public SimplePlayer GetPlayer() 
        {
            return _player;
        }

        public SimpleEnemy GetEnemy() 
        {
            return _enemy;
        }

        public void EndGame() 
        {
            _gameRunning = false;
        }
    }
}
