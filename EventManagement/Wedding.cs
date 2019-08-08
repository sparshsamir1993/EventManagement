namespace EventManagement
{
    public class Wedding : Event
    {
        private bool flowersReq;
        public bool FlowersReq { get => flowersReq; set => flowersReq = value; }
        public Wedding(string eventDay, string eventType, int numOfPeople, bool decorReq, string creditCard, string  addFea)
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