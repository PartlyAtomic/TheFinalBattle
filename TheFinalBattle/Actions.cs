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

    public void Act(ICharacter instigator, List<ICharacter>? targets);
}

class ActionDoNothing : IAction
{
    public ActionTargetType TargetType => ActionTargetType.None;

    public void Act(ICharacter instigator, List<ICharacter>? targets)
    {
        Console.WriteLine($"{instigator.Name} did NOTHING");
    }
}

class ActionPunch : IAction
{
    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(ICharacter instigator, List<ICharacter>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used PUNCH on {target.Name}");
    }
}

class ActionBoneCrunch : IAction
{
    public ActionTargetType TargetType => ActionTargetType.SingleEnemy;

    public void Act(ICharacter instigator, List<ICharacter>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used BONE CRUNCH on {target.Name}");
    }
}