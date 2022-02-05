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
    /// Логика взаимодействия для KeyPhraseWindow.xaml
    /// </summary>
    public partial class KeyPhraseWindow : Window
    {
        private int key;
        private string phrase;
        private string resultedPhrase;

        private enum Option
        {
            Encrypt,
            Decrypt
        }

        public KeyPhraseWindow()
        {
            InitializeComponent();

            this.key = 1;
            this.phrase = "";
            this.resultedPhrase = "";
        }

        private void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[1-9]$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void EncryptPhrase(object sender, RoutedEventArgs e)
        {
            try
            {
                key = Int32.Parse(keyInput.Text);
                phrase = phraseInput.Text;

                (resultedPhrase, char[,] arr) = encrypt(phrase, key);

                resultPhrase.Text = resultedPhrase;
                resultGrid.Visibility = Visibility.Visible;
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void DecryptPhrase(object sender, RoutedEventArgs e)
        {
            try
            {
                key = Int32.Parse(keyInput.Text);
                phrase = phraseInput.Text;

                (resultedPhrase, char[,] arr) = decrypt(phrase, key);

                resultPhrase.Text = resultedPhrase;
                resultGrid.Visibility = Visibility.Visible;
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private (string, char[,]) encrypt(string phrase, int key)
        {
            char[,] arr = fillMatrix(phrase, key, Option.Encrypt);

            return (getEncryptedString(arr), arr);
        }

        private (string, char[,]) decrypt(string phrase, int key)
        {
            char[,] arr = fillMatrix(phrase, key, Option.Decrypt);

            return (getDecryptedString(arr), arr);
        }

        private char[,] fillMatrixWithNull(char[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = '\0';
                }
            }
            return arr;
        }

        private char[,] fillMatrix(string phrase, int key, Option option)
        {
            char[,] arr = fillMatrixWithNull(new char[key, phrase.Length]);

            if (option == Option.Encrypt) fillEncMatrix(phrase, key);
            else fillDecryptMatrix(phrase, key);


            void fillEncMatrix(string phrase, int key)
            {
                
            }

            void fillDecryptMatrix(string phrase, int key)
            {

            }

            return arr;
        }

        private string getEncryptedString(char[,] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in arr)
            {
                if (c != '\0') stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        private string getDecryptedString(char[,] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();

            return stringBuilder.ToString();
        }
    }
}
