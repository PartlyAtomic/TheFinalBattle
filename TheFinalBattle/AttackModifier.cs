namespace TheFinalBattle;

abstract class AttackModifier(string name)
{
    public string Name => name;
    public abstract AttackInfo ModifyAttack(AttackInfo damageInfo);
}

class StoneSkinModifier : AttackModifier
{
    public StoneSkinModifier() : base("STONE ARMOR")
    {
    }

    public override AttackInfo ModifyAttack(AttackInfo damageInfo)
    {
        var initialDamage = damageInfo.Damage;
        var damage = Math.Clamp(initialDamage - 1, 0, float.MaxValue);

        Console.WriteLine($"{Name} reduced the attack by {initialDamage - damage} point(s).");
        return damageInfo with { Damage = damage };
    }
}

class ObjectSightModifier : AttackModifier
{
    public ObjectSightModifier() : base("Object Sight")
    {
    }

    public override AttackInfo ModifyAttack(AttackInfo damageInfo)
    {
        var initialDamage = damageInfo.Damage;
        var damage = initialDamage;
        if (damageInfo.DamageType == DamageType.Decoding)
        {
            damage = Math.Clamp(initialDamage - 2, 0, float.MaxValue);
            if (initialDamage - damage > 0)
            {
                Console.WriteLine($"{Name} reduced the decoding attack by {initialDamage - damage} point(s).");
            }
        }

        return damageInfo with { Damage = damage };
    }
}