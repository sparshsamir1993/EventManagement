namespace EventManagement
{
    public class MyEvent : Event
    {
        Event currEvent;
        string month;
        public Event CurrEvent { get => currEvent; set => currEvent = value; }
        public string Month { get => month; set => month = value; }

        public override int GetTotal()
        {
            return 0;
        }
    }
}