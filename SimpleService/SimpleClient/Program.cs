using System;

namespace SimpleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ServiceDiscoveryPatterns.PointToPointPattern();
            //ServiceDiscoveryPatterns.LoacalRegistryPattern();
            ServiceDiscoveryPatterns.SelfRegistrationPattern();
        }
    }
}
