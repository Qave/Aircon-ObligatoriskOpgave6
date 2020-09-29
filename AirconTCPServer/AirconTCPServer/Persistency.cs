using AirconLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirconTCPServer
{
    public class Persistency
    {
        public const string serverUrl = "http://localhost:58237/";
        public static List<FanOutput> FanReadings = new List<FanOutput>();
        public static async Task<List<FanOutput>> GetAllReadingsAsync()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                List<FanOutput> tableContents = new List<FanOutput>();

                var response = client.GetAsync("api/FanOutputs").Result;
                var tableData = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<FanOutput>>(tableData);
                }

            }
            return null;
        }

        private static readonly List<FanOutput> Outputs = new List<FanOutput>()
         {
             new FanOutput(1,"First Output",17,40),
             new FanOutput(2,"Second Output", 20, 55),
             new FanOutput(3,"Third Output", 22, 43),
             new FanOutput(4,"Fourth Output", 24, 80),
             new FanOutput(5,"Fifth Output",16, 60)
         };

        public static async Task LoadDataAsync()
        {
            //List<FanOutput> temp = await GetAllReadingsAsync();
            List<FanOutput> temp = Outputs;
            foreach (var item in Outputs)
            {
                FanReadings.Add(item);
                
            }
        }
    }
}
