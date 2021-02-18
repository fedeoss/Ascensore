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
    /// (In una seconda versione opzionale prevedere che l'ascensore contenga fino a 3 persone e che lo 
    /// spostamento dell'ascensore possa avere un criterio (ad esempio farlo muovere verso l'alto o verso il
    /// basso e servire i piani che incontra nello spostamento). Realizzare un'interfaccia grafica che mostri 
    /// lo spostamento dell'ascensore e contenga sia i pulsanti di piano che la pulsantiera interna all'ascensore)
    ///    
    ///    IDEE: 
    ///    mettere all'interno del semaforo lo spostamento dell'immagine a seconda di cosa si clicca nella tastiera dei bottoni.
    ///    in altezza si deve muovere:
    ///    -31,-90,489,417 se piano 2
    ///    -31,48,489,279 se piano 1
    ///    -31,183,489,144 se piano 0
    ///    -31,318,489,9 se piano -1
    ///    -31,456,489,-129   se piano -2
    ///    
    ///    il mio codice prevede che la teastiera all'interno dell'ascensore e fuori poichè aspetta che si riclicchi per spostarsi.Quindi la chiamata al piano coincide con il premere il tasto con il numero del piano in cui ci si trova
    ///    il lock funziona ma in certi casi non si nota che aspetta;oppure va semplicemente ricliccato il bootone,.

    public partial class MainWindow : Window
    {
        private Queue<int> ordinePrenotazione;
      //public Semaphore semaforo;
        public int posizionePartenza;     
        private static object x = new object();
        private static int daDove;

        public MainWindow()
        {
            InitializeComponent();
            posizionePartenza = 141;
            daDove = 186;
            ordinePrenotazione = new Queue<int>();
            // semaforo = new Semaphore(0, 1);
            // semaforo.Release();                  
        }
        private void piano2_Click(object sender, RoutedEventArgs e)
        {
            lock (x)
            {
                ordinePrenotazione.Enqueue(2);
                // semaforo.WaitOne();//creo il semafotro rosso
                Thread t3 = new Thread(new ThreadStart(Muovi3));
                t3.Start();
                ordinePrenotazione.Dequeue();
                t3.Join();
                //   semaforo.Release();//lascio il semaforo;quindi diventa verde
                this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(191, 56, 0, 580); }));
            }
        }

        private void Muovi3()
        {
            Thread t3 = new Thread(new ThreadStart(muoviAscensore2));
            t3.Start();
        }
        private void muoviAscensore2()
        {
            while (posizionePartenza < 417)//finchè non arriva al piano
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                posizionePartenza += 25;
                daDove -= 25;
                //-31,-90,489,417      
                this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
            }
            posizionePartenza = 417;
            daDove = -90;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }
        
        private void piano1_Click(object sender, RoutedEventArgs e)
        {
            lock (x)
            {
                ordinePrenotazione.Enqueue(1);
                // semaforo.WaitOne();//creo il semafotro rosso
                Thread t2 = new Thread(new ThreadStart(Muovi2));
                t2.Start();
                ordinePrenotazione.Dequeue();
                t2.Join();
                // semaforo.Release();//lascio il semaforo;quindi diventa verde
                this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(191, 186, 0, 450); }));
            }          
        }

        private void Muovi2()
        {         
            Thread t2 = new Thread(new ThreadStart(muoviAscensore1));
            t2.Start();
        }
        private void muoviAscensore1()
        {
            if (posizionePartenza < 279)
            {
                while (posizionePartenza < 279)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza += 25;
                    daDove -= 25;
                    // -31,48,489,279
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }
            if (posizionePartenza > 279)
            {
                while (posizionePartenza > 279)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;
                    //-31,48,489,279
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }                          
            posizionePartenza = 279;
            daDove = 48; 
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }

        public void piano0_Click(object sender, RoutedEventArgs e)
        {
            lock (x)
            {
                ordinePrenotazione.Enqueue(0);
                // semaforo.WaitOne(); //creo il semafotro rosso
                Thread t1 = new Thread(new ThreadStart(Muovi1));
                t1.Start();
                ordinePrenotazione.Dequeue();
                t1.Join();
                // semaforo.Release();//lascio il semaforo;quindi diventa verde
                this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(191, 324, 0, 312); }));
            }
        }

        private void Muovi1()
        {
            Thread t1 = new Thread(new ThreadStart(muoviAscensore0));
            t1.Start();
        }
        public void muoviAscensore0()
        {
            if (posizionePartenza < 144)
            {
                while (posizionePartenza < 144)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza += 25;
                    daDove -= 25;
                    // -31,183,489,144
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }else if (posizionePartenza > 144)
            {
                while (posizionePartenza > 144)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;
                    // -31,183,489,144
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }                        
            posizionePartenza = 144;
            daDove=183 ;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }

        private void piano_1_Click(object sender, RoutedEventArgs e)
        {
            lock (x)
            {
                ordinePrenotazione.Enqueue(-1);
                // semaforo.WaitOne();//creo il semafotro rosso
                Thread t4 = new Thread(new ThreadStart(Muovi4));
                t4.Start();
                ordinePrenotazione.Dequeue();
                t4.Join();
                //   semaforo.Release();//lascio il semaforo;quindi diventa verde
                this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(191, 458, 0, 178); }));
            }
        }
        private void Muovi4()
        {
            Thread t4 = new Thread(new ThreadStart(muoviAscensore3));
            t4.Start();
        }
        private void muoviAscensore3()
        {
            if (posizionePartenza < 9)//finchè non arriva al piano
            {
                while (posizionePartenza < 9)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza += 25;
                    daDove -= 25;
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }
            else if (posizionePartenza > 9)
            {
                while (posizionePartenza > 9)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }
            //-31,318,489,9
            posizionePartenza = 9;
            daDove = 318;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }

        private void piano_2_Click(object sender, RoutedEventArgs e)
        {
            lock (x)
            {
                ordinePrenotazione.Enqueue(-2);
                // semaforo.WaitOne();//creo il semafotro rosso
                Thread t5 = new Thread(new ThreadStart(Muovi5));
                t5.Start();
                ordinePrenotazione.Dequeue();
                t5.Join();
                //   semaforo.Release();//lascio il semaforo;quindi diventa verde
                this.Dispatcher.BeginInvoke(new Action(() => { barbie.Margin = new Thickness(191, 607, 0, 29); }));
            }
        }
        private void Muovi5()
        {
            Thread t5 = new Thread(new ThreadStart(muoviAscensore4));
            t5.Start();
        }
        private void muoviAscensore4()
        {
            if (posizionePartenza > -129)//finchè non arriva al piano
            {
                while (posizionePartenza > -129)//se è più in alto
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    posizionePartenza -= 25;
                    daDove += 25;
                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(-31, daDove, 489, posizionePartenza); }));
                }
            }
           /* else if (posizionePartenza > 456)//servirebbe se aggiungessimo un piano più in basso
                 {
                     Thread.Sleep(TimeSpan.FromMilliseconds(500));
                     posizionePartenza -= 25;                    
                 } 
          */
                //-31,456,489,-129               
            posizionePartenza = -129;
            daDove = 456;
            //fuori dal while perchè così l'ascensore si ferma esattamente nel punto in cui deve e non leggermente piu in alto.
            //dato che senza queste due variabili lo faceva;aggiungendo 25 non arrivava mai al numero richiesta perchè non era multiplo.
        }
    }
}
