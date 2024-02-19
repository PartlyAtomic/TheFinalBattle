// Set up parties here

var heroParty = new Party([new CharacterSkeleton()]);
var monsterParty = new Party([new CharacterSkeleton()]);
var battle = new Battle(heroParty, monsterParty);
battle.Run();


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

class Party(List<Character> members)
{
    public IReadOnlyList<Character> Members => members;
}

class Battle(Party heroParty, Party monsterParty)
{
    public void Run()
    {
        Console.WriteLine("Running");
        PartyType currentParty = PartyType.Hero;
        while (true)
        {
            foreach (var member in GetParty(currentParty).Members)
            {
                Console.WriteLine($"It is {member.Name}'s turn...");
                member.RandomAction();
                Thread.Sleep(500);
            }
            
            // Rotate to next party
            currentParty = (PartyType)(((int)currentParty + 1) % 2);
            
            Console.WriteLine("----------");
        }
    }

    private Party GetParty(PartyType type) => type switch
    {
        PartyType.Hero => heroParty,
        PartyType.Monster => monsterParty,
        _ => throw new ArgumentOutOfRangeException()
    };

    enum PartyType
    {
        Hero,
        Monster
    }
}