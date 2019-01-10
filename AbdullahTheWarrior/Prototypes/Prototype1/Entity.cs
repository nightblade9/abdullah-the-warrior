using System;
using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype1
{
    /// <summary>
    /// A living, breathing entity; NOT an ECS entity.
    /// </summary>
    public class Entity
    {
        public string Name { get; private set; }
        public int CurrentHealth { get; private set; }
        public int TotalHealth { get; }
        public int Strength { get; }
        public int Defense { get; }
        public Color Color { get; set; }
        public char Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int VisionRange { get; }
        public int NumberOfTurns { get; } = 1;
        public int NumberOfAttacks { get; } = 1;
        public int TurnsStunned { get; private set; }

        public static Entity CreateFromTemplate(string name)
        {
            // This code makes me cry.
            switch (name.ToLower()) {
                case "brigand": return new Entity("Brigand", 'b', Palette.Red, 40, 8, 3);
                default: throw new InvalidOperationException($"Not sure how to create a {name} template entity");
            }
        }
        
        public Entity(string name, char character, Color color, int health, int strength, int defense, int visionRange = 5, int numberOfTurns = 1, int numberOfAttacks = 1)
        {
            this.Name = name;
            this.Character = character;
            this.Color = color;
            this.CurrentHealth = health;
            this.TotalHealth = health;
            this.Strength = strength;
            this.Defense = defense;
            this.VisionRange = visionRange;
            this.NumberOfTurns = numberOfTurns;
            this.NumberOfAttacks = numberOfAttacks;
        }

        public void Damage(int damage)
        {
            if (damage < 0) 
            {
                throw new InvalidOperationException($"Damage ({damage}) must be non-negative");
            }

            this.CurrentHealth -= damage;

            if (this.CurrentHealth <= 0)
            {
                EventBus.Instance.Broadcast(GameEvent.EntityDeath, this);
            }
        }

        public void TakeTurn()
        {
            if (this.TurnsStunned > 0) {
                this.TurnsStunned -= 1;
            }
        }

        public void Stun(int turns)
        {
            this.TurnsStunned += turns;
        }

        public bool IsStunned { get { return this.TurnsStunned > 0; } }
    }
}