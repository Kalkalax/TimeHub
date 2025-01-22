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

namespace TiCloud_Desktop.views.controls
{
    /// <summary>
    /// Logika interakcji dla klasy TimerBar.xaml
    /// </summary>
    public partial class TimerBar : System.Windows.Controls.UserControl
    {
        public TimerBar()
        {
            InitializeComponent();


            StartStop_Timmer_Button.IsEnabled = false; //wyłączenie przycisku przed wybraniem projektu
            Reload_Timmer_Button.IsEnabled = false;

        }

        private void StartStop_Timmer_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Reload_Timmer_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
