using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            //The game should work with a "battlefield" of any size, including a non-square matrix - DONE
            Grid grid = new Grid(6, 5);

            CharacterClass playerCharacterClass;

            GridBox PlayerCurrentLocation;

            GridBox EnemyCurrentLocation;

            Character PlayerCharacter;

            Character EnemyCharacter;

            List<Character> AllPlayers = new List<Character>();

            int currentTurn = 0;

            int numberOfPossibleTiles = grid.grids.Count;

            Setup();


            void Setup()
            {
                GetPlayerChoice();
            }


            void GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.

                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }
            }


            void SetCharacterName()
            {
                Console.WriteLine("Set the players Name:");
                //store the player choice in a variable
                string charName = Console.ReadLine();
                if (string.IsNullOrEmpty(charName))
                    SetCharacterName();
                else
                    PlayerCharacter.Name = charName;
            }
            

            void CreatePlayerCharacter(int classIndex)
            {

                CharacterClass characterClass = (CharacterClass)classIndex;
                playerCharacterClass = characterClass;
                Console.WriteLine($"Player Class Choice: {playerCharacterClass}\n");
                PlayerCharacter = new Character(characterClass);
                PlayerCharacter.Health = 100;
                PlayerCharacter.BaseDamage = 15;
                PlayerCharacter.PlayerIndex = 0;
                SetCharacterName();

                CreateEnemyCharacter();

            }


            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                int randomInteger = GetRandomInt(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}\n");
                EnemyCharacter = new Character(enemyClass);
                EnemyCharacter.Health = 100;
                EnemyCharacter.BaseDamage = 15;
                EnemyCharacter.PlayerIndex = 1;

                string enemyName = "";
                for (int i = PlayerCharacter.Name.Length - 1; i >= 0; i--)
                {
                    string l = PlayerCharacter.Name[i].ToString();
                    enemyName += i % 2 == 0 ? l : l.ToUpper();
                }
                EnemyCharacter.Name = enemyName;

                StartGame();

            }


            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                AlocatePlayers();

                StartTurn();

            }


            void StartTurn()
            {

                Console.WriteLine($"==================== TURN {(currentTurn + 1)} ==================\n");

                foreach (Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                Console.WriteLine($"==================== END OF TURN {(currentTurn + 1)} ==================\n");

                currentTurn++;
                HandleTurn();
            }


            void HandleTurn()
            {
                //The game should inform the player when the battle is over and which team has been declared victorious - DONE
                if (
                    (PlayerCharacter.Status == AbnormalStatus.died || PlayerCharacter.Health <= 0) &&
                    (EnemyCharacter.Status == AbnormalStatus.died || EnemyCharacter.Health <= 0)
                )
                {
                    Console.WriteLine("DRAW - All players is died");
                    return;
                }
                else
                if (PlayerCharacter.Status == AbnormalStatus.died || PlayerCharacter.Health <= 0)
                {
                    Console.WriteLine($"{EnemyCharacter.Name} win!");
                    return;
                }
                else if (EnemyCharacter.Status == AbnormalStatus.died || EnemyCharacter.Health <= 0)
                {
                    Console.WriteLine($"{PlayerCharacter.Name} win!");
                    return;
                }
                else
                {
                    PrintCharactersStatus();
                    Console.WriteLine("Click on any key to start the next turn...\n");

                    var key = Console.ReadKey();
                    StartTurn();
                }
            }


            void PrintCharactersStatus()
            {
                Console.WriteLine($"{PlayerCharacter.PlayerIndex} - {PlayerCharacter.Name} - HP:{PlayerCharacter.Health} - [{PlayerCharacter.Status}] \n");
                Console.WriteLine($"{EnemyCharacter.PlayerIndex} - {EnemyCharacter.Name} - HP:{EnemyCharacter.Health} - [{EnemyCharacter.Status}] \n\n");
            }


            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }


            void AlocatePlayers()
            {
                AlocatePlayerCharacter();

            }
            

            void AlocatePlayerCharacter()
            {
                int random = GetRandomInt(0, grid.Size);

                GridBox RandomLocation = (grid.grids.ElementAt(random));
                if (!RandomLocation.ocupied)
                {
                    Console.Write($"{PlayerCharacter.PlayerIndex} - Player {PlayerCharacter.Name} is on position {random}\n");
                    PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                }
                else
                {
                    AlocatePlayerCharacter();
                }
            }


            void AlocateEnemyCharacter()
            {
                int random = GetRandomInt(0, grid.Size);
                GridBox RandomLocation = (grid.grids.ElementAt(random));

                if (!RandomLocation.ocupied)
                {
                    Console.Write($"{EnemyCharacter.PlayerIndex} - Player {EnemyCharacter.Name} is on position {random}\n\n");
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    grid.drawBattlefield(AllPlayers);

                }
                else
                {
                    AlocateEnemyCharacter();
                }


            }

        }
    }
}
