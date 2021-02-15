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
    ///   36,-102 se piano 2
    ///    36,19 se piano 1
    ///    36,125 se piano 0



    public partial class MainWindow : Window
    {


        public MainWindow()
        {


        }
        public int posizionePartenza = 0;
        private void muoviAscensore0()
        {
            if (posizionePartenza == -102)
            {
                while (posizionePartenza == 125)
                {
                    posizionePartenza = 125;

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));

                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, posizionePartenza, 0, 0); }));
                }

            }
            else if (posizionePartenza == 19)
            {
                while (posizionePartenza == 125)
                {
                    posizionePartenza = 125;

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));

                    this.Dispatcher.BeginInvoke(new Action(() => { ascensore1.Margin = new Thickness(36, posizionePartenza, 0, 0); }));
                }
            }

        }
        private void muoviAscensore1()
        {

        }
        private void muoviAscensore2()
        {

        }

        public void piano0_Click(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(muoviAscensore0));
            t1.Start();
            t1.Join();
        }

        private void piano1_Click(object sender, RoutedEventArgs e)
        {
            Thread t2 = new Thread(new ThreadStart(muoviAscensore1));
            t2.Start();
            t2.Join();
        }

        private void piano2_Click(object sender, RoutedEventArgs e)
        {
            Thread t3 = new Thread(new ThreadStart(muoviAscensore2));
            t3.Start();
            t3.Join();
        }

    }
}
