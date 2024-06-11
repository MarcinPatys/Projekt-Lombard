using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ProjektLombardWPF_3
{
    /// <summary>
    /// Logika interakcji dla klasy AddWindowView.xaml
    /// </summary>
    public partial class AddWindowView : Window
    {
        BaseModel baseModel = new BaseModel();
        DataGrid dataGrid;
        public AddWindowView(DataGrid dataGrid1)
        {
            InitializeComponent();
            dataGrid = dataGrid1;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string RichTextBox = new TextRange(RichTxt.Document.ContentStart, RichTxt.Document.ContentEnd).Text;

            string drivingLicense ="";
            string gender = "";
            if (A.IsChecked == true)
            {
                drivingLicense += " " + "A";
            }
            if (A1.IsChecked == true)
            {
                drivingLicense += " " + "A1";
            }
            if (A2.IsChecked == true)
            {
                drivingLicense += " " + "A2";
            }
            if (B.IsChecked == true)
            {
                drivingLicense += " " + "B";
            }
            if (C.IsChecked == true)
            {
                drivingLicense += " " + "C";
            }
            if (CE.IsChecked == true)
            {
                drivingLicense += " " + "C+E";
            }
            if (D1.IsChecked == true)
            {
                drivingLicense += " " + "D1";
            }
            if (B1.IsChecked == true)
            {
                drivingLicense += " " + "B1";
            }
            if (T.IsChecked == true)
            {
                drivingLicense += " " + "T";
            }
            if (Mezczynza.IsChecked == true)
            {
                gender = "Mężczyzna";
            }
            if (Kobieta.IsChecked == true)
            {
                gender = "Kobieta";
            }
            
            string richText = new TextRange(RichTxt.Document.ContentStart, RichTxt.Document.ContentEnd).Text;
            MessageBox.Show(richText);
            baseModel.AddDataToBase(Name_txt.Text, Surname_txt.Text,gender,drivingLicense,richText,Convert.ToInt32(phone.Text),dataGrid);
        }      
    }
}
