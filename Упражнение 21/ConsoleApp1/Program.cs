using System;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        // Создаем XML-документ
        var xmlDoc = new XElement("автомобили");

        // Добавляем каждый автомобиль в XML
        DodajAvtomobil(xmlDoc, "Ford", "FIESTA HATCHBACK", "5 мест", "$20000");
        DodajAvtomobil(xmlDoc, "Honda", "CR-V", "6 мест", "$30000");
        DodajAvtomobil(xmlDoc, "Opel", "CROSSLAND", "5 мест", "$25000");

        // Сохраняем XML-документ в файл
        xmlDoc.Save("автомобили.xml");

        // Выводим XML-документ на консоль
        Console.WriteLine(xmlDoc);

        // Изменяем цену автомобиля
        IzmeniCenuAvtomobila(xmlDoc);

        // Выводим измененный XML-документ на консоль
        Console.WriteLine("\nИзмененный XML-документ:");
        Console.WriteLine(xmlDoc);
        Console.ReadLine();
    }

    static void DodajAvtomobil(XElement root, string marka, string model, string mesta, string cena)
    {
        // Создаем элемент "автомобиль"
        var avtomobil = new XElement("автомобиль");

        // Создаем элементы для каждого параметра автомобиля
        avtomobil.Add(new XElement("Марка", marka));
        avtomobil.Add(new XElement("Модель", model));
        avtomobil.Add(new XElement("Количество_мест", mesta));
        avtomobil.Add(new XElement("Цена", cena));

        // Добавляем автомобиль в корневой элемент
        root.Add(avtomobil);
    }

    static void IzmeniCenuAvtomobila(XElement xmlDoc)
    {
        Console.Write("\nИзменить цену автомобиля (Введите марку): ");
        string marka = Console.ReadLine();

        // Находим элемент с заданной маркой
        XElement avtomobil = xmlDoc.Elements("автомобиль").First(e => e.Element("Марка").Value == marka);

        if (avtomobil != null)
        {
            Console.Write("Введите новую цену: ");
            string novaCena = Console.ReadLine();

            // Изменяем значение цены
            avtomobil.Element("Цена").Value = novaCena;

            Console.WriteLine("Цена автомобиля успешно изменена.");
        }
        else
        {
            Console.WriteLine("Автомобиль с указанной маркой не найден.");
        }
    }
}
