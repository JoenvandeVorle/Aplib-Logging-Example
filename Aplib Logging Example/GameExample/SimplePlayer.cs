using System;

namespace Aplib_Logging_Example.GameExample 
{
    public class SimplePlayer : SimpleEntity
    {
        public SimplePlayer() 
        {
            Health = 100;
            CurrentLocation = Location.Home;
        }

        public SimplePlayer(Location location) 
        {
            Health = 100;
            CurrentLocation = location;
        }

        public void Attack(SimpleEnemy enemy) 
        {
            enemy.TakeDamage(10);
        }

        public void Heal() 
        {
            Health = 100;
        }
    }
}
