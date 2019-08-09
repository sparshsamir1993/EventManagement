namespace EventManagement
{
    public class Birthday : Event
    {
        private bool cakeReq;
        public Birthday()
        {
           
        }

        public Birthday(string eventDay, string eventType, int numOfPeople, bool decorReq, string creditCard, string addFea)
        {
            EventDay = eventDay;
            EventType = eventType;
            NumOfPeople= numOfPeople;
            DecorReq= decorReq;
            CreditCard= creditCard;
            AdditionalFeature = addFea;
        }

        public bool CakeReq { get => cakeReq; set => cakeReq = value; }

        public override int GetTotal()
        {
            return 50 + (10 * NumOfPeople);
        }
    }
}