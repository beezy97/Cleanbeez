using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Cleanbeez
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\Temp");
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            SaveDate();
            GetDate();
        }

        // Gestion des clicks
        private void Button_WEB_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/alexandre-robert-5920a2246/"){
                    UseShellExecute = true
                });
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Erreur " + ex.Message);
            }
        }

        private void Button_NETTOYER_Click_2(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Nettoyage en cour...");
            btnClean.Content = "NETTOYAGE EN COUR";

            Clipboard.Clear();

            try
            {
                ClearTempData(winTemp);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur :" + ex.Message);
            }


            try
            {
                ClearTempData(appTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur :" + ex.Message);
            }

            btnClean.Content = "NETTOYAGE TERMINE";
            espace.Content = "0 Mb";
        }

        private void Button_ANALYSER_Click(object sender, RoutedEventArgs e)
        {
            AnalyseFolder();
        }

        //Analyse des temp

        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di=>DirSize(di));
        }

        public void AnalyseFolder()
        {
            Console.WriteLine("Début de l'analyse...");
            long totalSize = 0;

            try
            {
                totalSize += DirSize(winTemp) / 1000000;
                totalSize += DirSize(appTemp) / 1000000;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Impossible de réaliser une analyse : " + ex.Message);
            }

            espace.Content = totalSize + "Mb";
            titre.Content = "Analyse effectuée";
            date.Content = DateTime.Today;
        }

        //Suppression des temp
        public void ClearTempData (DirectoryInfo di)
        {
            foreach(FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete();
                    Console.WriteLine(dir.FullName);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        //Save date

        public void SaveDate()
        {
            string date = DateTime.Today.ToString();
            File.WriteAllText("date.txt", date);
        }

        public void GetDate()
        {
            string dateFichier = File.ReadAllText("date.txt");
            if(dateFichier != String.Empty)
            {
                date.Content = dateFichier;
            }
        }
    }
}
