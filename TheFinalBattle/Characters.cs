namespace TheFinalBattle;

class Party(IPlayer player, List<ICharacter> members)
{
    public IPlayer Player => player;
    public IReadOnlyList<ICharacter> Members => members;
}

interface ICharacter
{
    public IReadOnlyList<IAction> Actions { get; }
    public string Name { get; }
}

class CharacterSkeleton : ICharacter
{
    public IReadOnlyList<IAction> Actions { get; } = [new ActionBoneCrunch()];
    public string Name => "SKELETON";
}

class CharacterTrueProgrammer(string programmerName) : ICharacter
{
    public IReadOnlyList<IAction> Actions { get; } = [new ActionPunch()];
    public string Name => programmerName;
}