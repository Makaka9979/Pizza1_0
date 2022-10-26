using System;

namespace Model {
    internal class Menu {
        public string Link { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int Price { get; set; }
        public override string ToString()
        {
            return ($"• {Name}, {Price} грн");
        }
        public Menu(string link = "", string name = "Pizza", ushort price = 1, string text = "0") {
            Link = link;
            Name = name;
            Text = text;
            Price = price;
        }
    }
}