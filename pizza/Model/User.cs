namespace Model
{
    internal struct User
    {
        public string name;
        public long userId;
        public string deliveryAdress;
        public string phoneNumber;
        public string comment;
        public string order;
        public string data_time;
        public bool readyToOrder;
        public bool ifHaveCommand;
        public override string ToString()
        {
            return $"Iм'я: {name}\n" +
                $"Номер телефону: {phoneNumber}\n" +
                $"Адреса доставки: {deliveryAdress}\n" +
                $"Коментар: {comment}";
        }
        public string GetShortUserString()
        {
            return $"Iм'я: {name}\n" +
                $"Номер телефону: {phoneNumber}\n" +
                $"Адреса доставки: {deliveryAdress}\n";
        }
        public void Clear()
        {
            name = "";
            deliveryAdress = "";
            phoneNumber = "";
            comment = "";
            order = "";
            data_time = "";
            readyToOrder = false;
            ifHaveCommand = false;
        }
        public string Data_Time()
        {
            data_time = DateTime.Now.ToString();
            return data_time;
        }
        public string ThisOrder()
        {
            return ($"{order}" +
                $"\n----------\n" +
                $"{ToString()}\nНомер замовлення: {userId} {Data_Time()}");
        }
    }
}