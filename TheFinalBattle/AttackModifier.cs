namespace TheFinalBattle;

abstract class AttackModifier(string name)
{
    public string Name => name;
    public abstract float ModifyAttack(float damage);
}

class StoneSkinModifier : AttackModifier
{
    public StoneSkinModifier() : base("STONE ARMOR")
    {
    }

    public override float ModifyAttack(float damage)
    {
        var initialDamage = damage;
        damage = Math.Clamp(damage - 1, 0, float.MaxValue);

        Console.WriteLine($"{Name} reduced the attack by {initialDamage - damage} point(s).");
        return damage;
    }
}