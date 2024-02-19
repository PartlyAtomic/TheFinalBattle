namespace TheFinalBattle;

class Battle(Party heroParty, Party monsterParty)
{
    public void Run()
    {
        PartyType currentPartyType = PartyType.Hero;
        while (true)
        {
            var currentParty = GetParty(currentPartyType);
            foreach (var member in currentParty.Members)
            {
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
            Console.WriteLine("The heroes win and the Uncoded One was defeated!");
        }
        else
        {
            Console.WriteLine("The heroes lost and the Uncoded One's forces have prevailed...");
        }
    }

    private PartyType GetOtherPartyType(PartyType partyType) => (PartyType)(((int)partyType + 1) % 2);
    private Party GetOtherParty(PartyType partyType) => GetParty(GetOtherPartyType(partyType));

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