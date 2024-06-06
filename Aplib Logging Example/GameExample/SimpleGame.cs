using System;

namespace Aplib_Logging_Example.GameExample 
{
    public class SimpleGame 
    {
        public bool GameEnded => !_gameRunning;

        private bool _gameRunning = false;

        private SimplePlayer _player;
        private SimpleEnemy _enemy;

        /// <summary>
        /// Start the game
        /// </summary>
        public void Setup() 
        {
            _gameRunning = true;

            _player = new SimplePlayer();
            _enemy = new SimpleEnemy(50, Location.Forest);
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
            if (!_enemy.IsAlive)
                WinGame();
        }

        private void WinGame() 
        {
            Console.WriteLine("You win!");
            _gameRunning = false;
        }

        public SimplePlayer GetPlayer() 
        {
            return _player;
        }

        public SimpleEnemy GetEnemy() 
        {
            return _enemy;
        }
    }
}
