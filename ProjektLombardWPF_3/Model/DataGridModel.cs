using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjektLombardWPF_3
{
    public class DataGridModel
    {
       
        public void WrtieDataGrid(DataGrid dataGrid1, UserModel userModel)
        {
            dataGrid1.Items.Add(userModel);
        }
    }
}
