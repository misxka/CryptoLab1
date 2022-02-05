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
        private String resultedPhrase;

        private enum Direction
        {
            Rise,
            Decrease
        }

        private enum Option
        {
            Encrypt,
            Decrypt
        }

        public RailFenceWindow()
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

                viewMatrix(arr);
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
            char[,] arr = fillMatrix(phrase, key, Option.Encrypt);

            return (getEncryptedString(arr), arr);
        }

        private (string, char[,]) decrypt(string phrase, int key)
        {
            char[,] arr = fillMatrix(phrase, key, Option.Decrypt);

            return (getDecryptedString(arr), arr);
        }

        private char[,] fillMatrix(string phrase, int key, Option option)
        {
            int phraseIndex = 0;
            Direction direction = Direction.Rise;

            char[,] arr = fillMatrixWithNull(new char[key, phrase.Length]);

            if (option == Option.Encrypt) fillEncMatrix(phrase, key);
            else fillDecryptMatrix(phrase, key);


            void fillEncMatrix(string phrase, int key)
            {
                int rowIndex = 0;
                int phraseLength = phrase.Length;

                while (phraseIndex < phraseLength)
                {
                    arr[rowIndex, phraseIndex] = phrase[phraseIndex++];

                    rowIndex = iterateRowIndex(direction, rowIndex);
                    if (rowIndex == 0 || rowIndex == key - 1) direction = changeDirection(ref direction);
                }
            }

            void fillDecryptMatrix(string phrase, int key)
            {
                int phraseLength = phrase.Length;
                for(int row = 0; row < key; row++)
                {
                    int col = row;
                    bool reverseFlag = false;
                    while(col < phraseLength)
                    {
                        arr[row, col] = phrase[phraseIndex++];

                        if (row == 0) col += (key - row - 1) * 2;
                        else if (row == key - 1) col += (key - (key - row)) * 2;
                        else if (!reverseFlag) col += (key - row - 1) * 2;
                        else col += (key - (key - row)) * 2;

                        reverseFlag = !reverseFlag;
                    }
                }
            }

            return arr;
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

        private Direction changeDirection(ref Direction direction)
        {
            return (direction == Direction.Rise) ? Direction.Decrease : Direction.Rise;
        }

        private int iterateRowIndex(Direction direction, int rowIndex)
        {
            return (direction == Direction.Rise) ? ++rowIndex : --rowIndex;
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

        private string getDecryptedString(char[,] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();

            Direction direction = Direction.Rise;
            int rowIndex = 0;

            for(int colIndex = 0; colIndex < arr.GetLength(1); colIndex++) {
                if (arr[rowIndex, colIndex] != '\0')
                {
                    stringBuilder.Append(arr[rowIndex, colIndex]);
                    rowIndex = iterateRowIndex(direction, rowIndex);
                }

                if (rowIndex == 0 || rowIndex == key - 1) direction = changeDirection(ref direction);
            }

            return stringBuilder.ToString();
        }
    }
}
