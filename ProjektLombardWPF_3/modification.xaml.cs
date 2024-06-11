using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace ProjektLombardWPF_3
{
    /// <summary>
    /// Logika interakcji dla klasy modification.xaml
    /// </summary>
    public partial class modification : Window
    {
        DataGrid dataGrid;
        BaseModel baseModel = new BaseModel();

        public modification(DataGrid dataGrid1)
        {
            InitializeComponent();
            dataGrid = dataGrid1;
            baseModel.WriteDateModyfy(dataGrid1, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string drivingLicense = "";
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

            baseModel.ModifyBase(Name_txt.Text.ToString(), Surname_txt.Text.ToString(), drivingLicense, gender, Convert.ToInt32(phone.Text), dataGrid);
            baseModel.NextPage(dataGrid);
        }
        
    }
}
