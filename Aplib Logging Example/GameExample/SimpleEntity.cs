
namespace Aplib_Logging_Example.GameExample
{
    public abstract class SimpleEntity
    {
        public string Name { get; protected set;} 

        public int Health { get; protected set;}

        public Location CurrentLocation { get; protected set;}

        public bool IsAlive => Health > 0;

        protected bool _actionTaken;

        protected SimpleEntity(int health, Location location, string name = "Entity")
        {
            Health = health;
            CurrentLocation = location;
            Name = name;
        }

        public virtual void Update()
        {
            _actionTaken = false;
        }
        
        public virtual void TakeDamage(int damage) => Health -= damage;

        public virtual void MoveTo(Location location)
        {
            if (_actionTaken) return;
            _actionTaken = true;

            Console.WriteLine($"{Name} moved from {CurrentLocation} to {location}");
            CurrentLocation = location;
        }
    }
}