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
        private int key;
        private String phrase;
        private String encryptedPhrase;

        private enum Direction
        {
            Rise,
            Decrease
        }

        public RailFenceWindow()
        {
            InitializeComponent();

            this.key = 1;
            this.phrase = "";
            this.encryptedPhrase = "";
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

                (encryptedPhrase, char[,] arr) = encrypt(phrase, key);

                resultPhrase.Text = encryptedPhrase;
                resultGrid.Visibility = Visibility.Visible;

                viewMatrix(arr);
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void viewMatrix(char[,] arr)
        {
            List<List<char>> chars = new List<List<char>>();

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                chars.Add(new List<char>());

                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    chars[i].Add(arr[i, j]);
                }
            }

            matrixView.ItemsSource = chars;
            matrixGrid.Visibility = Visibility.Visible;
        }

        private (string, char[,]) encrypt(string phrase, int key)
        {
            char[,] arr = fillEncMatrix(phrase, key);

            return (getEncryptedString(arr), arr);
        }

        private char[,] fillEncMatrix(string phrase, int key)
        {
            int phraseIndex = 0;
            int colIndex = 0;
            Direction direction = Direction.Rise;

            char[,] arr = fillMatrix(new char[key, phrase.Length]);

            int phraseLength = phrase.Length;
            while (phraseIndex < phraseLength)
            {
                arr[colIndex, phraseIndex] = phrase[phraseIndex++];

                colIndex = iterateColIndex(direction, colIndex);
                if (colIndex == 0 || colIndex == key - 1) direction = changeDirection();
            }

            char[,] fillMatrix(char[,] arr)
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

            Direction changeDirection()
            {
                return (direction == Direction.Rise) ? Direction.Decrease : Direction.Rise;
            }

            int iterateColIndex(Direction direction, int colIndex)
            {
                return (direction == Direction.Rise) ? ++colIndex : --colIndex;
            }

            return arr;
        }

        private string getEncryptedString(char[,] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in arr)
            {
                if(c != '\0') stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }
    }
}
