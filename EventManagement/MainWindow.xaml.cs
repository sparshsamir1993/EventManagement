﻿using System;
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
        EventPlan eventPlan = new EventPlan();
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
            int numOfPeople = 0;
            bool decorReq = false;
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

                case "gradutaion":
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
    }
}
