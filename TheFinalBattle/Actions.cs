namespace TheFinalBattle;

enum ActionTargetType
{
    None,
    Self,
    SelfParty,
    EnemyParty,
    SingleEnemy
}

// TODO: Back to base class instead of interface...?
interface IGameAction
{
    public string Name { get; }

    public ActionTargetType TargetType { get; }

    public void Act(Character instigator, List<Character>? targets);
}

enum DamageType
{
    Normal,
    Decoding
}

record AttackInfo(float Damage, DamageType DamageType = DamageType.Normal);

// TODO: Add attack class as default implementation

class ActionDoNothing : IGameAction
{
    public string Name => "NOTHING";
    public ActionTargetType TargetType => ActionTargetType.None;

    public void Act(Character instigator, List<Character>? targets)
    {
        Console.WriteLine($"{instigator.Name} did {Name}");
    }
}

class ActionPunch : IGameAction
{
    public string Name => "PUNCH";

    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();


        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");
        var damageDealt = target.ApplyDamage(new AttackInfo(Damage: 1));
        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}

class ActionBoneCrunch : IGameAction
{
    public string Name => "BONE CRUNCH";

    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");

        var damage = Random.Shared.Next(2);
        var damageDealt = target.ApplyDamage(new AttackInfo(Damage: damage));
        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}

class ActionBite : IGameAction
{
    public string Name => "BITE";

    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");

        var damage = Random.Shared.Next(1);
        var damageDealt = target.ApplyDamage(new AttackInfo(Damage: damage));
        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}

class ActionUnraveling : IGameAction
{
    public string Name => "UNRAVELING";
    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");

        var damage = Random.Shared.Next(4);
        var damageDealt = target.ApplyDamage(new AttackInfo(Damage: damage, DamageType: DamageType.Decoding));

        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}