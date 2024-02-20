using static TheFinalBattle.Utilities;
using TheFinalBattle;


var mode = ReadAnswerOf<int>("""
                             Choose a gameplay mode:
                             1 - Player vs Computer
                             2 - Player vs Player
                             3 - Computer vs Computer
                             What is your choice?
                             """);

(IPlayer player1, IPlayer player2) players = mode switch
{
    1 => (new HumanPlayer(), new ComputerPlayer()),
    2 => (new HumanPlayer(), new HumanPlayer()),
    3 => (new ComputerPlayer(), new ComputerPlayer()),
    _ => throw new ArgumentOutOfRangeException()
};

// Setup parties
var programmerName = ReadAnswer("What is your name, True Programmer?") ?? "True Programmer";
var heroParty = new Party(players.player1,
    [new CharacterTrueProgrammer(programmerName), new CharacterVinFletcher()],
    [new ItemHealthPotion(), new ItemHealthPotion(), new ItemHealthPotion()]);
var monsterParty1 = new Party(players.player2,
    [new CharacterSkeleton(new Dagger())],
    [new ItemHealthPotion()]);
var monsterParty2 = new Party(players.player2,
    [new CharacterSkeleton(), new CharacterSkeleton()],
    [new ItemHealthPotion(), new Dagger(), new Dagger()]);
var uncodedOneParty = new Party(players.player2,
    [new CharacterUncodedOne()],
    [new ItemHealthPotion()]);
List<Party> monsterParties = [monsterParty1, monsterParty2, uncodedOneParty];

Battle.RunSeries(heroParty, monsterParties);