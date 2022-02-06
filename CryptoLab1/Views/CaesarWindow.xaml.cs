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
    /// Логика взаимодействия для CaesarWindow.xaml
    /// </summary>
    public partial class CaesarWindow : Window
    {
        private int key;
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

        public CaesarWindow()
        {
            InitializeComponent();

            keyInput.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, HandlePasteKey));

            this.key = 1;
            this.phrase = "";
            this.resultedPhrase = "";
            this.alphabet = Alphabet.Russian;
            this.modulus = 33;
        }


        private void ToggleEncryptButton()
        {
            if (keyInput.Text.Length > 0 && phraseInput.Text.Length > 0)
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

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            ToggleEncryptButton();
        }

        private void HandlePasteKey(object sender, ExecutedRoutedEventArgs e)
        {
            if (!e.Handled) ToggleEncryptButton();
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
                key = Int32.Parse(keyInput.Text);
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

            return (char)((((c + key) - indentChar) % modulus) + indentChar);
        }

        //decrypt
        private void DecryptPhrase(object sender, RoutedEventArgs e)
        {
            try
            {
                key = Int32.Parse(keyInput.Text);
                phrase = phraseInput.Text;

                resultedPhrase = decrypt(phrase, key);

                resultPhrase.Text = resultedPhrase;
                resultGrid.Visibility = Visibility.Visible;
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private string decrypt(string phrase, int key)
        {
            return getDecryptedString(phrase, key);
        }
        
        private string getDecryptedString(string phrase, int key)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in phrase)
            {
                stringBuilder.Append(decryptCharacter(c, key));
            }

            return stringBuilder.ToString();
        }

        private char decryptCharacter(char c, int key)
        {
            char indentChar;
            if (alphabet == Alphabet.Russian) indentChar = char.IsUpper(c) ? 'А' : 'а';
            else indentChar = char.IsUpper(c) ? 'A' : 'a';

            return (char)((((c + modulus - key) - indentChar) % modulus) + indentChar);
        }
    }
}
