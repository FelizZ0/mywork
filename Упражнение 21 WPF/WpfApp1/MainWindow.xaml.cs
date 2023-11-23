using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private const string XmlFilePath = "автомобили.xml";
        private XDocument xmlDoc;

        public MainWindow()
        {
            InitializeComponent();
            LoadXml();
        }

        private void LoadXml()
        {
            try
            {
                if (File.Exists(XmlFilePath))
                {
                    xmlDoc = XDocument.Load(XmlFilePath);
                }
                else
                {
                    xmlDoc = new XDocument(new XElement("автомобили"));
                    xmlDoc.Save(XmlFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке/создании XML файла: {ex.Message}");
            }
        }

        private void ShowCars_Click(object sender, RoutedEventArgs e)
        {
            carListBox.Items.Clear();
            foreach (var avtomobil in xmlDoc.Descendants("автомобиль"))
            {
                carListBox.Items.Add(avtomobil.Element("Марка").Value);
                carListBox.Items.Add(avtomobil.Element("Цена").Value);
            }
        }

        private void UpdatePrice_Click(object sender, RoutedEventArgs eventArgs)
        {
            string brand = brandTextBox.Text;
            string newPrice = priceTextBox.Text;

            XElement avtomobil = xmlDoc.Descendants("автомобиль").FirstOrDefault(e => e.Element("Марка").Value == brand);

            if (avtomobil != null)
            {
                avtomobil.Element("Цена").Value = newPrice;
                MessageBox.Show("Цена автомобиля успешно изменена.");
                SaveXml();
            }
            else
            {
                MessageBox.Show("Автомобиль с указанной маркой не найден.");
            }
        }


        private void SaveXml()
        {
            xmlDoc.Save("автомобили.xml");
        }
    }
}
