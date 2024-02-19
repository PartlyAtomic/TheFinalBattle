namespace TheFinalBattle;


class Battle(Party heroParty, Party monsterParty)
{
    public void Run()
    {
        PartyType currentParty = PartyType.Hero;
        while (true)
        {
            foreach (var member in GetParty(currentParty).Members)
            {
                Console.WriteLine($"It is {member.Name}'s turn...");
                member.RandomAction();
                Thread.Sleep(500);
            }
            
            // Rotate to next party
            currentParty = (PartyType)(((int)currentParty + 1) % 2);
            
            Console.WriteLine("----------");
        }
    }

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