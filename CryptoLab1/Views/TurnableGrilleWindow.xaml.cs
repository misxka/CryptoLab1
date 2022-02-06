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
    /// Логика взаимодействия для TurnableArrayWindow.xaml
    /// </summary>
    public partial class TurnableGrilleWindow : Window
    {
        private string key;
        private string phrase;
        private string resultedPhrase;

        private enum Option
        {
            Encrypt,
            Decrypt
        }

        public TurnableGrilleWindow()
        {
            InitializeComponent();

            keyInput.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, HandlePasteKey));

            this.key = "";
            this.phrase = "";
            this.resultedPhrase = "";
        }

        private void ToggleEncryptButton()
        {
            if (keyInput.Text.Length == phraseInput.Text.Length && keyInput.Text.Length > 0)
            {
                encryptButton.IsEnabled = true;
                decryptButton.IsEnabled = true;
            }
            else
            {
                encryptButton.IsEnabled = false;
                decryptButton.IsEnabled = false;
            }
        }

        private void HandlePasteKey(object sender, ExecutedRoutedEventArgs e)
        {
            if (!e.Handled) ToggleEncryptButton();
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            ToggleEncryptButton();
        }

        private void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[Xx.]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void HandleErrorOutput(Visibility visibility, string message = "Неверный ввод")
        {
            errorOutput.Text = message;
            errorOutput.Visibility = visibility;
        }

        private void EncryptPhrase(object sender, RoutedEventArgs e)
        {
            key = keyInput.Text.ToLower();

            IsCheckSuccessful(key);

            if (IsCorrectSlotsAmount(key))
            {
                phrase = phraseInput.Text;

                resultedPhrase = Encrypt(phrase, key);

                resultPhrase.Text = resultedPhrase;
                resultGrid.Visibility = Visibility.Visible;
            } else HandleErrorOutput(Visibility.Visible, "Неверное количество отверстий");
        }

        private void DecryptPhrase(object sender, RoutedEventArgs e)
        {
            key = keyInput.Text.ToLower();
            phrase = phraseInput.Text;

            //(resultedPhrase, char[,] arr) = decrypt(phrase, key);

            resultPhrase.Text = resultedPhrase;
            resultGrid.Visibility = Visibility.Visible;

        }

        private string Encrypt(string phrase, string key)
        {
            int size = (int)Math.Sqrt(key.Length);

            char[,] keyMatrix = createKeyMatrix(new char[size, size], size);

            char[,] phraseMatrix = fillMatrixWithNull(new char[size, size]);

            phraseMatrix = fillPhraseMatrix(phraseMatrix, size, keyMatrix);

            return getEncryptedString(phraseMatrix);
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

        private char[,] createKeyMatrix(char[,] keyMatrix, int size)
        {
            int keyIndex = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    keyMatrix[i, j] = key[keyIndex++];
                }
            }

            return keyMatrix;
        }

        private char[,] fillPhraseMatrix(char[,] phraseMatrix, int size, char[,] keyMatrix)
        {
            int phraseIndex = 0;
            for (int k = 0; k < size; k++)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (keyMatrix[i, j] == '.')
                        {
                            phraseMatrix[i, j] = phrase[phraseIndex++];
                        }
                    }
                }
                phraseMatrix = rotateMatrix(phraseMatrix);
            }

            return phraseMatrix;
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


        private bool IsCorrectSlotsAmount(string key)
        {
            int correctAmount = (int) Math.Ceiling(key.Length / 4.0);

            return key.Count(c => (c == '.')) == correctAmount;
        }

        private bool IsPerferctSquare(int number)
        {
            return (Math.Sqrt(number) % 1 == 0);
        }

        private bool IsCheckSuccessful(string key)
        {
            if (IsPerferctSquare(key.Length)) HandleErrorOutput(Visibility.Collapsed);
            else
            {
                HandleErrorOutput(Visibility.Visible, "Количество символов должно быть полным квадратом");
                return false;
            }

            if (IsCorrectSlotsAmount(key)) HandleErrorOutput(Visibility.Collapsed);
            else
            {
                HandleErrorOutput(Visibility.Visible, "Неверное количество отверстий");
                return false;
            }

            return true;
        }

        //private (string, char[,]) decrypt(string phrase, string key)
        //{

        //}

        private char[,] rotateMatrix(char[,] matrix)
        {
            int size = matrix.GetLength(0);
            for (int i = 0; i < size / 2; i++)
            {
                for (int j = i; j < size - i - 1; j++)
                {
                    char temp = matrix[i, j];
                    matrix[i, j] = matrix[size - 1 - j, i];
                    matrix[size - 1 - j, i] = matrix[size - 1 - i, size - 1 - j];
                    matrix[size - 1 - i, size - 1 - j] = matrix[j, size - 1 - i];
                    matrix[j, size - 1 - i] = temp;
                }
            }

            return matrix;
        }
    }
}
