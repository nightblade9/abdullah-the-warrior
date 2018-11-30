using System;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    public static class AttackResolver
    {
        public static int Attacks(Entity attacker, Entity defender)
        {
            var damage = Math.Max(attacker.Strength - defender.Defense, 0);
            defender.Damage(damage);
            return damage;
        }
    }
}