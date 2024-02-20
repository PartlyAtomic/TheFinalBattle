namespace TheFinalBattle;

interface IItem
{
    public event Action<IItem> EventConsumed;

    public string Name { get; }
}

interface IEquippable : IItem
{
    void Equip(Character character);

    void Unequip(Character character);

    IGameAction Skill { get; }
}

class ItemHealthPotion : IItem, IGameAction
{
    public event Action<IItem> EventConsumed = _ => { };
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

// When equipped add skill to character skills
class Sword : IEquippable, IGameAction
{
    public event Action<IItem>? EventConsumed;

    string IGameAction.Name => Equipped switch
    {
        false => "Equip Sword",
        true => "Unequip Sword"
    };

    string IItem.Name => "Sword";
    

    public ActionTargetType TargetType => ActionTargetType.Self;
    public bool Equipped = false;
    public IGameAction? Skill => new SwordSkillSlash();

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

        Console.WriteLine($"{instigator.Name} used {Name} on {target.Name}");
        Console.WriteLine($"{Name} dealt 2 damage to {target.Name}");
        target.ApplyDamage(2);
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}

class Dagger : IEquippable, IGameAction
{
    public event Action<IItem>? EventConsumed;

    string IGameAction.Name => Equipped switch
    {
        false => "Equip Dagger",
        true => "Unequip Dagger"
    };
    
    string IItem.Name => "Dagger";


    public ActionTargetType TargetType => ActionTargetType.Self;
    public bool Equipped = false;
    public IGameAction? Skill => new DaggerSkillStab();

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
        Console.WriteLine($"{Name} dealt 1 damage to {target.Name}");
        target.ApplyDamage(1);
        if (target.CurrentHP > 0)
        {
            Console.WriteLine($"{target.Name} is now at {target.CurrentHP}/{target.MaxHP}");
        }
    }
}