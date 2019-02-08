using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    public class AbstractEntity {
        public Color Color { get; set; }
        public char Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        public AbstractEntity(int x, int y, char character, Color color) : this(character, color)
        {
            this.X = x;
            this.Y = y;
        }

        public AbstractEntity(char character, Color color)
        {
            this.Character = character;
            this.Color = color;
        }
    }
}