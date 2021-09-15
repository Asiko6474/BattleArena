using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Game
    {
        private Entity _player;
        private Entity[] _enemies;
        private bool _gameOver = false;
        private int _currentScene;
        private int _currentEnemyIndex = -1;
        private Entity _currentEnemy;
        private string _playerName;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();
            while (!_gameOver)
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
            _gameOver = false;
            _currentScene = 0;
            InitializeEnemies();
        }

        public void InitializeEnemies()
        {
            _currentEnemyIndex = 0;
            Entity Fraawg = new Entity("Frawwg", 10, 15, 34);

            Entity Phil = new Entity("Phil", 15, 34, 20);

            Entity Mike = new Entity("Mike", 34, 20, 10);

            _enemies = new Entity[] { Fraawg, Phil, Mike };

            _currentEnemy = _enemies[_currentEnemyIndex];
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
            if (_currentScene == 0)
            {
                DisplayMainMenu();
            }
            if (_currentScene == 1)
            {
                Battle();
                CheckBattleResults();
            }
            if (_currentScene == 2)
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
            _currentScene = 1;
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Please enter your name");
            Console.Write("> ");
            _playerName = Console.ReadLine();
            Console.WriteLine("Nice to meet you " + _playerName);
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
                Console.WriteLine("You're a wizard " + _playerName);
                _player = new Entity(_playerName, 50, 350000, 2300000);
                _currentScene++;
            }
            else if (input == 2)
            {
                Console.WriteLine("You're a grand knight " + _playerName);
                _player = new Entity(_playerName, 100, 74000, 230000);
                _currentScene++;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine("name: " + character.Name);
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("Attack: " + character.AttackPower);
            Console.WriteLine("Defense: " + character.DefensePower);
        }


        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;
            while (_currentEnemy.Health > 0)
            {
                //display player stats
                DisplayStats(_player);

                //display enemy stats
                DisplayStats(_currentEnemy);
                int input = GetInput("What do you do?", "Attack", "no");
                {
                    if (input == 1)
                    {
                        damageDealt = _player.Attack(_currentEnemy);
                        
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
            if (_currentEnemyIndex >= 2)
            {
                _currentScene = 2;
            }

            else if (_currentEnemy.Health <= 0)
            {
                _currentEnemyIndex++;
                _currentEnemy = _enemies[_currentEnemyIndex];
            }

        }
        void DisplayRestartMenu()
        {
            int choice = GetInput("Would you like to play again?", "Yes", "No");
            if (choice == 1)
            {
                InitializeEnemies();
                _currentScene = 0;
            }
            else if (choice == 2)
            {
                _gameOver = true;
            }
        }
    }
}
