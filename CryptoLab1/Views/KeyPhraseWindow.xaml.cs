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
        private string key;
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

            this.key = "";
            this.phrase = "";
            this.resultedPhrase = "";
        }

        private void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[а-яА-Я]$");
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
                key = keyInput.Text.ToLower();
                phrase = phraseInput.Text;

                resultedPhrase = encrypt(phrase, key);

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
            /*try
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
            }*/
        }


        private string encrypt(string phrase, string key)
        {
            char[] sortedChars = key.ToCharArray();
            Array.Sort(sortedChars);

            int keyLength = key.Length;

            int[] numbers = createNumbersArray(sortedChars, keyLength);

            char[,] matrix = createPhraseMatrix(phrase, keyLength);

            return buildEncryptedString(matrix, numbers);


            //inner methods
            int[] createNumbersArray(char[] sortedChars, int keyLength)
            {
                int[] numbers = new int[keyLength];

                int lastFoundIndex = -1;
                for (int i = 0; i < keyLength; i++)
                {
                    char currentChar = sortedChars[i];

                    if (i > 0 && sortedChars[i - 1] != currentChar) lastFoundIndex = -1;

                    lastFoundIndex = key.IndexOf(currentChar, lastFoundIndex + 1);
                    numbers[lastFoundIndex] = i;
                }

                return numbers;
            }

            char[,] createPhraseMatrix(string phrase, int keyLength)
            {
                int phraseLength = phrase.Length;
                int rows = (phraseLength / keyLength) + 1;

                char[,] matrix = new char[rows, keyLength];
                int phraseIndex = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (phraseIndex >= phraseLength) matrix[i, j] = '\0';
                        else matrix[i, j] = phrase[phraseIndex++];
                    }
                }

                return matrix;
            }
            
            string buildEncryptedString(char[,] matrix, int[] numbers)
            {
                StringBuilder stringBuilder = new StringBuilder();

                var dict = new Dictionary<int, int>();
                int numbersLength = numbers.Length;
                for (int i = 0; i < numbersLength; i++)
                {
                    dict.Add(numbers[i], i);
                }

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < numbersLength; j++)
                    {
                        if (matrix[i, dict[j]] == '\0') break;
                        stringBuilder.Append(matrix[i, dict[j]]);
                    }
                }

                return stringBuilder.ToString();
            }
        }
    }
}
