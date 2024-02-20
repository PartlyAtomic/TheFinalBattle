using static TheFinalBattle.Utilities;

namespace TheFinalBattle;

interface IPlayer
{
    public void PickAction(Character currentCharacter, Party selfParty, Party enemyParty);
}

class ComputerPlayer : IPlayer
{
    public void PickAction(Character currentCharacter, Party selfParty, Party enemyParty)
    {
        List<Character> GetDefaultTarget(IGameAction targetingAction) => targetingAction.TargetType switch
        {
            ActionTargetType.None => [],
            ActionTargetType.Self => [currentCharacter],
            ActionTargetType.SelfParty => [..selfParty.Members],
            ActionTargetType.EnemyParty => [..enemyParty.Members],
            ActionTargetType.SingleEnemy => [enemyParty.Members[Random.Shared.Next(enemyParty.Members.Count)]],
            _ => throw new ArgumentOutOfRangeException()
        };

        // Check if heals needed
        if (ShouldUsePotion(currentCharacter))
        {
            var potions = from item in selfParty.Inventory where item is ItemHealthPotion select item;
            var potion = potions.FirstOrDefault();
            if (potion == null)
            {
                Console.WriteLine($"{currentCharacter.Name} reached for a Health Potion but had none...");
            }
            else
            {
                var potionAction = potion as IGameAction;
                potionAction?.Act(currentCharacter, GetDefaultTarget(potionAction));
                return;
            }
        }
        
        // Check if gear needed
        if (ShouldEquipGear(currentCharacter))
        {
            var availableGear = from item in selfParty.Inventory where item is IEquippable select item as IEquippable;
            var gear = availableGear.FirstOrDefault();
            if (gear != null)
            {
                gear.Equip(currentCharacter);
            }
        }

        var gearSkill = currentCharacter.Equipment?.Skill;
        if (gearSkill != null)
        {
            gearSkill.Act(currentCharacter, GetDefaultTarget(gearSkill));
            return;
        }
        
        // Add logic for choosing target here as well, once action is known
        var action = RandomAction(currentCharacter);
        if (action == null)
        {
            Console.WriteLine($"{currentCharacter.Name} had no available actions...");
            return;
        }

        action.Act(currentCharacter, GetDefaultTarget(action));
    }


    public IGameAction? RandomAction(Character character)
    {
        if (character.Actions.Count <= 0)
        {
            return null;
        }

        // TODO: Centralize RNG
        var actionIdx = Random.Shared.Next(character.Actions.Count);
        return character.Actions[actionIdx];
    }

    bool ShouldUsePotion(Character currentCharacter)
    {
        if (currentCharacter.CurrentHP / currentCharacter.MaxHP < .5)
        {
            return Random.Shared.NextDouble() < .25;
        }

        return false;
    }

    bool ShouldEquipGear(Character currentCharacter)
    {
        if (currentCharacter.Equipment == null)
        {
            return Random.Shared.NextDouble() < .5;
        }

        return false;
    }
}

class HumanPlayer : IPlayer
{
    public void PickAction(Character currentCharacter, Party selfParty, Party enemyParty)
    {
        var itemActions = from item in selfParty.Inventory where item is IGameAction select item as IGameAction;
        List<IGameAction> allActions = [..currentCharacter.Actions, ..itemActions];
        if (allActions.Count == 0)
        {
            Console.WriteLine("No possible actions!");
            return;
        }

        for (var i = 0; i < allActions.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {allActions[i].Name}");
        }

        int choice;
        do
        {
            choice = ReadAnswerOf<int>("What do you want to do?") - 1;
        } while (choice < 0 || choice >= allActions.Count);

        // TODO: Ask for a target depending on action's target type
        var action = allActions[choice];
        List<Character> defaultTarget = action.TargetType switch
        {
            ActionTargetType.Self => [currentCharacter],
            ActionTargetType.SingleEnemy => [enemyParty.Members[0]],
            ActionTargetType.SelfParty => [currentCharacter],
            _ => throw new NotImplementedException()
        };
        allActions[choice].Act(currentCharacter, defaultTarget);
    }
}