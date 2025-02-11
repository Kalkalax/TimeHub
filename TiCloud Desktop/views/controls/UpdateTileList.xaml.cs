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
using Button = System.Windows.Controls.Button;
using TiCloud_Desktop.core.data;

namespace TiCloud_Desktop.views.controls
{
    /// <summary>
    /// Logika interakcji dla klasy UpdateTileList.xaml
    /// </summary>
    public partial class UpdateTileList : System.Windows.Controls.UserControl
    {
        public UpdateTileList()
        {
            InitializeComponent();
            AddButton();
        }


        private void AddButton()
        {
            UpdateTileListPanel.Children.Clear();

            int count = UpdateInfoManager.CountUpdateFiles(); // Pobranie liczby aktualizacji

            for (int i = 0; i <= count-1; i++) // Użycie 'count' jako wartości początkowej
            {
                UpdateTile b = new UpdateTile
                {
                    Margin = new Thickness(0, 0, 0, 10)
                };

                UpdateTileListPanel.Children.Add(b);
            }
        }
    }

}
