using System;
using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    /// <summary>
    /// A living, breathing entity; NOT an ECS entity.
    /// </summary>
    public class Entity
    {
        public int CurrentHealth { get; }
        public int TotalHealth { get; }
        public int Strength { get; }
        public int Defense { get; }
        public Color Color { get; }
        public char Character { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public static Entity CreateFromTemplate(string name)
        {
            // This code makes me cry.
            switch (name.ToLower()) {
                case "brigand": return new Entity('b', Color.Red, 20, 4, 1);
                default: throw new InvalidOperationException($"Not sure how to create a {name} template entity");
            }
        }
        
        public Entity(char character, Color color, int health, int strength, int defense)
        {
            this.Character = character;
            this.Color = color;
            this.CurrentHealth = health;
            this.TotalHealth = health;
            this.Strength = strength;
            this.Defense = defense;
        }
    }
}