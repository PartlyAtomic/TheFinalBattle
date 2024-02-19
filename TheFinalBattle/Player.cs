namespace TheFinalBattle;

interface IPlayer
{
    public void PickAction(ICharacter currentCharacter, Party selfParty, Party enemyParty);
}

class ComputerPlayer : IPlayer
{
    public void PickAction(ICharacter currentCharacter, Party selfParty, Party enemyParty)
    {
        // Add logic for choosing target here as well, once action is known
        var action = RandomAction(currentCharacter);
        if (action == null)
        {
            Console.WriteLine($"{currentCharacter.Name} had no available actions...");
            return;
        }

        List<ICharacter> targets = action.TargetType switch
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

    public IAction? RandomAction(ICharacter character)
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
    public void PickAction(ICharacter currentCharacter, Party selfParty, Party enemyParty)
    {
        throw new NotImplementedException();
    }
}