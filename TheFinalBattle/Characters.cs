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
        Inventory = new List<IItem>();

        foreach (var member in _members)
        {
            member.Party = this;
            member.EventDeath += OnMemberDied;
        }

        foreach (var item in inventory)
        {
            AddInventoryItem(item);
        }
    }

    public void AddInventoryItem(IItem item)
    {
        Inventory.Add(item);
        item.EventConsumed += OnItemConsumed;
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

    public List<IGameAction> Actions { get; protected init; } = [];
    public string Name { get; protected init; } = "Default Character";

    public IEquippable? Equipment { get; set; }

    public Party Party { get; set; }

    protected Character(float maxHP, IEnumerable<IGameAction> actions, string name, IEquippable? equipment = null)
    {
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        Actions = actions.ToList();
        Name = name;
        equipment?.Equip(this);
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
    public CharacterSkeleton(IEquippable? equipment = null) : base(5, [new ActionBoneCrunch()], "SKELETON", equipment)
    {
    }
}

class CharacterTrueProgrammer : Character
{
    public CharacterTrueProgrammer(string programmerName) : base(25, [new ActionPunch(), new ActionDoNothing()],
        programmerName, new Sword())
    {
    }
}

class CharacterUncodedOne : Character
{
    public CharacterUncodedOne() : base(15, [new ActionUnraveling()], "The Uncoded One")
    {
    }
}