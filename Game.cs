using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    //TESTING TESTING  test commit
    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        Character Slime;
        Character Zombie;
        Character Sidolis;
        Character player;
        Character[] enemies;
        bool gameOver = false;
        int currentScene;
        public int currentEnemyIndex = -1;
        private Character currentEnemy;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();
            while (!gameOver)
            {
                Update();
            }
            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            Slime.name = "Slime";
            Slime.health = 10;
            Slime.attackPower = 1;
            Slime.defensePower = 0;

            Zombie.name = "Zombie";
            Zombie.health = 15;
            Zombie.attackPower = 5;
            Zombie.defensePower = 2;

            Sidolis.name = "Lodis's evil cousin, Sidolis";
            Sidolis.health = 25;
            Sidolis.attackPower = 10;
            Sidolis.defensePower = 5;

            enemies = new Character[] { Slime, Zombie, Sidolis };
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Goodbye");
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
                }

                Console.Clear();
            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            if (currentScene == 0)
            {
                DisplayMainMenu();
            }
            if (currentScene == 1)
            {
                Battle();
                CheckBattleResults();
            }
            if (currentScene == 2)
            {
                DisplayRestartMenu();
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            GetPlayerName();
            CharacterSelection();
            currentScene = 1;
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Please enter your name");
            Console.Write("> ");
            player.name = Console.ReadLine();
            Console.WriteLine("Nice to meet you " + player.name);
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput("please select your class", "Wizard", "Knight");
            if (input == 1)
            {
                Console.WriteLine("You're a wizard " + player.name);
                player.health = 50;
                player.attackPower = 25;
                player.defensePower = 5;
            }
            else if (input == 2)
            {
                Console.WriteLine("You're a grand knight " + player.name);
                player.health = 75;
                player.attackPower = 15;
                player.defensePower = 10;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine("name: " + character.name);
            Console.WriteLine("Health: " + character.health);
            Console.WriteLine("Attack: " + character.attackPower);
            Console.WriteLine("Defense: " + character.defensePower);
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            float damage = attackPower - defensePower;

            if (damage <= 0)
            {
                damage = 0;
            }
            return damage;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public float Attack(ref Character attacker, ref Character defender)
        {
            float damageTaken = CalculateDamage(attacker.attackPower, defender.defensePower);
            defender.health -= damageTaken;
            return damageTaken;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {

            while (currentEnemy.health > 0)
            {
                //display player stats
                DisplayStats(player);

                //display enemy stats
                DisplayStats(currentEnemy);
                int input = GetInput("What do you do?", "Attack", "no");
                {
                    if (input == 1)
                    {
                        Attack(ref player, ref currentEnemy);
                        Attack(ref currentEnemy, ref player);
                    }
                    if (input == 2)
                    {
                        Console.WriteLine("You two sit down and look at each other.");
                    }
                }

            }
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if (currentEnemyIndex >= 2)
            {
                currentScene = 2;
            }

            else if (currentEnemy.health <= 0)
            {
                currentEnemyIndex++;
                currentEnemy = enemies[currentEnemyIndex];
            }

        }
        void DisplayRestartMenu()
        {
            int choice = GetInput("Would you like to play again?", "Yes", "No");
            if (choice == 1)
            {
                currentEnemyIndex = -1;
                currentScene = 0;
            }
            else if (choice == 2)
            {
                gameOver = true;
            }
        }
    }
}
