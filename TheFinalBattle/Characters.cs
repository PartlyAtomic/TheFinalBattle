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
    public IReadOnlyList<IAction> Actions { get; } = [new ActionDoNothing()];
    public string Name => "SKELETON"; // TODO: Do as attribute?
}

class CharacterTrueProgrammer(string programmerName) : ICharacter
{
    public IReadOnlyList<IAction> Actions { get; } = [new ActionDoNothing()];
    public string Name => programmerName;
}