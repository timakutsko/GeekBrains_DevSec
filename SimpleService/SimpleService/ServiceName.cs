using System;

namespace SimpleService
{
    public static class ServiceName
    {
        public static string Name { get; set; } = Guid.NewGuid().ToString();
    }
}
