using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    public enum ItemType
    {
        DEFENSE, 
        ATTACK,
        NONE
    }

    public enum Scene
    {
        STARTMENU,
        NAMECREATION,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
    }
    class Game
    {
        private Player _player;
        private Entity[] _enemies;
        private Entity _currentEnemy;
        private bool _gameOver = false;
        private int _currentScene;
        private int _currentEnemyIndex = -1;
        private string _playerName;
        private Item[] _wizardItems;
        private Item[] _knightItems;


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
            InitializeItems();
        }

        public void InitializeItems()
        {
            //Wizard Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5, Type = ItemType.ATTACK};
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, Type = ItemType.DEFENSE};

            //Knight Items
            Item wand = new Item { Name = "Wand", StatBoost = 1025, Type = ItemType.ATTACK};
            Item shoes = new Item { Name = "Shoes", StatBoost = 5201, Type = ItemType.DEFENSE };

            //Initialize arrays
            _wizardItems = new Item[] { bigWand, bigShield };
            _knightItems = new Item[] { wand, shoes };
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

        public void Save()
        {
            //create a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            //Save current enemy index
            writer.WriteLine(_currentEnemyIndex);

            //Save payer and enemy
            _player.Save(writer);
            _currentEnemy.Save(writer);

            //Close Writer When Done Saving
            writer.Close();
        }

        public bool Load()
        {
            //if file doesn't exist...
            if (!File.Exists("SaveData.txt"))
            {
                //return false
                return false;
            }
            //create a new reader to read from the text file
            StreamReader reader = new StreamReader("SaveData.txt");
            // if the first line can't be converted into an integer
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
            {
                // return false
                return false;
            }
            //Create a new instance and try to load the player
            _player = new Player();

            if (!_player.Load(reader))
            {
                return false;
            }

            //create a new instance and try to load the enemy
            _currentEnemy = new Entity();

            if (!_currentEnemy.Load(reader))
            {
                return false;
            }
            // Update the array to match the current enemy stats
            _enemies[_currentEnemyIndex] = _currentEnemy;

            string job = reader.ReadLine();

            if (job == "wizard")
            {
                _player = new Player(_wizardItems);
            }
            else if (job == "Knight")
            {
                _player = new Player(_knightItems);
            }
            else
            {
                return false;
            }

            reader.Close();

            return true;
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputRecieved = -1;

            while (inputRecieved == -1)
            {
                //print options
                Console.WriteLine(description);
                for (int i = 0; i< options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }
                Console.Write("> ");

                input = Console.ReadLine();

                if (int.TryParse(input, out inputRecieved))
                {
                    inputRecieved--;
                    if (inputRecieved < 0 || inputRecieved > -options.Length)
                    {
                        inputRecieved = 0;
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    inputRecieved = -1;
                    Console.WriteLine("InvalidInput");
                    Console.ReadKey(true);
                }
                Console.Clear();
            }
            return inputRecieved;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            switch(_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;
                case Scene.NAMECREATION:
                    GetPlayerName();
                    break;
                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    break;
                case Scene.BATTLE:
                    Battle();
                    CheckBattleResults();
                    break;
                case Scene.RESTARTMENU:
                    DisplayRestartMenu();
                    break;

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

        public void DisplayStartMenu()
        {
            int choice = GetInput("Welcome to Battle Arena!", "Start New Game", "Load Game");

            if (choice == 0)
            {
                _currentScene = Scene.NAMECREATION;
            }
            else if (choice == 1)
            {
                if (Load())
                {
                    Console.WriteLine("Load Successfull");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
            }
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
            if (input == 0)
            {
                Console.WriteLine("You're a wizard " + _playerName);
                _player = new Player(_playerName, 50, 35, 23, _wizardItems, "Wizard");
                _currentScene++;
            }
            else if (input == 1)
            {
                Console.WriteLine("You're a grand knight " + _playerName, "Knight");
                _player = new Player(_playerName, 100, 74, 23, _knightItems);
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


        public void DisplayEquipItemMenu()
        {

            //Get Item Index
            int choice = GetInput("Select an item to equip", _player.GetItemNames());

            //Equip item at given index
            if (!_player.TryEquipItem(choice))
                Console.WriteLine("You couldn't find that item in your bag.");

            //Print feedback
            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
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
                int input = GetInput("What do you do?", "Attack", "Equip Item", "Remove current item", "Save");
                {
                    if (input == 0)
                    {
                        damageDealt = _player.Attack(_currentEnemy);
                        
                    }
                    else if (input == 1)
                    {
                        DisplayEquipItemMenu();
                        Console.ReadKey(true);
                        Console.Clear();
                        return;
                    }
                    else if (input == 2)
                    {
                        if (!_player.TryremoveCurrentItem())
                        {
                            Console.WriteLine("You don't have anything equipped");
                        }
                        else
                        {
                            Console.WriteLine("You placed the itewm in your bag");
                        }

                        Console.ReadKey(true);
                        Console.Clear();
                        return;
                    }
                    else if (input == 2)
                    {
                        Save();
                        Console.WriteLine("You saved the game!");

                        Console.ReadKey(true);
                        Console.Clear();
                        return;
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
                _currentScene = Scene.RESTARTMENU;
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
            if (choice == 0)
            {
                InitializeEnemies();
                _currentScene = Scene.BATTLE;
            }
            else if (choice == 1)
            {
                _gameOver = true;
            }
        }
    }
}
