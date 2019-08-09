namespace EventManagement
{
    public class MyEvent : Event
    {
        Event currEvent;
        public Event CurrEvent { get => currEvent; set => currEvent = value; }

    }
}