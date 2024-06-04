
namespace Aplib_Logging_Example.GameExample
{
    public abstract class SimpleEntity
    {
        public int Health { get; protected set; }

        public Location CurrentLocation { get; protected set; }

        public bool IsAlive => Health > 0;

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public virtual void MoveTo(Location location)
        {
            CurrentLocation = location;
        }
    }
}