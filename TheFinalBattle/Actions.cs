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

record AttackInfo(float ExpectedDamage);

interface IAttackAction
{
    AttackInfo GetAttackInfo();
}

class ActionDoNothing : IGameAction
{
    public string Name => "NOTHING";
    public ActionTargetType TargetType => ActionTargetType.None;

    public void Act(Character instigator, List<Character>? targets)
    {
        Console.WriteLine($"{instigator.Name} did {Name}");
    }
}

class ActionPunch : IGameAction, IAttackAction
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
        var damage = target.ApplyDamage(1);
        Console.WriteLine($"{Name} dealt {damage} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }

    public AttackInfo GetAttackInfo()
    {
        return new AttackInfo(ExpectedDamage: 1);
    }
}

class ActionBoneCrunch : IGameAction, IAttackAction
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
        var damageDealt = target.ApplyDamage(damage);
        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }

    public AttackInfo GetAttackInfo()
    {
        return new AttackInfo(ExpectedDamage: .5f);
    }
}

class ActionUnraveling : IGameAction, IAttackAction
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

        var damage = Random.Shared.Next(3);
        Console.WriteLine($"{Name} dealt {damage} damage to {target.Name}");
        target.ApplyDamage(damage);
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }

    public AttackInfo GetAttackInfo()
    {
        return new AttackInfo(ExpectedDamage: 1.0f);
    }
}