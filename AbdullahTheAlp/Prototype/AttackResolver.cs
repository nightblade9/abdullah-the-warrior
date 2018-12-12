using System;

namespace DeenGames.AbdullahTheAlp.Prototype
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

        private static void TryToStun(Player player, Entity monster)
        {
            if (random.Next(100) <= Player.StunProbability)
            {
                monster.Stun(Player.StunTurns);
            }
        }
    }
}