using System;

namespace EventManagement
{
    public abstract class Event : IComparable<Event>, IDisposable
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

        public int CompareTo(Event other)
        {
            if(other != null)
            {
                return this.EventDay.CompareTo(other.EventDay);
            }

            return 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}