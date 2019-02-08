using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    public abstract class AbstractEntity {
        public Color Color { get; set; }
        public char Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public AbstractEntity(char character, Color color)
        {
            this.Character = character;
            this.Color = color;
        }
    }
}