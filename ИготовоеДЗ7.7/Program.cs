using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

// Выполняю это задание с 3 попытки, прошлую умудрился стереть, далось с огромным трудом.
// Задание абстрактное, решение оставил таким же, было очень трудно, особенно с придумыванием логики.
// Кое-где оставил комментарии, что я пытался показать,правильно ли я понял о чем читал в теории.
// Буду рад прочитать любой фидбэк,в том числе с готовым вариантом решения, если это неверно
// Можете написать куда угоднохоть в пачку,обязательно прочитаю.

namespace ИготовоеДЗ_7._7
{
    static class Adress
    {
        public static string adress;
        static Adress() { adress = string.Empty; }
        // Правильно ли использовал метод расширения?
        public static string Edit(this string oldadr, string newadr)
        {
            oldadr = oldadr + newadr;
            adress = oldadr;
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
        private readonly LogisticCompany company;
        public readonly string Name = "boxberry";
        public readonly int PhoneNumber = 1234;
        public HomeDelivery(string type) : base(type)
        {
            company = new LogisticCompany(Name, PhoneNumber);
            Console.WriteLine("Info: {0}, {1}", company.Name, company.PhoneNumber);
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
        private readonly LogisticCompany comp;
        public ShopDelivery(LogisticCompany company, string type) : base(type) 
        {
            comp = company;
            Console.WriteLine("Info: {0}, {1}", comp.Name, comp.PhoneNumber);
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
        public Order()
        {
            OrderID = 0;
            Description = string.Empty;
        }
        public void DisplayAddress()
        {
            Console.WriteLine("Информация о заказе: \nномер {0}", OrderID);
            Console.WriteLine("Адрес: " + Adress.adress);
        }
        public static Order<TDelivery> operator ++(Order<TDelivery> a)
        {
            Order<TDelivery> order = new Order<TDelivery>();
            order.OrderID += 1;
            return order;
        }
        public static Order<TDelivery> operator --(Order<TDelivery> b)
        {
            Order<TDelivery> order2 = new Order<TDelivery>();
            if(order2.OrderID > 0) order2.OrderID -= 1;
            return order2;
        }
        public static void Return<T>(int number) where T : struct
        {
            Console.WriteLine("возврат товара, номер: {0}", number);
            Product<T> product = new Product<T>();
            product.Count = 0;
        }
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
    class Program
    {
        static void Main(string[] args)
        {
            string type = "Доставка курьером";
            Adress.adress = "г. Забытый, ул. Заброшенная, д. 1 ".Edit("корпус 4");
            int number = 10;
            int item1 = 1, item2 = 2;

            Product<int> product = new Product<int>();
            product.Name = "first";
            product.Description = "description";
            Product<int>.Swap(ref item1,ref item2);
            Order<HomeDelivery> order = new Order<HomeDelivery> { OrderID = 1 };
            var variable = order.Delivery = new HomeDelivery(type);
            order.DisplayAddress();
            Console.WriteLine("Тип доставки: {0}", variable.TypeDelivery);
            order++;
            Console.WriteLine(order.OrderID);
            // не понял ошибку в 163 строке:
            // PremiumService<HomeDelivery> service = new PremiumService<HomeDelivery> ();
            // service.ExpressDelivery();
            Order<HomeDelivery>.Return<int>(number);
        }
    }
}
