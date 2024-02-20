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
            // Transfer items on death
            var onCharacterDeath = (Character deadCharacter, Character killer) =>
            {
                if (deadCharacter.Equipment != null)
                {
                    var equipment = deadCharacter.Equipment;
                    // Behavior of unequip is to place the item back in the party inventory, but that's not desired here
                    // Instead since the character is dead, just removing it directly
                    // deadCharacter.Equipment.Unequip(deadCharacter);
                    deadCharacter.Equipment = null;
                    Console.WriteLine($"{deadCharacter.Name} dropped {equipment.Name} when they perished.");
                    currentParty.AddInventoryItem(equipment);
                }

                killer.Experience += deadCharacter.Experience;
                Console.WriteLine($"{killer.Name} gets {deadCharacter.Experience} XP.");
            };

            foreach (var member in GetOtherParty(currentPartyType).Members)
            {
                member.EventDeath += onCharacterDeath;
            }

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

                // Thread.Sleep(500);
            }

            foreach (var member in GetOtherParty(currentPartyType).Members)
            {
                member.EventDeath -= onCharacterDeath;
            }

            if (GetOtherParty(currentPartyType).Members.Count == 0)
            {
                Console.WriteLine("A party has been incapacitated. Battle over.");
                break;
            }

            // Rotate to next party
            currentPartyType = GetOtherPartyType(currentPartyType);
        }

        PartyType winner;
        if (GetParty(PartyType.Hero).Members.Count > 0)
        {
            winner = PartyType.Hero;
        }
        else
        {
            winner = PartyType.Monster;
        }

        var winningParty = GetParty(winner);
        var losingParty = GetOtherParty(winner);

        List<IItem> acquiredItems = new List<IItem>();

        foreach (var item in losingParty.Inventory)
        {
            winningParty.AddInventoryItem(item);
            acquiredItems.Add(item);
        }

        losingParty.Inventory.Clear();

        if (acquiredItems.Count > 0)
        {
            var itemList = String.Join(", ", (from item in acquiredItems select item.Name).ToList());
            Console.WriteLine($"The following item(s) went to the winning party: {itemList}");
        }

        return winner;
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
        foreach (var member in heroParty.Members)
        {
            Console.WriteLine($"{member.Name} now has {member.Experience} XP");
        }
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
            health = "(" + health.PadLeft(4 + (int)Math.Log10(member.MaxHP) + 1).PadRight(7) + ")";
            var characterStatus = "| " + member.Name;
            if (member.Equipment != null)
            {
                characterStatus += $" [{member.Equipment.Name}]";
            }

            characterStatus += health.PadLeft(40 - characterStatus.Length);
            characterStatus = characterStatus.PadRight(80 - 1) + "|";
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
            health = "(" + health.PadLeft(4 + (int)Math.Log10(member.MaxHP) + 1).PadRight(7) + ")";
            var characterStatus = member.Name;
            if (member.Equipment != null)
            {
                characterStatus += $" [{member.Equipment.Name}]";
            }

            characterStatus += " " + health + " |";
            characterStatus = "|" + characterStatus.PadLeft(80 - 1);
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