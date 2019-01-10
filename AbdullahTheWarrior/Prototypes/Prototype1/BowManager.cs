using System;
using System.Collections.Generic;
using System.Linq;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype1
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

        public void RotateTarget()
        {
            // Find a list of visible monsters
            var visibleMonsters = new List<Entity>();

            foreach (var monster in this.monsters)
            {
                var distance = Math.Sqrt(Math.Pow(monster.X - player.X, 2) + Math.Pow(monster.Y - player.Y, 2));
                if (distance < player.VisionRange)
                {
                    visibleMonsters.Add(monster);
                }
            }

            if (visibleMonsters.Any())
            {
                // Find current monster index
                var currentIndex = visibleMonsters.IndexOf(this.Target);
                
                // Go to the next target
                currentIndex = (currentIndex + 1) % visibleMonsters.Count;
                this.Target = visibleMonsters[currentIndex];
            }
        }

        private Entity FindClosestMonster()
        {   
            Entity target = null;
            double distance = 999999;

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