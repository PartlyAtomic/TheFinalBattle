namespace TheFinalBattle;

class Party
{
    public IPlayer Player;
    private List<Character> _members;
    public IReadOnlyList<Character> Members => _members;
    public List<IItem> Inventory;

    public Party(IPlayer player, List<Character> members, List<IItem> inventory)
    {
        Player = player;
        _members = members;
        Inventory = inventory;

        foreach (var member in _members)
        {
            member.EventDeath += OnMemberDied;
        }

        foreach (var item in inventory)
        {
            item.EventConsumed += OnItemConsumed;
        }
    }

    public void OnMemberDied(Character character)
    {
        _members.Remove(character);
        character.EventDeath -= OnMemberDied;
    }

    public void OnItemConsumed(IItem item)
    {
        Inventory.Remove(item);
        item.EventConsumed -= OnItemConsumed;
    }
}

class Character
{
    public float MaxHP { get; protected init; }
    public float CurrentHP { get; protected set; }

    public event Action<Character> EventDeath = _ => { };

    public IReadOnlyList<IAction> Actions { get; protected init; } = [];
    public string Name { get; protected init; } = "Default Character";

    protected Character(float maxHP, IEnumerable<IAction> actions, string name)
    {
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        Actions = actions.ToList();
        Name = name;
    }

    public void ApplyDamage(float damage)
    {
        if (CurrentHP <= 0)
        {
            return;
        }

        CurrentHP = Math.Clamp(CurrentHP - damage, 0, MaxHP);
        if (CurrentHP == 0)
        {
            Console.WriteLine($"{Name} has been defeated!");
            EventDeath?.Invoke(this);
        }
    }

    public void ApplyHealing(float healing)
    {
        if (CurrentHP <= 0)
        {
            return;
        }

        CurrentHP = Math.Clamp(CurrentHP + healing, 0, MaxHP);
    }
}

class CharacterSkeleton : Character
{
    public CharacterSkeleton() : base(5, [new ActionBoneCrunch()], "SKELETON")
    {
    }
}

class CharacterTrueProgrammer : Character
{
    public CharacterTrueProgrammer(string programmerName) : base(25, [new ActionPunch(), new ActionDoNothing()],
        programmerName)
    {
    }
}

class CharacterUncodedOne : Character
{
    public CharacterUncodedOne() : base(15, [new ActionUnraveling()], "The Uncoded One")
    {
    }
}