namespace TheFinalBattle;

class Party(List<Character> members)
{
    public IReadOnlyList<Character> Members => members;
}


class Character
{
    public virtual IReadOnlyList<Action> Actions { get; } = new List<Action>();
    public virtual string Name => "GENERIC CHARACTER";

    public void RandomAction()
    {
        if (Actions.Count <= 0) return;

        var actionIdx = Random.Shared.Next(Actions.Count);
        Actions[actionIdx].Act(this, null);
    }
}

class CharacterSkeleton : Character
{
    public override IReadOnlyList<Action> Actions { get; } = [new ActionDoNothing()];
    public override string Name => "SKELETON"; // TODO: Do as attribute?
}

class CharacterTrueProgrammer(string programmerName) : Character
{
    public override IReadOnlyList<Action> Actions { get; } = [new ActionDoNothing()];
    public override string Name => programmerName;
}