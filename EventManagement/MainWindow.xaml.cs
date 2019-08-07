using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EventManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum EventTypes
    {
        Birthday = 0,
        Wedding  = 1,
        Graduation = 2
    }
    public partial class MainWindow : Window
    {
        ObservableCollection<Event> eventList = null;
        public ObservableCollection<Event> EventList { get => eventList; set => eventList = value; }
        Hashtable  datetable = GetDateTable();

        private static Hashtable GetDateTable()
        {
            Hashtable dateTable = new Hashtable();

            for(int i = 0; i < 15; i++)
            {
                string date = DateTime.Now.AddDays(i+1).ToString("MMMM dd, dddd");
                dateTable.Add(i, date);
            }

            return dateTable;
        }

        public MainWindow()
        {
            InitializeComponent();
            //populate event types
            PopulateTypes();
            //populate days 
            PopulateDays();
            EventList = new ObservableCollection<Event>();
            HideAdditionalFeatures();
        }

        public void PopulateDays()
        {
            foreach(DictionaryEntry pair in datetable)
            {
                daySelection.Items.Add(pair.Value);
            }
        }
        public void PopulateTypes()
        {
            foreach(var val in Enum.GetValues(typeof(EventTypes)))
            {
                eventSelection.Items.Add(val);
            }
        }

        private void EventSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string choice = eventSelection.SelectedValue.ToString();

            ShowAdditionalFeature(choice);    
        }

        private void ShowAdditionalFeature(string choice)
        {
            HideAdditionalFeatures();
            switch (choice.ToLower())
            {
                case "birthday":

                    CakeLabel.Visibility = Visibility.Visible;
                    CakeBox.Visibility = Visibility.Visible;

                    //flowerLabel.Visibility = Visibility.Hidden;
                    //flowerBox.Visibility = Visibility.Hidden;
                    //DJLabel.Visibility = Visibility.Hidden;
                    //DJBox.Visibility = Visibility.Hidden;

                    break;
                case "wedding":

                    flowerLabel.Visibility = Visibility.Visible;
                    flowerBox.Visibility = Visibility.Visible;

                    //CakeLabel.Visibility = Visibility.Hidden;
                    //CakeBox.Visibility = Visibility.Hidden;
                    //DJLabel.Visibility = Visibility.Hidden;
                    //DJBox.Visibility = Visibility.Hidden;

                    break;
                case "graduation":

                    DJLabel.Visibility = Visibility.Visible;
                    DJBox.Visibility = Visibility.Visible;

                    //flowerLabel.Visibility = Visibility.Hidden;
                    //flowerBox.Visibility = Visibility.Hidden;
                    //CakeLabel.Visibility = Visibility.Hidden;
                    //CakeBox.Visibility = Visibility.Hidden;

                    break;
            }
        }

        private void HideAdditionalFeatures()
        {
            DJLabel.Visibility = Visibility.Hidden;
            DJBox.Visibility = Visibility.Hidden;

            flowerLabel.Visibility = Visibility.Hidden;
            flowerBox.Visibility = Visibility.Hidden;

            CakeLabel.Visibility = Visibility.Hidden;
            CakeBox.Visibility = Visibility.Hidden;
        }
    }
}
