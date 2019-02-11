using System;
using System.Collections.Generic;
using System.Linq;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    // Two of these make a pair of a single teleporter. Or maybe three of them cycle ...   
    public class TeleporterPad : AbstractEntity
    {
        public TeleporterPad Destination { get; set; }  
        
        public TeleporterPad(int x, int y) : base(x, y, '?', Palette.Blue)
        {
        }

        public void TeleportContents(IList<Entity> monsters, Entity player)
        {
            var toTeleport = new List<Entity>();
            
            toTeleport.AddRange(monsters.Where(m => m.X == this.X && m.Y == this.Y && m.WasJustTeleported == false));
            
            if (player.X == this.X && player.Y == this.Y && player.WasJustTeleported == false) {
                toTeleport.Add(player);
            }
            
            foreach (var entity in toTeleport)
            {
                entity.WasJustTeleported = true;
                entity.X = this.Destination.X;
                entity.Y = this.Destination.Y;
                Console.WriteLine($"Teleported {entity.Name} from {this.X}, {this.Y} to {this.Destination.X}, {this.Destination.Y}");
            }
        }   
    }
}