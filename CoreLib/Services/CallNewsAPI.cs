using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace CoreLib.Services
{
    public static class CallNewsAPI
    {
        public static HttpClient client = new HttpClient();

        public static void Unit()
        {
            client.BaseAddress = new Uri("");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<HttpResponseMessage> PostNewsAsync(NewsWebsiteAPI news, string LinkAPI)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(LinkAPI,news);

            // return URI of the created resource.
            return response;
        }

        //  Lấy tin tức
        public static async Task<TinTucWebApiNew> API_GetNew(string strApiName)
        {
            TinTucWebApiNew objReturn = new TinTucWebApiNew();
            // Get http request
            try
            {
                WebRequest request = WebRequest.Create(strApiName);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();             
                objReturn = (TinTucWebApiNew)JsonConvert.DeserializeObject<TinTucWebApiNew>(responseFromServer);
            }
            catch (Exception ex)
            {
            }
            return objReturn;
        }
    }
}
