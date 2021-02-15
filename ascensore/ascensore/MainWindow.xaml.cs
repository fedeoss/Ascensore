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
    /// ealizzare un programma concorrente che consenta di gestire il 
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
    ///    a 50 se piano 2
    ///    a 68 se piano 1
    ///    a 176 se piano 0



    public partial class MainWindow : Window
    {
        public MainWindow()
        { 
            Thread t1 = new Thread(new ThreadStart(F));
            t1.Start();
            InitializeComponent();
        }
        int posizionePartenza1;
        

    }

    private void piano2_Click(object sender, RoutedEventArgs e)
        {
        
        }

        private void piano1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void piano0_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
