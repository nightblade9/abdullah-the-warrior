using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype1
{
    public static class AttackResolver
    {
        private static Random random = new Random();

        public static int Attacks(Entity attacker, Entity defender)
        {
            var totalDamage = 0;
            var attacksLeft = attacker.NumberOfAttacks;
            
            do
            {                
                var damage = Math.Max(attacker.Strength - defender.Defense, 0);
                totalDamage += damage;
                attacksLeft -= 1;
            } while (attacksLeft > 0);

            defender.Damage(totalDamage);

            if (attacker is Player)
            {
                var player = attacker as Player;
                if (player.Specialization == Specialization.Stunhammer)
                {
                    TryToStun(player, defender);
                }
            }

            return totalDamage;
        }

        public static int Shoots(Entity attacker, Entity defender)
        {
            var damage = Math.Max((int)Math.Floor(attacker.Strength * 0.8f) - defender.Defense, 0);
            defender.Damage(damage);            
            return damage;
        }

        public static void ApplyKnockbacks(Player player, Entity monster, IList<Entity> monsters, IList<Vector2> walls, int damage)
        {
            if (player.Specialization == Specialization.Stunhammer && random.Next(0, 100) <= Player.KnockBackProbability)
            {
                // Primary knockback in the direction of player => monster
                var dx = monster.X - player.X;
                var dy = monster.Y - player.Y;
                int startX = monster.X;
                int startY = monster.Y;
                int stopX = startX + Player.KnockBackDistance * Math.Sign(dx);
                int stopY = startY + Player.KnockBackDistance * Math.Sign(dy);

                // Horrible method but iterates in one direction only, guaranteed
                var minX = Math.Min(startX, stopX);
                var maxX = Math.Max(startX, stopX);
                var minY = Math.Min(startY, stopY);
                var maxY = Math.Max(startY, stopY);

                // Move monster if spaces are clear
                for (var y = minY; y <= maxY; y++) {
                    for (var x = minX; x <= maxX; x++) {
                        // Primary knockback was vertical, go horizontal. Check all spaces and move the player one by one if the space is empty.
                        if (!monsters.Any(m => m.X == x && m.Y == y && m != monster) && !walls.Any(w => w.X == x && w.Y == y))
                        {
                            // One of these is zero so we're really just moving in one direction.
                            monster.X += dx;                            
                            monster.Y += dy;
                        }
                    }
                }

                // Secondary knockback is orthagonal to the primary one (random direction)
                int secondaryDamage = (int)Math.Ceiling(damage * Player.SecondaryKnockbackDamageRatio);

                for (var y = minY; y <= maxY; y++) {
                    for (var x = minX; x <= maxX; x++) {
                        var target = monsters.SingleOrDefault(m => m.X == x && m.Y == y && m != monster);
                        if (target != null) {
                            // Secondary knockback
                            var sign = random.Next(100) <= 50 ? -Player.SecondaryKnockbackDistance : Player.SecondaryKnockbackDistance;
                            if (dx == 0) {
                                // Primary knockback was vertical, go horizontal. Check all spaces and move the player one by one if the space is empty.
                                for (var i = Math.Min(target.X, target.X + sign); i < target.X + sign; i++) {
                                    if (!monsters.Any(m => m.X == i && m.Y == target.Y && m != target) && !walls.Any(w => w.X == x && w.Y == target.Y)) {
                                        target.X += Math.Sign(sign);
                                    }
                                }
                            } else {
                                // Primary was horizontal, go vertical
                                for (var i = Math.Min(target.Y, target.Y + sign); i < target.Y + sign; i++) {
                                    if (!monsters.Any(m => m.X == target.X && m.Y == i && m != target) && !walls.Any(w => w.X == target.X && w.Y == i)) {
                                        target.Y += Math.Sign(sign);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void TryToStun(Player player, Entity monster)
        {
            if (random.Next(100) <= Player.StunProbability)
            {
                monster.Stun(Player.StunTurns);
            }
        }
    }
}