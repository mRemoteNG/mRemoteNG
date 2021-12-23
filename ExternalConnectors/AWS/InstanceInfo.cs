using Amazon.EC2.Model;
using System;

namespace ExternalConnectors.AWS
{
    public class InstanceInfo
    {
        public string InstanceId { get; }
        public string Name { get; }
        public string Status { get; }
        public string PublicIP { get; }
        public string PrivateIP { get; }
        public InstanceInfo(Instance instance, string name)
        {
            InstanceId = instance.InstanceId;
            Name = name;

            switch(instance.State.Code)
            {
                case 0: Status = "Pending"; break;
                case 16: Status = "Running"; break;
                case 32: Status = "Shutdown"; break;
                case 48: Status = "Terminated"; break;
                case 64: Status = "Stopping"; break;
                case 80: Status = "Stopped"; break;
                default: Status = "Unknown"; break;
            };

            PublicIP = instance.PublicIpAddress ?? "";
            PrivateIP = instance.PrivateIpAddress ?? "";

        }
    }
}
