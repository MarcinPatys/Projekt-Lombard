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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektLombardWPF_3
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseModel baseModel = new BaseModel();
        public MainWindow()
        {
            InitializeComponent();
            baseModel.SelectBase(dataGrid1);
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddWindowView addWindowView = new AddWindowView(this.dataGrid1);
            addWindowView.Show();
        }

        private void btn_previous_Click(object sender, RoutedEventArgs e)
        {
            baseModel.PreviousPage(dataGrid1);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            baseModel.NextPage(dataGrid1);
        }

        private void btn_delate_Click(object sender, RoutedEventArgs e)
        {
            baseModel.DeleteUser(dataGrid1);
        }

        private void btn_modify_Click(object sender, RoutedEventArgs e)
        {
            modification modification = new modification(this.dataGrid1);
            modification.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text == "")
            {
                dataGrid1.Items.Clear();
                baseModel.SelectBase(dataGrid1);
            }
            else
            {
                baseModel.searchData(dataGrid1, txtSearch.Text);
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
