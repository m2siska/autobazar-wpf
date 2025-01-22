using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Autobazar2
{
    public partial class MainWindow : Window
    {
        Autobazar autobazar;
        List<Vikend> vikend = new List<Vikend>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FSButton(object sender, RoutedEventArgs e)
        {
            if (autobazar != null && autobazar.Auto != null && autobazar.Auto.Count > 0)
            {
                autobazar.Auto.Clear();
            }
            if (vikend != null && vikend.Count > 0)
            {
                vikend.Clear();
            }
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "XML File | *.xml";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            bool? success = fileDialog.ShowDialog();
            string path = fileDialog.FileName;
            bool serialization = false;

            if (success == true)
            {
                path = fileDialog.FileName;
                DeserializeXML(path);
                serialization = true;
            }


            if (serialization == true)
            {

            prodejeaut.ItemsSource = autobazar.Auto;
            prodejeaut.Columns[2].Visibility = Visibility.Hidden;
            DataGridTextColumn cenaImport = prodejeaut.Columns[3] as DataGridTextColumn;
                if (cenaImport != null)
                {
                    Binding newBinding = new Binding
                    {
                        Path = ((Binding)cenaImport.Binding).Path,
                        StringFormat = "N",
                        ConverterCulture = CultureInfo.CreateSpecificCulture("cs-CZ")
                    };
                    cenaImport.Binding = newBinding;
                }

                CenaDPH(vikend);
            prodejVikend.ItemsSource = vikend;
            DataGridTextColumn vikendDPH = prodejVikend.Columns[1] as DataGridTextColumn;
                if (vikendDPH != null)
                {
                    Binding newBinding = new Binding
                    {
                        Path = ((Binding)vikendDPH.Binding).Path,
                        StringFormat = "N",
                        ConverterCulture = CultureInfo.CreateSpecificCulture("cs-CZ")
                    };
                    vikendDPH.Binding = newBinding;
                }
            }

        }
        public void DeserializeXML(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Autobazar));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                autobazar = (Autobazar)serializer.Deserialize(fs);
            }

        }

        public void CenaDPH(List<Vikend> vikend)
        {
            foreach (var item in autobazar.Auto)
            {
                if (item.DatumStruct.DayOfWeek == DayOfWeek.Saturday || item.DatumStruct.DayOfWeek == DayOfWeek.Sunday)
                {
                vikend.Add(new Vikend(item.Model, item.Cena, item.Cena + (item.Cena * item.DPH / 100)));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu si přejete ukončit program?", "Konec",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void prodejVikend_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[prodejVikend.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }
    }
}
