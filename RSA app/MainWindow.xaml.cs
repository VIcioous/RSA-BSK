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

namespace RSA_app
{

    public partial class MainWindow : Window
    {
        int y, n, p, q, en, d, m, c, flag;
        bool isPrime = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        bool checkPrime(int num)
        {
            for (int i = 2; i <= (num / 2); i++)
            {
                if (num % i == 0) 
                    return false;
            }
            return true;

        } //sprawdzenie czy liczby są pierwsze
        void calculateE()
        {
            for (int j = 2; j < y; j++)
            {
                if (y % j == 0)
                    continue;
                isPrime = checkPrime(j);
                if (isPrime && j != p && j != q)
                {
                    en = j;
                    flag = calculateD(en);
                    if (flag > 0)
                    {
                        d = flag;
                        break;
                    }
                }
            }
        } //liczenie liczby względnie pierwszej z tocjenta od n
        int calculateD(int e)
        {
            int k = 1;
            while (true)
            {
                k = k + y;
                if (k % e == 0)
                {
                    return k / e;
                }
            }
        } //d, gdzie jej różnica z odwrotnością modularną liczby e jest podzielna przez φ(n)
        private void CipherText(object sender, RoutedEventArgs e) //obsługa przycisku szyfrującego
        {
            string cipher="";
            string msg = MessageInput.Text;
            char[] oMSG = msg.ToCharArray(0, msg.Length); //przerobienie na tablice charów aby łatwiej przerobić na ASCII
            int.TryParse(pInput.Text, out p); // konwersja liczb
            int.TryParse(qInput.Text, out q);

            StringBuilder intMessage = new();

            isPrime = checkPrime(p); //sprawdzenie czy są liczbami pierwszymi
            isPrime = checkPrime(q);
            n = p * q; //liczenie n
            y = (p - 1) * (q - 1); //liczenie wartości funkcji eulera od n
            calculateE(); //wybór liczby e oraz d 

            pText.Text = p.ToString(); //wypisanie w aplikacji wyników 
            qText.Text = q.ToString();
            nText.Text = n.ToString();
            yText.Text = y.ToString();
            eText.Text = en.ToString();
            dText.Text = d.ToString();

            for (int a = 0; a < msg.Length; a++) //szyfrowanie wiadomości
            {
                m = oMSG[a]; //zamiana litery na ASCII
                long k = 1;
                for (int i = 0; i < en; i++) // wykonanie operacji  
                {
                    k = k * m;
                    k = k % n;
                }
                c = (int)k;
                cipher += (char)c; //bindowanie tekstu
                intMessage.Append(c);
                intMessage.Append(" ");
            }

            finalTextbox.Text = cipher; //wyświetlenie wiadomości
            finalNumberTextbox.Text = intMessage.ToString();
        }
        private void DecipherText(object sender, RoutedEventArgs e) //deszyfracja
        {
            string decipher="";
            string msg = MessageInput.Text;
            string[] tab = msg.Split(" "); //dzielenie na tablicę z separatorem spacji

            StringBuilder intMessage = new();

            int.TryParse(pInput.Text, out p);
            int.TryParse(qInput.Text, out q);

            isPrime = checkPrime(p);
            isPrime = checkPrime(q);
            n = p * q;
            y = (p - 1) * (q - 1);
            calculateE();

            pText.Text = p.ToString();
            qText.Text = q.ToString();
            nText.Text = n.ToString();
            yText.Text = y.ToString();
            eText.Text = en.ToString();
            dText.Text = d.ToString();

            for (long a = 0; a < tab.Length; a++)
            {
                int.TryParse(tab[a],out m); //przetworzenie na liczbę
                long k = 1;
                for (int i = 0; i < d; i++)
                {
                    k = k * m;
                    k = k % n;
                }
                c = (int)k;
                decipher += (char)c; //a tutaj szybkie przetworzenie na tekst
                intMessage.Append(c);
                intMessage.Append(" ");
            }

            finalTextbox.Text = decipher;
            finalNumberTextbox.Text = intMessage.ToString();
        }
    }
}
