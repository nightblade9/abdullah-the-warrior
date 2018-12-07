using System;
using System.Collections.Generic;
using System.Linq;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    /// <summary>
    /// Small little class to handle bow drawing/input/etc.
    /// </summary>
    public class BowManager
    {
        public bool IsActive { get; private set; } = false;
        private Entity player;
        private IEnumerable<Entity> monsters;
        public Entity Target { get; private set; }

        public BowManager(Entity player)
        {
            this.player = player;
        }

        public void Activate(IEnumerable<Entity> monsters)        
        {
            this.IsActive = true;
            this.monsters = monsters;

            this.Target = this.FindClosestMonster();
        }

        public void Deactivate()
        {
            this.IsActive = false;
            this.Target = null;
        }

        // Return true if player took a turn
        public bool ProcessPlayerInput()
        {
            return false;
        }

        public void Draw(SadConsole.Console console)
        {
            if (!this.IsActive)
            {
                return;
            }

            if (this.Target != null)
            {
                console.DrawCharacter(this.Target.X, this.Target.Y, this.Target.Character, this.Target.Color, Palette.PaleYellow);
            }
        }

        public bool HasTarget { get { return this.Target != null; } }

        private Entity FindClosestMonster()
        {
            Entity target = null;
            var distance = double.MaxValue;

            foreach (var monster in this.monsters)
            {
                var currentDistance = Math.Sqrt(Math.Pow(monster.X - player.X, 2) + Math.Pow(monster.Y - player.Y, 2));
                if (currentDistance <= player.VisionRange && currentDistance < distance)
                {
                    distance = currentDistance;
                    target = monster;
                }
            }
            
            return target;
        }
    }
}