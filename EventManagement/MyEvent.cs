namespace EventManagement
{
    public class MyEvent : Event
    {
        Event currEvent;
        public Event CurrEvent { get => currEvent; set => currEvent = value; }

        public override int GetTotal()
        {
            return 0;
        }
    }
}