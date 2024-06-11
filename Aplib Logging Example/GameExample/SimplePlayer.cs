using System;

namespace Aplib_Logging_Example.GameExample 
{
    public class SimplePlayer : SimpleEntity
    {
        public Location Home => Location.Home;

        public SimplePlayer(int Health = 100, Location CurrentLocation = Location.Home, string Name = "Player") 
            : base(Health, CurrentLocation, Name)
        {
        }

        /// <summary>
        /// Try to attack an enemy
        /// </summary>
        /// <returns>True if the attack was successful, false otherwise</returns>
        public bool TryAttack(SimpleEnemy enemy) 
        {
            if (_actionTaken) return false;
            _actionTaken = true;

            if (enemy.CurrentLocation != CurrentLocation && enemy.IsAlive)
            {
                Console.WriteLine($"{Name} cannot attack {enemy.Name}!");
                return false;
            }
            Console.WriteLine($"{Name} attacked {enemy.Name}!");
            enemy.TakeDamage(10);
            return true;
        }

        public void Heal() 
        {
            if (_actionTaken) return;
            _actionTaken = true;

            Health = 100;
        }
    }
}
