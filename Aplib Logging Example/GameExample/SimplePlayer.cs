using System;

namespace Aplib_Logging_Example.GameExample 
{
    public class SimplePlayer : SimpleEntity
    {
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
            if (enemy.CurrentLocation != CurrentLocation)
            {
                Console.WriteLine($"{Name} cannot attack enemy in different location!");
                return false;
            }
            Console.WriteLine($"{Name} attacked {enemy.Name}!");
            enemy.TakeDamage(10);
            return true;
        }

        public void Heal() 
        {
            Health = 100;
        }
    }
}
