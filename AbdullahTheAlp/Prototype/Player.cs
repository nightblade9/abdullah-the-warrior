using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheAlp.Prototype
{
    public class Player : Entity
    {
        public static readonly int StunProbability = 50; // 30 = 30%
        public static readonly int StunTurns = 3;

        public Specialization Specialization { get; private set; }

        public Player(Specialization specialization, int health, int strength, int defense, int visionRange, int numberOfTurns, int numberOfAttacks)
        : base("You", '@', Color.White, health, strength, defense, visionRange, numberOfTurns, numberOfAttacks)
        {
            this.Specialization = specialization;
        }
    }

    public enum Specialization {
        Faris,
        Stunhammer
    }
}