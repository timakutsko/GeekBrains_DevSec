using System;

namespace SimpleZooKeeper
{
    public enum IsAlive
    {
        Yes,
        No
    }

    public class SelfRegistrationData
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public IsAlive Status { get; set; }

        public string Url { get; set; }

        public DateTime Created { get; set; }
    }
}
