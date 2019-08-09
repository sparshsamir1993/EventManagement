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
            if (File.Exists("eventPlan.xml"))
            {
                File.Delete("eventPlan.xml");
            }
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
        private int returnKey(String value, Hashtable htable)
        {
            int key = -1;
            foreach (DictionaryEntry p in htable)
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
            ResetForeground();
            Event currentEvent = null;
            string eventDay = string.Empty;
            string eventType = string.Empty;
            int numOfPeople ;
            bool decorReq;
            string creditCard = string.Empty;
            string error = string.Empty;
            if(daySelection.SelectedValue == null || daySelection.SelectedValue.ToString() == "")
            {
                daySelection.Foreground = Brushes.Red;
                daySelection.ToolTip = "Can't be empty";
                return;
            }
            else if(eventSelection.SelectedValue == null || eventSelection.SelectedValue.ToString() == "")
            {
                eventSelection.Foreground = Brushes.Red;
                eventSelection.ToolTip = "can't be empty";
                return;
            }
            else if(numOfPeopleBox.Text == "" || !int.TryParse(numOfPeopleBox.Text, out numOfPeople))
            {
                numOfPeopleBox.Foreground = Brushes.Red;
                return;
            }
            else if(decorReqBox.Text == "" || (decorReqBox.Text.ToLower() != "y" && decorReqBox.Text.ToLower() != "n"))
            {
                decorReqBox.Foreground = Brushes.Red;
                return;
            }
            else if(CCBox.Text == "" || !CreditCardRule.CheckCC(CCBox.Text, out error))
            {
                
                return;
            }
            else
            {
                eventDay = daySelection.SelectedValue.ToString();
                var query = from currEv in eventPlan where currEv.EventDay == eventDay select currEv;
                if(query.Count<Event>() > 0)
                {
                    return;
                }
                eventType = eventSelection.SelectedValue.ToString();
                //numOfPeople = int.Parse(numOfPeopleBox.Text);
                decorReq = decorReqBox.Text == "y" ? true : false;

                creditCard = CCBox.Text;

                switch (eventType.ToLower())
                {

                    case "birthday":
                        if(CakeBox.Text == "")
                        {
                            CakeBox.Foreground = Brushes.Red;
                            return;
                        }
                        string addFea = CakeBox.Text == "y" ? "Cake Required" : "Cake Not Required";
                        currentEvent = new Birthday(eventDay, eventType, numOfPeople, decorReq, creditCard, addFea);
                        break;

                    case "graduation":
                        if(DJBox.Text == "")
                        {
                            return;
                        }
                        bool djReq = DJBox.Text == "y" ? true : false;
                        addFea = DJBox.Text == "y" ? "DJ Required" : "DJ Not Required";
                        currentEvent = new Graduation(eventDay, eventType, numOfPeople, decorReq, creditCard, addFea);
                        break;

                    case "wedding":
                        if(flowerBox.Text == "")
                        {
                            return;
                        }
                        bool flowersReq = flowerBox.Text == "y" ? true : false;
                        addFea = flowerBox.Text == "y" ? "Flowers Required" : "Flowers Not Required";
                        currentEvent = new Wedding(eventDay, eventType, numOfPeople, decorReq, creditCard, addFea);

                        break;

                }
                EventList.Add(currentEvent);
                eventPlan.Add(currentEvent);
                int slot = returnKey(daySelection.SelectedValue.ToString(), datetable);
                datetable.Remove(slot);
                SaveEventListInXML();
                ResetForm();
                ResetForeground();
                eventGrid.ItemsSource = eventPlan;
                ResetToolTip();
            }

        }
        public void ResetForeground()
        {
            daySelection.Foreground = Brushes.Black;
            eventSelection.Foreground = Brushes.Black;
            numOfPeopleBox.Foreground = Brushes.Black;
            decorReqBox.Foreground = Brushes.Black;
        }

        public void ResetToolTip()
        {
            daySelection.ToolTip = "";
            eventSelection.ToolTip = "";
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
            ResetForm();
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
            if (day == string.Empty)
            {
                return;
            }
            else if (month == string.Empty)
            {
                return;
            }
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
                CCBox.Text = ev.showHiddenCC();

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
            if(day == string.Empty)
            {
                return;
            }else if(month == string.Empty)
            {
                return;
            }
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
            ResetForm();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string day = dayBox.Text.ToLower();
            string month = monthBox.Text.ToLower();
            if (day == string.Empty)
            {
                return;
            }
            else if (month == string.Empty)
            {
                return;
            }
            eventPlan = ReadFromXML();
            int slot = -1;
            Hashtable dupliDateTable = GetDateTable();
            foreach (Event currEv in eventPlan)
            {
                if (currEv.EventDay.ToLower().Contains(month) && currEv.EventDay.ToLower().Contains(day))
                {
                    eventPlan.Remove(currEv);
                    currEv.Dispose();
                    slot = returnKey(currEv.EventDay, dupliDateTable);
                    if (!datetable.ContainsKey(slot))
                    {
                        datetable.Add(slot, currEv.EventDay);
                    }
                    
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
