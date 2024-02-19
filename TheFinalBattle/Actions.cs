namespace TheFinalBattle;

class Action
{
    public virtual void Act(Character instigator, List<Character>? targets)
    {
    }
}

class ActionDoNothing : Action
{
    public override void Act(Character instigator, List<Character>? targets)
    {
        Console.WriteLine($"{instigator.Name} did NOTHING");
    }
}