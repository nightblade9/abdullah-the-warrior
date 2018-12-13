using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheAlp.Prototype
{
    public class Player : Entity
    {
        public static readonly int StunProbability = 50; // 30 = 30%
        public static readonly int StunTurns = 3;
        public static readonly int KnockBackProbability = 100; // 10 = 10%
        public static readonly int KnockBackDistance = 4;
        public static readonly int SecondaryKnockbackDistance = 2;
        public static readonly float SecondaryKnockbackDamageRatio = 0.5f; // 0.2f = 20%

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