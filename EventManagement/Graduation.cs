namespace EventManagement
{
    public class Graduation : Event
    {
        private bool djReq;
        public bool DJReq { get => djReq; set => djReq = value; }
        public Graduation()
        {

        }
        public Graduation(string eventDay, string eventType, int numOfPeople, bool decorReq, string creditCard, string addFea)
        {
            EventDay = eventDay;
            EventType = eventType;
            NumOfPeople = numOfPeople;
            DecorReq = decorReq;
            CreditCard = creditCard;
            AdditionalFeature = addFea;
        }
    }
}