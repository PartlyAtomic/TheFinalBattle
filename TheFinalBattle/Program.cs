using static TheFinalBattle.Utilities;
using TheFinalBattle;

// Setup parties
var programmerName = ReadAnswer("What is your name, True Programmer?") ?? "True Programmer";
var heroParty = new Party(new ComputerPlayer(), [new CharacterTrueProgrammer(programmerName)]);
var monsterParty1 = new Party(new ComputerPlayer(), [new CharacterSkeleton()]);
var monsterParty2 = new Party(new ComputerPlayer(), [new CharacterSkeleton(), new CharacterSkeleton()]);
List<Party> monsterParties = [monsterParty1, monsterParty2];

Battle.RunSeries(heroParty, monsterParties);

