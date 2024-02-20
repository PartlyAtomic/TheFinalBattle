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
        // Add logic for choosing target here as well, once action is known
        var action = RandomAction(currentCharacter);
        if (action == null)
        {
            Console.WriteLine($"{currentCharacter.Name} had no available actions...");
            return;
        }

        List<Character> targets = action.TargetType switch
        {
            ActionTargetType.None => [],
            ActionTargetType.Self => [currentCharacter],
            ActionTargetType.SelfParty => [..selfParty.Members],
            ActionTargetType.EnemyParty => [..enemyParty.Members],
            ActionTargetType.SingleEnemy => [enemyParty.Members[Random.Shared.Next(enemyParty.Members.Count)]],
            _ => throw new ArgumentOutOfRangeException()
        };

        action.Act(currentCharacter, targets);
    }

    public IAction? RandomAction(Character character)
    {
        if (character.Actions.Count <= 0)
        {
            return null;
        }

        // TODO: Centralize RNG
        var actionIdx = Random.Shared.Next(character.Actions.Count);
        return character.Actions[actionIdx];
    }
}

class HumanPlayer : IPlayer
{
    public void PickAction(Character currentCharacter, Party selfParty, Party enemyParty)
    {
        if (currentCharacter.Actions.Count == 0)
        {
            Console.WriteLine("No possible actions!");
            return;
        }

        for (var i = 0; i < currentCharacter.Actions.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {currentCharacter.Actions[i].Name}");
        }

        int choice;
        do
        {
            choice = ReadAnswerOf<int>("What do you want to do?") - 1;
        } while (choice < 0 || choice >= currentCharacter.Actions.Count);

        // TODO: Ask for a target depending on action's target type
        // For now nothing will break if the first member of the enemy party is always selected

        currentCharacter.Actions[choice].Act(currentCharacter, [enemyParty.Members[0]]);
    }
}