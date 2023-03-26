using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SimpleZooKeeper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleZooKeeperController : ControllerBase
    {
        private readonly ILogger<SimpleZooKeeperController> _logger;
        private readonly ServiceRepository _serviceRepository;
        private readonly HealthChecker _healthChecker;

        public SimpleZooKeeperController(ILogger<SimpleZooKeeperController> logger, ServiceRepository serviceRepository, HealthChecker healthChecker)
        {
            _logger = logger;
            _serviceRepository = serviceRepository;
            _healthChecker = healthChecker;
        }

        [HttpGet]
        public IEnumerable<SelfRegistrationData> Get()
        {
            IEnumerable<SelfRegistrationData> services = _serviceRepository.GetAllServices();
            foreach (var service in services)
            {
                _healthChecker.Check(service);
                if (service.Status == IsAlive.No)
                    _serviceRepository.Remove(service);
            }
            
            return services;
        }

        [HttpPost]
        public SelfRegistrationData RegistryService([FromBody] SelfRegistrationData selfRegistrationData)
        {
            return _serviceRepository.AddService(selfRegistrationData);
        }
    }
}
