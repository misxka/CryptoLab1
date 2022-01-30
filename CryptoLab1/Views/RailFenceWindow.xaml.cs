using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CryptoLab1.Views
{
    /// <summary>
    /// Логика взаимодействия для RailFenceWindow.xaml
    /// </summary>
    public partial class RailFenceWindow : Window
    {
        public RailFenceWindow()
        {
            InitializeComponent();
        }

        private void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }
    }
}
