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

using CryptoLab1.Views;
using CryptoLab1.Models;

namespace CryptoLab1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HandleClick(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            switch(clicked.Name)
            {
                case Methods.OpenRailFenceButton:
                    {
                        initWindow(new RailFenceWindow(), Methods.RailFenceMethod);
                        break;
                    }
                case Methods.OpenKeyPhraseButton:
                    {
                        initWindow(new KeyPhraseWindow(), Methods.KeyPhraseMethod);
                        break;
                    }
                case Methods.OpenTurnableGrilleButton:
                    {
                        initWindow(new TurnableGrilleWindow(), Methods.TurnableGrilleMethod);
                        break;
                    }
                case Methods.OpenCaesarButton:
                    {
                        initWindow(new CaesarWindow(), Methods.CaesarMethod);
                        break;
                    }
                case Methods.OpenSubstitutionMultiplyButton:
                    {
                        initWindow(new SubstitutionMultiplyWindow(), Methods.SubstitutionMultiplyMethod);
                        break;
                    }
            }
        }

        private void initWindow(Window window, String methodName)
        {
            window.Title = methodName;
            window.Show();
        }
    }
}
