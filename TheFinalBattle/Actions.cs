namespace TheFinalBattle;

interface IAction
{
    public void Act(ICharacter instigator, List<ICharacter>? targets);
}

class ActionDoNothing : IAction
{
    public void Act(ICharacter instigator, List<ICharacter>? targets)
    {
        Console.WriteLine($"{instigator.Name} did NOTHING");
    }
}