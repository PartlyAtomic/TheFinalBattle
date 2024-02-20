namespace TheFinalBattle;

enum PartyType
{
    Hero,
    Monster
}

class Battle(Party heroParty, Party monsterParty)
{
    // Returns winner
    public PartyType Run()
    {
        PartyType currentPartyType = PartyType.Hero;
        while (true)
        {
            var currentParty = GetParty(currentPartyType);
            foreach (var member in currentParty.Members)
            {
                DisplayStatus(member, heroParty, monsterParty);
                
                Console.WriteLine($"It is {member.Name}'s turn...");
                currentParty.Player.PickAction(member,
                    GetParty(currentPartyType),
                    GetOtherParty(currentPartyType));

                if (GetOtherParty(currentPartyType).Members.Count == 0)
                {
                    break;
                }

                Thread.Sleep(500);
            }

            if (GetOtherParty(currentPartyType).Members.Count == 0)
            {
                Console.WriteLine("A party has been incapacitated. Battle over.");
                break;
            }

            // Rotate to next party
            currentPartyType = GetOtherPartyType(currentPartyType);

            Console.WriteLine("----------");
        }

        if (GetParty(PartyType.Hero).Members.Count > 0)
        {
            return PartyType.Hero;
        }
        else
        {
            return PartyType.Monster;
        }
    }

    public static PartyType RunSeries(Party heroParty, List<Party> monsterParties)
    {
        foreach (var monsterParty in monsterParties)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("Battle is imminent!");
            // Start battle
            var battle = new Battle(heroParty, monsterParty);
            var results = battle.Run();
            if (results == PartyType.Monster)
            {
                Console.WriteLine("The heroes lost and the Uncoded One's forces have prevailed...");
                return PartyType.Monster;
            }
        }

        Console.WriteLine("The heroes win and the Uncoded One was defeated!");
        return PartyType.Hero;
    }

    private void DisplayStatus(Character currentCharacter, Party heroParty, Party monsterParty)
    {
        Console.WriteLine("==================================== BATTLE ====================================");

        foreach (var member in heroParty.Members)
        {
            if (currentCharacter == member)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            var health = $"{member.CurrentHP}/{member.MaxHP}";
            health = "(" + health.PadLeft(7 - health.Length / 2).PadRight(7) + ")";
            var characterStatus = "| " + member.Name + health.PadLeft(40 - member.Name.Length - 2);
            characterStatus = characterStatus.PadRight(80-1) + "|";
            Console.Write(characterStatus);
            
            if (currentCharacter == member)
            {
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------------- VS --------------------------------------");
        foreach (var member in monsterParty.Members)
        {
            if (currentCharacter == member)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            var health = $"{member.CurrentHP}/{member.MaxHP}";
            health = "(" + health.PadLeft(7 - health.Length / 2).PadRight(7) + ")";
            var characterStatus =  member.Name + " " + health + " |";
            characterStatus = "|" + characterStatus.PadLeft(80-1);
            Console.Write(characterStatus);
            
            if (currentCharacter == member)
            {
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }

        Console.WriteLine("================================================================================");
    }

    private PartyType GetOtherPartyType(PartyType partyType) => (PartyType)(((int)partyType + 1) % 2);
    private Party GetOtherParty(PartyType partyType) => GetParty(GetOtherPartyType(partyType));

    private Party GetParty(PartyType type) => type switch
    {
        PartyType.Hero => heroParty,
        PartyType.Monster => monsterParty,
        _ => throw new ArgumentOutOfRangeException()
    };
}