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

namespace TiCloud
{
    //TODO: napisać summary i dopisać funkcje na anuluj i informacje gdy jest pusty i sprawdzanie czy nazwa jest wykorzystana ?
    /// <summary>
    /// Logika interakcji dla klasy NameInputWindow.xaml
    /// </summary>
    public partial class NameInputWindow : Window
    {

        public string EnteredName { get; private set; }

        public NameInputWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            EnteredName = NameTextBox.Text;  // Pobranie tekstu z TextBox
            this.DialogResult = true;  // Zamknięcie okna z wynikiem "true"
        }
    }
}
