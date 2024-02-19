using static TheFinalBattle.Utilities;
using TheFinalBattle;

// Setup parties
var programmerName = ReadAnswer("What is your name, True Programmer?") ?? "True Programmer";
var heroParty = new Party([new CharacterTrueProgrammer(programmerName)]);
var monsterParty = new Party([new CharacterSkeleton()]);

// Start battle
var battle = new Battle(heroParty, monsterParty);
battle.Run();