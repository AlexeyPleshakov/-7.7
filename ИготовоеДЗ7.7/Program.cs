using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ИготовоеДЗ7._7
{
    static class Adress
    {
        public static string adress;
        // Правильно ли использовал метод расширения?
        public static string Edit(this string oldadr, string newadr)
        {
            adress = newadr;
            oldadr = newadr;
            return adress;
        }
    }
    class Product<T>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // Количество товара(счетчик)
        public int Count;
        // Метод обмена товара
        public static void Swap<T>(ref T x, ref T y)
        {
            T t = x;
            x = y;
            y = t;
        }
    }
    class LogisticCompany
    {
        public readonly string Name;
        public readonly int PhoneNumber;
        public LogisticCompany(string name, int phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }
        public static void GetPackage() => Console.WriteLine("выдача посылки"); 
    }
    abstract class Delivery
    {
        private protected string type;
        public Delivery(string type) { this.type = type; }
        public virtual string TypeDelivery {  get; set; }

    }

    class HomeDelivery : Delivery
    {
        // Пример композиции
        private LogisticCompany company;
        public readonly string Name = "boxberry";
        public readonly int PhoneNumber = 1234;
        public HomeDelivery(string type) : base(type)
        {
            company = new LogisticCompany(Name, PhoneNumber);
        }
        public override string TypeDelivery
        {
            get { Console.WriteLine("Доставка на дом \n"); return type; }
            set { type = value; }
        }
    }
    class PickPointDelivery : Delivery
    {
        public PickPointDelivery(string type) : base(type) { }
        public override string TypeDelivery
        {
            get
            {
                Console.WriteLine("Доставка в ПВЗ");
                return type;
            }
            set { value = type; }
        }
    }

    class ShopDelivery : Delivery
    {
        // Агрегация, пример
        private LogisticCompany comp;
        public ShopDelivery(LogisticCompany company, string type) : base(type) 
        {
            comp = company;
        }
        public override string TypeDelivery
        {
            get
            {
                Console.WriteLine("Доставка в магазин");
                return type;
            }
            set { type = value; }
        }
    }

    class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;
        public int OrderID;
        public string Description;
        public Order() { }
        public void DisplayAddress()
        {
            Console.WriteLine("Информация о заказе: \nномер {0}", OrderID);
            Console.WriteLine("Адрес: " + Adress.adress);
            Console.WriteLine("Тип доставки: ", Delivery.TypeDelivery);
        }
        public static void Return<T>(int number) where T : struct
        {
            Console.WriteLine("возврат товара, номер: {0}", number);
            Product<T> product = new Product<T>();
            product.Count = 0;
        }
        class PremiumService<T> where T : Order<HomeDelivery>
        {
            // Не придумал логики с этим полем...
            public T Order;
            public void ExpressDelivery()
            {
                Console.WriteLine("Вызов курьера по указанному адресу");
                LogisticCompany.GetPackage();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
