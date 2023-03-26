using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleZooKeeper
{
    public class ServiceRepository
    {
        private List<SelfRegistrationData> _selfRegistrationDataRepository;

        public ServiceRepository()
        {
            _selfRegistrationDataRepository = new List<SelfRegistrationData>();
        }

        public SelfRegistrationData AddService(SelfRegistrationData selfRegistrationData)
        {
            selfRegistrationData.Created = DateTime.Now;
            _selfRegistrationDataRepository.Add(selfRegistrationData);

            return selfRegistrationData;
        }

        public IEnumerable<SelfRegistrationData> GetAllServices() => _selfRegistrationDataRepository;

        public void Remove(SelfRegistrationData selfRegistrationData) => _selfRegistrationDataRepository.RemoveAll(s => s.Name == selfRegistrationData.Name);
    }
}
