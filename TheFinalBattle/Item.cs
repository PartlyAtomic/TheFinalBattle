namespace TheFinalBattle;

interface IItem
{
    public event Action<IItem> EventConsumed;
}

class ItemHealthPotion : IItem, IAction
{
    public event Action<IItem> EventConsumed = _ => { };
    public string Name => "Health Potion";
    public ActionTargetType TargetType => ActionTargetType.SelfParty;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        var targetName = instigator == target ? "self" : target.Name;
        Console.WriteLine($"{instigator.Name} used {Name} on {targetName}");
        target.ApplyHealing(10);
        EventConsumed?.Invoke(this);
        Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
    }
}