using System;

namespace Controller
{
    internal class Order {
        private Model.Menu? value;

        public override string ToString() {
            return ($"{value.Name} - {value.Price}");
        }
        public Order() { Console.WriteLine("Class Order error"); }
        public Order(Model.Menu? value) { 
            this.value = value; 
        }
    }
}
