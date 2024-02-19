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
interface IAction
{
    public ActionTargetType TargetType { get; }

    public void Act(Character instigator, List<Character>? targets);
}

record AttackInfo(float ExpectedDamage);

interface IAttackAction
{
    AttackInfo GetAttackInfo();
}

class ActionDoNothing : IAction
{
    public ActionTargetType TargetType => ActionTargetType.None;

    public void Act(Character instigator, List<Character>? targets)
    {
        Console.WriteLine($"{instigator.Name} did NOTHING");
    }
}

class ActionPunch : IAction, IAttackAction
{
    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used PUNCH on {target.Name}");
        Console.WriteLine($"PUNCH dealt 1 damage to {target.Name}");
        target.ApplyDamage(1);
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

class ActionBoneCrunch : IAction, IAttackAction
{
    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used BONE CRUNCH on {target.Name}");

        var damage = Random.Shared.Next(2);
        Console.WriteLine($"BONE CRUNCH dealt {damage} damage to {target.Name}");
        target.ApplyDamage(damage);
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