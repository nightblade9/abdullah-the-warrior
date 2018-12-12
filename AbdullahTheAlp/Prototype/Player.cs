using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheAlp.Prototype
{
    public class Player : Entity
    {
        public string ClassName { get; private set; }

        public Player(string className, int health, int strength, int defense, int visionRange, int numberOfTurns, int numberOfAttacks)
        : base("You", '@', Color.White, health, strength, defense, visionRange, numberOfTurns, numberOfAttacks)
        {
            this.ClassName = className;
        }
    }
}