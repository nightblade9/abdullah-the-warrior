using System;
using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheAlp.Prototype
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

        public static Entity CreateFromTemplate(string name)
        {
            // This code makes me cry.
            switch (name.ToLower()) {
                case "brigand": return new Entity("Brigand", 'b', Palette.Red, 20, 4, 1);
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
    }
}