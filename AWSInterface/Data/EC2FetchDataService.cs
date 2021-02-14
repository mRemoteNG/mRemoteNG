using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AWSInterface.Data
{
    public class EC2FetchDataService
    {
        public static DateTime lastFetch;
        public static List<InstanceInfo> lastData;

        public static async Task<string> GetEC2InstanceDataAsync(string input)
        {
            // get secret id
            if (!input.StartsWith("AWSAPI:"))
                throw new Exception("calling this function requires AWSAPI: input");
            string InstanceID = input.Substring(7);

            // init connection credentials, display popup if necessary
            AWSConnectionData.init();
            var alldata = await GetEC2IPDataAsync();
            var found = alldata.Where(x => x.InstanceId == InstanceID).SingleOrDefault();
            return found.PublicIP;
        }

        private static async Task<List<InstanceInfo>> GetEC2IPDataAsync()
        {
            // caching
            TimeSpan timeSpan = DateTime.Now - lastFetch;
            if (timeSpan.TotalMinutes < 1 && lastData != null)
                return lastData;

            AWSConfigs.AWSRegion = "eu-central-1";
            string awsAccessKeyId = AWSConnectionData.awsKeyID;
            string awsSecretAccessKey = AWSConnectionData.awsKey;

            var _client = new AmazonEC2Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
            bool done = false;

            List<InstanceInfo> instanceList = new List<InstanceInfo>();
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
                        InstanceInfo inf = new InstanceInfo(instance, vmname);
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
            private static RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\StrongITmRemoteAWSInterface");

            public static string awsKeyID = "";
            public static string awsKey = "";

            public static void init()
            {
                if (awsKey != "")
                    return;

                // display gui and ask for data
                AWSConnectionForm f = new AWSConnectionForm();
                f.tbAccesKeyID.Text = (string)key.GetValue("KeyID");
                f.tbAccesKey.Text = (string)key.GetValue("Key");

                DialogResult result = f.ShowDialog();

                if (f.DialogResult != DialogResult.OK)
                    return;

                // store values to memory
                awsKeyID = f.tbAccesKeyID.Text;
                awsKey = f.tbAccesKey.Text;

                // write values to registry
                key.SetValue("KeyID", awsKeyID);
                key.SetValue("Key", awsKey);
                key.Close();
            }
        }

    }
}
