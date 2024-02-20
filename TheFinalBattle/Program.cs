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
var heroParty = new Party(players.player1, [new CharacterTrueProgrammer(programmerName)]);
var monsterParty1 = new Party(players.player2, [new CharacterSkeleton()]);
var monsterParty2 = new Party(players.player2, [new CharacterSkeleton(), new CharacterSkeleton()]);
var uncodedOneParty = new Party(players.player2, [new CharacterUncodedOne()]);
List<Party> monsterParties = [monsterParty1, monsterParty2, uncodedOneParty];

Battle.RunSeries(heroParty, monsterParties);