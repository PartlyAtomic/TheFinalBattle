namespace TheFinalBattle;

interface IItem
{
    public event Action<IItem>? EventConsumed;

    public string Name { get; }
}

class ItemHealthPotion : IItem, IGameAction
{
    public event Action<IItem>? EventConsumed;
    public string Name => "Health Potion";

    public ActionTargetType TargetType => ActionTargetType.Self;

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
        EventConsumed.Invoke(this);
        Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
    }
}

interface IEquippable : IItem
{
    void Equip(Character character);

    void Unequip(Character character);

    IGameAction? Skill { get; }
}

abstract class Equipment(string name, IGameAction skill) : IEquippable, IGameAction
{
    public ActionTargetType TargetType => ActionTargetType.Self;

    public event Action<IItem>? EventConsumed;
    string IItem.Name => name;

    string IGameAction.Name => Equipped switch
    {
        false => "Equip " + (this as IItem).Name,
        true => "Unequip " + (this as IItem).Name
    };

    public IGameAction? Skill => skill;

    public bool Equipped = false;


    public void Equip(Character character)
    {
        // Unequip current weapon
        character.Equipment?.Unequip(character);
        // Remove this from inventory (consume)
        EventConsumed?.Invoke(this);
        // Equip this weapon
        character.Equipment = this;
        // Add skill to character skills
        if (Skill != null)
        {
            character.Actions.Add(Skill);
        }
    }

    public void Unequip(Character character)
    {
        // Unequip current weapon
        if (character.Equipment != this)
        {
            throw new ArgumentException($"Character does not have {this} equipped");
        }

        character.Equipment = null;
        // Add this to inventory
        character.Party.AddInventoryItem(this);
        // Remove skill from character skills
        if (Skill != null)
        {
            var foundSkill = character.Actions.FirstOrDefault(x => x.GetType() == Skill.GetType());
            if (foundSkill != null)
            {
                character.Actions.Remove(foundSkill);
            }
        }
    }

    public void Act(Character instigator, List<Character>? targets)
    {
        if (!Equipped)
        {
            Equip(instigator);
        }
        else
        {
            Unequip(instigator);
        }
    }
}

class Sword : Equipment
{
    public Sword() : base("Sword", new SwordSkillSlash())
    {
    }
}

class SwordSkillSlash : IGameAction
{
    public string Name { get; } = "Slash";
    public ActionTargetType TargetType { get; } = ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        var damageDealt = target.ApplyDamage(new AttackInfo(instigator, 2));
        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}

class Dagger : Equipment
{
    public Dagger() : base("Dagger", new DaggerSkillStab())
    {
    }
}

class DaggerSkillStab : IGameAction
{
    public string Name { get; } = "Stab";
    public ActionTargetType TargetType { get; } = ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");
        var damageDealt = target.ApplyDamage(new AttackInfo(instigator, 1));
        Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}

class VinsBow : Equipment
{
    public VinsBow() : base("Vin's Bow", new BowSkillQuickShot())
    {
    }
}

class BowSkillQuickShot : IGameAction
{
    public string Name { get; } = "Quick Shot";
    public ActionTargetType TargetType { get; } = ActionTargetType.SingleEnemy;

    public void Act(Character instigator, List<Character>? targets)
    {
        var numTargets = targets?.Count ?? 0;
        if (numTargets != 1)
        {
            throw new ArgumentException();
        }

        var target = targets!.First();

        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");
        var missed = Random.Shared.NextDouble() < .5;
        if (missed)
        {
            Console.WriteLine($"{Name} missed the target...");
        }
        else
        {
            var damageDealt = target.ApplyDamage(new AttackInfo(instigator, 2));
            Console.WriteLine($"{Name} dealt {damageDealt} damage to {target.Name}");
            if (target.CurrentHP > 0)
            {
                Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
            }
        }
    }
}