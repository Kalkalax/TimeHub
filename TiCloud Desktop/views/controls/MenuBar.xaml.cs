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
using TiCloud;
using TiCloud_Desktop.viewmodels;


namespace TiCloud_Desktop.views.controls
{
    /// <summary>
    /// Logika interakcji dla klasy MenuBar.xaml
    /// </summary>
    public partial class MenuBar : System.Windows.Controls.UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
           // DataContext = new MainWindowViewModel();
        }
    }
}
