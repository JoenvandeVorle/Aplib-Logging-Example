
namespace Aplib_Logging_Example.GameExample 
{
    public  class SimpleEnemy : SimpleEntity
    {
        public SimpleEnemy(int health, Location location) 
        {
            Health = health;
            CurrentLocation = location;
        }
    }
}
