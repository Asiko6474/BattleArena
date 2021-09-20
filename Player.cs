using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;
        public override float AttackPower
        {
            get
            {
                if (_currentItem.Type == ItemType.ATTACK)
                    return base.AttackPower + CurrentItem.StatBoost;

                return base.AttackPower;
            }
        }

        public override float DefensePower
        {
            get 
            {
                if (_currentItem.Type == ItemType.DEFENSE)
                    return base.DefensePower + CurrentItem.StatBoost;

                return base.DefensePower;
            }

        }
            
        public string Job
        {
            get
            {
                return _job;
            }
            set
            {
                _job = value;
            }
        }

        

        public Item CurrentItem
        {
            get
            {
                return _currentItem;
            }
        }

        public Player()
        {
            _items = new Item[0];
            _currentItem.Name = "Nothing";
        }
        public Player(Item[] items): base()
        {
            _currentItem.Name = "Nothing";
            _items = items;
        }
        public Player(string name, float health, float attackPower, float defensePower, Item[] items, string job) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
            _job = job;
        }

        /// <summary>
        /// Sets the item at the given index to be the current item
        /// </summary>
        /// <param name="index">The index of the item in the array</param>
        /// <returns>Dalse if the index is outsid ethe bounds of the array</returns>
        public bool TryEquipItem(int index)
        {
            // if the index is out of bounds...
            if (index >= _items.Length || index < 0)
            {
                ///...return false
                return false;
            }


            ///set the current item to be the array at the given index
            _currentItem = _items[index];

            return true;
        }
        /// <summary>
        /// Set the current item to be nothing
        /// </summary>
        /// <returns>False if there is no item equipped</returns>
        public bool TryremoveCurrentItem()
        {
            //Returns false if there is no item equipped
            if (CurrentItem.Name == "Nothing")
            {
                return false;
            }

            _currentItemIndex = -1;
            //set item to be nothing
            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }



        /// <returns>Gets the names of all items in the player inventory </returns>
        public string[] GetItemNames()
        {
            string[] itemNames = new string[_items.Length];

            for (int i = 0; i < _items.Length; i++ )
            {
                itemNames[i] = _items[i].Name;
            }

            return itemNames;
        }
        public override void Save(StreamWriter writer)
        {
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
            writer.WriteLine(_job);
        }

        public override bool Load(StreamReader reader)
        { 
            //if base loading function fails...
            if (!base.Load(reader))
                return false;
            //if the current Line can't be converted into an int...
            if (!int.TryParse(reader.ReadLine(), out _currentItemIndex))
                return false;

            //return wheter or not the item was equipped succesfully
            TryEquipItem(_currentItemIndex);

            return true;
        }
    }
}
