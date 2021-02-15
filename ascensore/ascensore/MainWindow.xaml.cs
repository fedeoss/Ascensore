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
using System.Threading;
using System.Diagnostics;
using System.Data.Odbc;

namespace ascensore
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    /// Realizzare un programma concorrente che consenta di gestire il 
    /// movimento di un ascensore in un palazzo di 5 piani. In particolare consentire 
    /// di gestire 6 prenotazioni consecutive riferite ai bottoni dei 5 piani e quello 
    /// all'interno dell'ascensore. In una prima versione ipotizzare che l'ascensore possa 
    /// contenere una sola persona e quando si ferma in un piano occorre attendere che la persona 
    /// entri e scelga il piano a cui andare, a quel punto occorrerà spostare l'ascensore nel piano 
    /// e una volta fatta scendere la persona riprendere a servire i piani in ordine di prenotazione.
    /// In una seconda versione opzionale prevedere che l'ascensore contenga fino a 3 persone e che lo 
    /// spostamento dell'ascensore possa avere un criterio (ad esempio farlo muovere verso l'alto o verso il
    /// basso e servire i piani che incontra nello spostamento). Realizzare un'interfaccia grafica che mostri 
    /// lo spostamento dell'ascensore e contenga sia i pulsanti di piano che la pulsantiera interna all'ascensore
    ///    
    ///    IDEE: 
    ///    mettere all'interno del semaforo lo spostamento dell'immagine a seconda di cosa si clicca nella tastiera dei bottoni.
    ///    in altezza si deve muovere:
    ///    36,-102 se piano 2
    ///    36,19 se piano 1
    ///    36,125 se piano 0
    ///    ho inserito solo 3 piani perchè non trovavo immagini con 5 piani per cui dato che la logica non cambia;semplicemente ci sarebbero sue metodi in più ho deciso di lasciare cosi



    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            posizionePartenza = -134;
            ordinePrenotazione = new Queue<int>();
            semaforo = new Semaphore(0, 1);
            semaforo.Release();
            
            daDove = 36;

        }
        private Queue<int> ordinePrenotazione;
        public Semaphore semaforo;
        public int posizionePartenza;
        public int daDove;
        int nextStop = -1;
        bool call = false;
        private void muoviAscensore0()
        {


            while (posizionePartenza > -134)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                posizionePartenza -= 25;
                daDove += 25;
                //36,125,429,-134
                this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 439, posizionePartenza); }));
            }




        }
        private void muoviAscensore1()
        {
            if (posizionePartenza < -22)
            {
                while (posizionePartenza < -22)//|| posizionePartenza > - 22)
                {


                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza += 25;
                    daDove -= 25;


                    //36,13,431,-22
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 431, posizionePartenza); }));
                }
            }
            else
            {
                while (posizionePartenza > -22)
                {


                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;

                    //36,13,431,-22
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 431, posizionePartenza); }));
                }
            }
        }
        private void muoviAscensore2()
        {


            while (posizionePartenza < 97)
            {
                if (posizionePartenza < 97)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza += 25;
                    daDove -= 25;
                }
                else if (posizionePartenza > 97)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;
                }
                //36,-106,429,97
                this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 429, posizionePartenza); }));
            }
        }

        public void piano0_Click(object sender, RoutedEventArgs e)
        {
            
            ordinePrenotazione.Enqueue(1);
            semaforo.WaitOne();
            Thread t1 = new Thread(new ThreadStart(Muovi1));
            t1.Start();
            ordinePrenotazione.Dequeue();

            t1.Join();
            semaforo.Release();
        }

        private void Muovi1()
        {

            Thread t1 = new Thread(new ThreadStart(muoviAscensore0));
            t1.Start();

        }


        private void piano1_Click(object sender, RoutedEventArgs e)
        {
            ordinePrenotazione.Enqueue(2);
            semaforo.WaitOne();
            Thread t2 = new Thread(new ThreadStart(Muovi2));
            t2.Start();
            ordinePrenotazione.Dequeue();

            t2.Join();
            semaforo.Release();
        }
        private void Muovi2()
        {

            Thread t2 = new Thread(new ThreadStart(muoviAscensore1));
            t2.Start();

        }

        private void piano2_Click(object sender, RoutedEventArgs e)
        {
            ordinePrenotazione.Enqueue(3);
            semaforo.WaitOne();
            Thread t3 = new Thread(new ThreadStart(Muovi3));
            t3.Start();
            ordinePrenotazione.Dequeue();

            t3.Join();
            semaforo.Release();
        }
        private void Muovi3()
        {

            Thread t3 = new Thread(new ThreadStart(muoviAscensore2));
            t3.Start();

        }


    }
}
