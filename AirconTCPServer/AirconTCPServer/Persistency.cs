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
        public static async Task LoadDataAsync()
        {
            List<FanOutput> temp = await GetAllReadingsAsync();
            foreach (var item in temp)
            {
                FanReadings.Add(item);
                
            }
        }
    }
}
