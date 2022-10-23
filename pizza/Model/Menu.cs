using System;

namespace Model {
    internal class Menu {
        private string _link = "";
        private string _name = "";
        private string _text = "";
        private long _price = 0;
        private bool _selected = false;
        public string Link { get { return _link; } }
        public string Name { get { return _name; } }
        public string Text { get { return _text; } }
        public long Price { get { return _price; } set { _price = value; } }

        //If the item is out of stock (если товар закончился, ну типа на английском пафаснее что-ли)
        public bool SelectedFalse { set { _selected = false; } }
        public bool SelectedTrue { set { _selected = true; } }

        public Menu(string link = "", string name = "Pizza", long price = 1, string text = "0") {
            _link = link;
            _name = name;
            _text = text;
            _price = price;
            _selected = true;
        }
        public Menu() { Console.WriteLine("class Menu error"); }
    }
}
