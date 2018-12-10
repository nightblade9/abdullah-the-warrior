using System;

namespace DeenGames.AbdullahTheAlp.Prototype
{
    public static class AttackResolver
    {
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
            return totalDamage;
        }

        public static int Shoots(Entity attacker, Entity defender)
        {
            var damage = Math.Max(attacker.Strength - defender.Defense, 0);
            defender.Damage(damage);            
            return damage;
        }
    }
}