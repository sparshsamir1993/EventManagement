using System.Collections;
using System.Collections.Generic;

namespace EventManagement
{
    public class EventPlan : IEnumerable<Event>
    {
        private List<Event> eventList = null;
        public List<Event> EventList { get => eventList; set => eventList = value; }
        public EventPlan()
        {
            eventList = new List<Event>();
        }

        public void Add(Event e)
        {
            eventList.Add(e);
        }
        public int Count
        {
            get => eventList.Count;
        }
        public IEnumerator<Event> GetEnumerator()
        {
            return ((IEnumerable<Event>)eventList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Event>)eventList).GetEnumerator();
        }

        public Event this[int i]
        {
            get { return eventList[i]; }
            set { eventList[i] = value; }
        }
        public void Clear()
        {
            EventList.Clear();
        }
    }
}