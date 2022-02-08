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
    /// Логика взаимодействия для SubstitutionMultiplyWindow.xaml
    /// </summary>
    public partial class SubstitutionMultiplyWindow : Window
    {
        private int encryptKey;
        private int decryptKey;
        private string phrase;
        private string resultedPhrase;
        private Alphabet alphabet;
        private int modulus;

        private enum Option
        {
            Encrypt,
            Decrypt
        }

        private enum Alphabet
        {
            Russian,
            English
        }

        public SubstitutionMultiplyWindow()
        {
            InitializeComponent();

            encryptKeyInput.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, HandlePasteKey));
            decryptKeyInput.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, HandlePasteKey));

            this.encryptKey = 1;
            this.decryptKey = 1;
            this.phrase = "";
            this.resultedPhrase = "";
            this.alphabet = Alphabet.Russian;
            this.modulus = 33;
        }


        private void ToggleEncryptButton()
        {
            ToggleHandlersSeparate(encryptButton, encryptKeyInput);
        }

        private void ToggleDecryptButton()
        {
            ToggleHandlersSeparate(decryptButton, decryptKeyInput);
        }

        private bool AreKeysValid()
        {
            try
            {
                int key1 = Int32.Parse(encryptKeyInput.Text);
                int key2 = Int32.Parse(decryptKeyInput.Text);

                if (encryptKeyInput.Text.Length > 0 && decryptKeyInput.Text.Length > 0 && !checkKeys(key1, key2))
                {
                    encryptButton.IsEnabled = false;
                    decryptButton.IsEnabled = false;
                    return false;
                }

                return true;
            } catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            bool checkKeys(int key1, int key2)
            {
                return (key1 * key2) % modulus == 1;
            }
        }

        private void ToggleHandlersSeparate(Button btn, TextBox keyInput)
        {
            if(keyInput.Text.Equals("0")) {
                btn.IsEnabled = false;
                return;
            }

            if (keyInput.Text.Length > 0 && phraseInput.Text.Length > 0)
            {
                btn.IsEnabled = true;
            }
            else
            {
                btn.IsEnabled = false;
                return;
            }

            if (!AreRelativelyPrime(Int32.Parse(keyInput.Text), modulus))
            {
                btn.IsEnabled = false;
                return;
            }

            if (!AreKeysValid())
            {
                return;
            }

            bool AreRelativelyPrime(int a, int b)
            {
                return EuclidGcd(a, b) == 1;
            }

            int EuclidGcd(int a, int b)
            {
                int t;
                while (b != 0)
                {
                    t = a;
                    a = b;
                    b = t % b;
                }
                return a;
            }
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            ToggleEncryptButton();
            ToggleDecryptButton();

            AreKeysValid();
        }

        private void HandlePasteKey(object sender, ExecutedRoutedEventArgs e)
        {
            if (!e.Handled)
            {
                ToggleEncryptButton();
                ToggleDecryptButton();

                AreKeysValid();
            }
        }

        private void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^([0-9]|[1-9][0-9])$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void alphabetSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selIndex = ((ComboBox)sender).SelectedIndex;
            alphabet = selIndex == 0 ? Alphabet.Russian : Alphabet.English;
            modulus = selIndex == 0 ? 33 : 26;
        }

        private void HandlePreviewPhraseInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex;

            if (alphabet == Alphabet.Russian) regex = new Regex(@"^[а-яА-Я]$");
            else regex = new Regex(@"^[a-zA-Z]$");

            e.Handled = !regex.IsMatch(e.Text);
        }


        //encrypt
        private void EncryptPhrase(object sender, RoutedEventArgs e)
        {
            try
            {
                encryptKey = Int32.Parse(encryptKeyInput.Text);
                phrase = phraseInput.Text;

                resultedPhrase = encrypt(phrase, encryptKey);

                resultPhrase.Text = resultedPhrase;
                resultGrid.Visibility = Visibility.Visible;
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private string encrypt(string phrase, int key)
        {
            return getEncryptedString(phrase, key);
        }

        private string getEncryptedString(string phrase, int key)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in phrase)
            {
                stringBuilder.Append(encryptCharacter(c, key));
            }

            return stringBuilder.ToString();
        }

        private char encryptCharacter(char c, int key)
        {
            char indentChar;
            if (alphabet == Alphabet.Russian) indentChar = char.IsUpper(c) ? 'А' : 'а';
            else indentChar = char.IsUpper(c) ? 'A' : 'a';

            return (char)(((c * key - indentChar) % modulus) + indentChar);
        }

        //decrypt
        private void DecryptPhrase(object sender, RoutedEventArgs e)
        {
            try
            {
                decryptKey = Int32.Parse(decryptKeyInput.Text);
                phrase = phraseInput.Text;

                resultedPhrase = encrypt(phrase, decryptKey);

                resultPhrase.Text = resultedPhrase;
                resultGrid.Visibility = Visibility.Visible;
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}