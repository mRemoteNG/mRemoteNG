using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Microsoft.Win32;

namespace ExternalConnectors.AWS
{
    public class EC2FetchDataService
    {
        private static DateTime lastFetch;
        private static List<InstanceInfo>? lastData;

        // input must be in format "AWSAPI:instanceid" where instanceid is the ec2 instance id, e.g. i-066f750a76c97583d
        public static async Task<string> GetEC2InstanceDataAsync(string input, string region)
        {
            // get secret id
            if (!input.StartsWith("AWSAPI:"))
                throw new Exception("calling this function requires AWSAPI: input");
            string InstanceID = input[7..];

            // init connection credentials, display popup if necessary
            AWSConnectionData.Init();
            var alldata = await GetEC2IPDataAsync(region);
            var found = alldata.Where(x => x.InstanceId == InstanceID).SingleOrDefault();
            return (found == null) ? "" : found.PublicIP;
        }

        private static async Task<List<InstanceInfo>> GetEC2IPDataAsync(string region)
        {
            // caching
            TimeSpan timeSpan = DateTime.Now - lastFetch;
            if (timeSpan.TotalMinutes < 1 && lastData != null)
                return lastData;

            //AWSConfigs.AWSRegion = AWSConnectionData.region;
            AWSConfigs.AWSRegion = region;
            string awsAccessKeyId = AWSConnectionData.awsKeyID;
            string awsSecretAccessKey = AWSConnectionData.awsKey;

            var _client = new AmazonEC2Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
            bool done = false;

            List<InstanceInfo> instanceList = new();
            var request = new DescribeInstancesRequest();
            while (!done)
            {
                DescribeInstancesResponse response = await _client.DescribeInstancesAsync(request);

                foreach (var reservation in response.Reservations)
                {
                    foreach (var instance in reservation.Instances)
                    {
                        string vmname = "";
                        foreach (var tag in instance.Tags)
                        {
                            if (tag.Key == "Name")
                            {
                                vmname = tag.Value;
                            }
                        }
                        InstanceInfo inf = new(instance, vmname);
                        instanceList.Add(inf);
                    }
                }

                request.NextToken = response.NextToken;

                if (response.NextToken == null)
                {
                    done = true;
                }
            }

            lastData = instanceList.OrderBy(x => x.Name).ToList();
            lastFetch = DateTime.Now;
            return lastData;
        }


        public static class AWSConnectionData
        {
            private static readonly RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\mRemoteAWSInterface");

            public static string awsKeyID = "";
            public static string awsKey = "";
            //public static string _region = "eu-central-1";

            public static void Init()
            {
                if (awsKey != "")
                    return;
                // display gui and ask for data
                AWSConnectionForm f = new();
                f.tbAccesKeyID.Text = "" + key.GetValue("KeyID");
                f.tbAccesKey.Text = "" + key.GetValue("Key");
                //f.tbRegion.Text = "" + key.GetValue("Region");
                //if (f.tbRegion.Text == null || f.tbRegion.Text.Length < 2)
                //    f.tbRegion.Text = region;
                _ = f.ShowDialog();

                if (f.DialogResult != DialogResult.OK)
                    return;

                // store values to memory
                awsKeyID = f.tbAccesKeyID.Text;
                awsKey = f.tbAccesKey.Text;
                //region = f.tbRegion.Text;


                // write values to registry
                key.SetValue("KeyID", awsKeyID);
                key.SetValue("Key", awsKey);
                //key.SetValue("Region", region);
                key.Close();
            }
        }

    }
}
