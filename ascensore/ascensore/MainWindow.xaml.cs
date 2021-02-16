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
    /// 
    /// 
    /// Realizzare un programma concorrente che consenta di gestire il 
    /// movimento di un ascensore in un palazzo di 5 piani. In particolare consentire 
    /// di gestire 6 prenotazioni consecutive riferite ai bottoni dei 5 piani e quello 
    /// all'interno dell'ascensore. In una prima versione ipotizzare che l'ascensore possa 
    /// contenere una sola persona e quando si ferma in un piano occorre attendere che la persona 
    /// entri e scelga il piano a cui andare, a quel punto occorrerà spostare l'ascensore nel piano 
    /// e una volta fatta scendere la persona riprendere a servire i piani in ordine di prenotazione.
    /// 
    /// In una seconda versione opzionale prevedere che l'ascensore contenga fino a 3 persone e che lo 
    /// spostamento dell'ascensore possa avere un criterio (ad esempio farlo muovere verso l'alto o verso il
    /// basso e servire i piani che incontra nello spostamento). Realizzare un'interfaccia grafica che mostri 
    /// lo spostamento dell'ascensore e contenga sia i pulsanti di piano che la pulsantiera interna all'ascensore
    ///    
    ///    IDEE: 
    ///    mettere all'interno del semaforo lo spostamento dell'immagine a seconda di cosa si clicca nella tastiera dei bottoni.
    ///    in altezza si deve muovere:
    ///    36,125,429,-134 se piano 0
    ///    36,13,431,-22 se piano 1
    ///    36,-106,429,97   se piano 2
    ///    ho inserito solo 3 piani perchè non trovavo immagini con 5 piani per cui dato che la logica non cambia;semplicemente ci sarebbero due metodi in più uguali a quello del piano 1; quindi ho deciso di lasciare cosi
    ///    il mio codice prevede che la teastiera all'interno dell'ascensore e fuori poichè aspetta che si riclicchi per spostarsi.Quindi la chiamata al piano coincide con il premere il tasto con il numero del piano in cui ci si trova
    ///       



    public partial class MainWindow : Window
    {
        private Queue<int> ordinePrenotazione;
        public Semaphore semaforo;
        public int posizionePartenza;
        public int daDove;

        public MainWindow()
        {
            InitializeComponent();
            posizionePartenza = -134;
            ordinePrenotazione = new Queue<int>();
            semaforo = new Semaphore(0, 1);
            semaforo.Release();            
            daDove = 36;
        }

        public void muoviAscensore0()
        {
            while (posizionePartenza > -134)//può essere solo più in alto per cui non serve un if
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                posizionePartenza -= 25;
                daDove += 25;
                //36,125,429,-134
                this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 439, posizionePartenza); }));
            }
            posizionePartenza = -134;
            daDove = 125;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }

        private void muoviAscensore1()
        {
            if (posizionePartenza < -22)
            {
                while (posizionePartenza < -22)//||se è più in basso
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
                while (posizionePartenza > -22) //se è più in alto
                {
                   Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;
                    //36,13,431,-22
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 431, posizionePartenza); }));
                }
            }
            posizionePartenza = -22;
            daDove = 13;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }

        private void muoviAscensore2()
        {           
            while (posizionePartenza < 97)//finchè non arriva al piano
            {
                if (posizionePartenza < 97)//se è più in alto
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza += 25;
                    daDove -= 25;
                }
                /* else if (posizionePartenza > 97)//servirebbe se aggiungessimo un piano più in alto
                 {
                     Thread.Sleep(TimeSpan.FromMilliseconds(500));
                     posizionePartenza -= 25;
                     daDove += 25;
               } */
                //36,-106,429,97      
                this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, daDove, 429, posizionePartenza); }));
            }         
            posizionePartenza = 97;
            daDove = -106;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }

        public void piano0_Click(object sender, RoutedEventArgs e)
        {           
            ordinePrenotazione.Enqueue(1);
            semaforo.WaitOne(); //creo il semafotro rosso
            Thread t1 = new Thread(new ThreadStart(Muovi1));
            t1.Start();
            ordinePrenotazione.Dequeue();
            t1.Join();
            semaforo.Release();//lascio il semaforo;quindi diventa verde
            this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(192, 239, 0, -7); }));
        }

        private void Muovi1()
        {
            Thread t1 = new Thread(new ThreadStart(muoviAscensore0));
            t1.Start();
        }

        private void piano1_Click(object sender, RoutedEventArgs e)
        {
            ordinePrenotazione.Enqueue(2);
            semaforo.WaitOne();//creo il semafotro rosso
            Thread t2 = new Thread(new ThreadStart(Muovi2));
            t2.Start();
            ordinePrenotazione.Dequeue();
            t2.Join();
            semaforo.Release();//lascio il semaforo;quindi diventa verde
            this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(184, 126, 0, 127); }));
        }

        private void Muovi2()
        {         
            Thread t2 = new Thread(new ThreadStart(muoviAscensore1));
            t2.Start();
        }

        private void piano2_Click(object sender, RoutedEventArgs e)
        {
            ordinePrenotazione.Enqueue(3);
            semaforo.WaitOne();//creo il semafotro rosso
            Thread t3 = new Thread(new ThreadStart(Muovi3));
            t3.Start();
            ordinePrenotazione.Dequeue();
            t3.Join();
            semaforo.Release();//lascio il semaforo;quindi diventa verde
            this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(178,10,0,243); }));
        }

        private void Muovi3()
        {
           Thread t3 = new Thread(new ThreadStart(muoviAscensore2));
            t3.Start();
        }
    }
}
