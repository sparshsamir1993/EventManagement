using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Serialization;

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
        EventPlan eventPlan = new EventPlan();
        MyEvent myEv = new MyEvent();
        public MyEvent MyEv { get => myEv; set => myEv = value; } 
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
            DataContext = this;
        }

        public void PopulateDays()
        {
            daySelection.Items.Clear();
            foreach(DictionaryEntry pair in datetable)
            {
                daySelection.Items.Add(pair.Value);
            }
        }
        public void PopulateTypes()
        {
            eventSelection.Items.Clear();
            foreach(var val in Enum.GetValues(typeof(EventTypes)))
            {
                eventSelection.Items.Add(val);
            }
        }
        private int returnKey(String value)
        {
            int key = -1;
            foreach (DictionaryEntry p in datetable)
            {
                int intKey = 0;
                if (p.Value.ToString() == value && int.TryParse(p.Key.ToString(), out intKey))
                {
                    key = intKey;
                }
            }
            return key;
        }

        private void EventSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(eventSelection.SelectedValue != null)
            {
                string choice = eventSelection.SelectedValue.ToString();

                ShowAdditionalFeature(choice);
            }

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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Event currentEvent = null;
            string eventDay = string.Empty;
            string eventType = string.Empty;
            int numOfPeople ;
            bool decorReq;
            string creditCard = string.Empty;

            eventDay = daySelection.SelectedValue.ToString();
            eventType = eventSelection.SelectedValue.ToString();
            numOfPeople = int.Parse(numOfPeopleBox.Text);
            decorReq = decorReqBox.Text == "y" ? true : false;
            
            creditCard = CCBox.Text;

            switch (eventType.ToLower())
            {

                case "birthday":
                   
                    string addFea = CakeBox.Text == "y" ? "Cake Required" : "Cake Not Required";
                    currentEvent = new Birthday(eventDay, eventType, numOfPeople, decorReq, creditCard, addFea);
                    break;

                case "graduation":
                    bool djReq = DJBox.Text == "y" ? true : false;
                    addFea = DJBox.Text == "y" ? "DJ Required" : "DJ Not Required";
                    currentEvent = new Graduation(eventDay, eventType, numOfPeople, decorReq, creditCard, addFea);
                    break;

                case "wedding":
                    bool flowersReq = flowerBox.Text == "y" ? true : false;
                    addFea = flowerBox.Text == "y" ? "Flowers Required" : "Flowers Not Required";
                    currentEvent = new Wedding(eventDay, eventType, numOfPeople, decorReq, creditCard, addFea);
                    
                    break;

            }
            EventList.Add(currentEvent);
            eventPlan.Add(currentEvent);
            int slot = returnKey(daySelection.SelectedValue.ToString());
            datetable.Remove(slot);
            SaveEventListInXML();
            ResetForm();
        }

        private void ResetForm()
        {
            PopulateTypes();
            //populate days 
            PopulateDays();
            HideAdditionalFeatures();
            flowerBox.Text = "";
            CCBox.Text = "";
            DJBox.Text = "";
            CakeBox.Text = "";
            numOfPeopleBox.Text = "";
            decorReqBox.Text = "";
            eventSelection.Text = "";
            daySelection.Text = "";
        }
        public static EventPlan ReadFromXML()
        {
            EventPlan listFromXML = new EventPlan();
            if (File.Exists("eventPlan.xml"))
            {

                XmlSerializer serializer = new XmlSerializer(typeof(EventPlan));
                TextReader tr = new StreamReader("eventPlan.xml");
                listFromXML = (EventPlan)serializer.Deserialize(tr);
                tr.Close();

            }
            else
            {

            }
            return listFromXML;
        }
        public  void SaveEventListInXML()
        {
            eventPlan.Sort();
            XmlSerializer serializer = new XmlSerializer(typeof(EventPlan));
            TextWriter tw = new StreamWriter("eventPlan.xml");
            serializer.Serialize(tw, eventPlan);
            tw.Close();
        }

        private void DisplayButton_Click(object sender, RoutedEventArgs e)
        {
            eventPlan.Clear();
            EventList.Clear();
            eventPlan = ReadFromXML();
            foreach(Event ev in eventPlan)
            {
                EventList.Add(ev);
            }
            eventGrid.ItemsSource = EventList;
        }

        private void LessThan200_Click(object sender, RoutedEventArgs e)
        {
            EventPlan plan = ReadFromXML();
            var query = from currEvent in plan
                        where currEvent.NumOfPeople < 200
                        select currEvent;
            EventPlan lt200 = new EventPlan();
            foreach(Event ev in query)
            {
                lt200.Add(ev);
            }
            eventGrid.ItemsSource = lt200;
        }

        private void MoreOrEq200_Click(object sender, RoutedEventArgs e)
        {
            EventPlan plan = ReadFromXML();
            var query = from currEvent in plan
                        where currEvent.NumOfPeople >= 200
                        select currEvent;
            EventPlan mt200 = new EventPlan();
            foreach (Event ev in query)
            {
                mt200.Add(ev);
            }
            eventGrid.ItemsSource = mt200;
        }

        private void TypeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            EventPlan plan = ReadFromXML();
            var query = from currEvent in plan
                        where currEvent.EventType.ToLower() == typeSearchBox.Text.ToLower()
                        select currEvent;
            EventPlan typeEv = new EventPlan();
            foreach (Event ev in query)
            {
                typeEv.Add(ev);
            }
            eventGrid.ItemsSource = typeEv;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string day = dayBox.Text.ToLower();
            string month = monthBox.Text.ToLower();
            EventPlan plan = ReadFromXML();
            var query = from currEvent in plan
                        where currEvent.EventDay.ToLower().Contains(month) && currEvent.EventDay.ToLower().Contains(day)
                        select currEvent;
            EventPlan typeEv = new EventPlan();
            foreach (Event ev in query)
            {
                typeEv.Add(ev);

                //daySelection.SelectedIndex = GetComboBoxIndex(day, month);
                daySelection.Items.Clear();
                daySelection.Items.Add(ev.EventDay);
                daySelection.SelectedIndex = 0;
                eventSelection.Items.Clear();
                eventSelection.Items.Add(ev.EventType);
                eventSelection.SelectedIndex = 0;
                numOfPeopleBox.Text = ev.NumOfPeople.ToString();
                decorReqBox.Text = ev.DecorReq ? "y" : "n";
                CCBox.Text = ev.CreditCard;

                if(ev.EventType.ToLower() == "birthday")
                {
                    CakeBox.Visibility = Visibility.Visible;
                    CakeBox.Text = ev.AdditionalFeature.ToLower().Contains("not") ? "n" : "y";
                }
                else if(ev.EventType.ToLower() == "graduation")
                {
                    DJBox.Visibility = Visibility.Visible;
                    DJBox.Text = ev.AdditionalFeature.ToLower().Contains("not") ? "n" : "y";
                }
                else if(ev.EventType.ToLower() == "wedding")
                {
                    flowerBox.Visibility = Visibility.Visible;
                    flowerBox.Text = ev.AdditionalFeature.ToLower().Contains("not") ? "n" : "y";
                }
            }
            eventGrid.ItemsSource = typeEv;
            
        }

        private int GetComboBoxIndex( string day, string month)
        {
            int key = -1;
            foreach (DictionaryEntry pair in datetable)
            {
                if(pair.Value.ToString().ToLower().Contains(day) && pair.Value.ToString().ToLower().Contains(month))
                {
                    key = 14 - (int)pair.Key;
                }
            }
            return key;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string day = dayBox.Text.ToLower();
            string month = monthBox.Text.ToLower();
            eventPlan = ReadFromXML();
            var query = from currEvent in eventPlan
                        where currEvent.EventDay.ToLower().Contains(month) && currEvent.EventDay.ToLower().Contains(day)
                        select currEvent;
            EventPlan typeEv = new EventPlan();
            int numOfPpl = 0;
            if(!int.TryParse(numOfPeopleBox.Text.ToString(), out numOfPpl))
            {
                return ;
            }
            foreach(Event currEv in eventPlan)
            {
                
                if(currEv.EventDay.ToLower().Contains(month) && currEv.EventDay.ToLower().Contains(day))
                {
                    currEv.NumOfPeople = numOfPpl;
                    currEv.DecorReq = decorReqBox.Text.ToLower() == "y" ? true : false;
                    if (currEv.EventType.ToLower() == "birthday")
                    {
                        currEv.AdditionalFeature = CakeBox.Text.ToLower() == "y" ? "Cake Required" : "Cake Not Required";
                    }
                    else if (currEv.EventType.ToLower() == "graduation")
                    {
                        currEv.AdditionalFeature = DJBox.Text.ToLower() == "y" ? "DJ Required" : "DJ Not Required";
                    }
                    else if (currEv.EventType.ToLower() == "wedding")
                    {
                        currEv.AdditionalFeature = flowerBox.Text.ToLower() == "y" ? "Flowers Required" : "Flowers Not Required";
                    }
                    currEv.CreditCard = CCBox.Text;
                    typeEv.Add(currEv);
                }
               


            }
            eventGrid.ItemsSource = typeEv;
            SaveEventListInXML();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string day = dayBox.Text.ToLower();
            string month = monthBox.Text.ToLower();
            eventPlan = ReadFromXML();
            foreach(Event currEv in eventPlan)
            {
                if (currEv.EventDay.ToLower().Contains(month) && currEv.EventDay.ToLower().Contains(day))
                {
                    eventPlan.Remove(currEv);
                    currEv.Dispose();
                    break;
                }
            }
            SaveEventListInXML();
            eventPlan = ReadFromXML();
            eventGrid.ItemsSource = eventPlan;
            ResetForm();

        }
    }
}
