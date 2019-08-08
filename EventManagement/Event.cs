namespace EventManagement
{
    public abstract class Event
    {
        private string eventDay;
        private string eventType;
        private int numOfPeople;
        private bool decorReq;
        private string creditCard;
        private string additionalFeature;

        public string EventDay { get => eventDay; set => eventDay = value; }
        public string EventType { get => eventType; set => eventType = value; }
        public int NumOfPeople { get => numOfPeople; set => numOfPeople = value; }
        public bool DecorReq { get => decorReq; set => decorReq = value; }
        public string CreditCard { get => creditCard; set => creditCard = value; }
        public string AdditionalFeature { get => additionalFeature; set => additionalFeature = value; }

    }
}