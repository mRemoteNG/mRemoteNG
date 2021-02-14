using Amazon.EC2.Model;
using System;

namespace AWSInterface
{
    public class InstanceInfo
    {
        public string InstanceId { get; }
        public string Name { get; }
        public int Userid { get; }
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

            try
            {
                int pos = name.IndexOf("User");
                if (pos >= 0)
                {
                    string temp = name.Substring(pos+4);
                    pos = temp.IndexOf(" ");
                    temp = temp.Substring(0, pos);

                    Userid = Int32.Parse(temp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            PublicIP = instance.PublicIpAddress;
            PrivateIP = instance.PrivateIpAddress;
        }
    }
}
