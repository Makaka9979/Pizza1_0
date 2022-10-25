namespace Model
{
    internal struct User
    {
        public string name;
        public long userId;
        public string deliveryAdress;
        public string phoneNumber;
        public bool payment;
        private string strPayment;
        public string comment;
        public string order;
        public string data_time;
        public bool readyToOrder;
        public override string ToString()
        {
            if (payment)
            {
                strPayment = "cash";
            }
            else
            {
                strPayment = "card";
            }
            return $"Iм'я: {name}\n" +
                $"Номер телефону: {phoneNumber}\n" +
                $"Адреса доставки: {deliveryAdress}\n" +
                $"Спосiб оплати: {strPayment}\n" +
                $"Коментар: {comment}";
        }
        public void Clear()
        {
            name = "";
            deliveryAdress = "";
            phoneNumber = "";
            strPayment = "";
            comment = "";
            order = "";
            data_time = "";
            readyToOrder = false;
        }
    }
}