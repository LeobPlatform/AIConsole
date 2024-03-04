using AIConsole.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace AIConsole.Services
{
    public class BlockOneDataService
    {
        public HttpClient BlockOneHttpClient { get; set; }

        private SocketsHttpHandler myHttpHandler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = TimeSpan.FromHours(1),
            PooledConnectionLifetime = TimeSpan.FromHours(1)
        };

        public BlockOneDataService()
        {
            BlockOneHttpClient = new HttpClient(myHttpHandler);
        }

        public async Task<List<TrainingImageData>> GetVisionModelImagesForTraining(string BlockOneID,int Status, int WCSID, int Surface, int PageIndex, int PageSize)
        {
            string requestUrl = string.Format("https://{0}.blockone.com/api/ai/GetVisionModelImagesForTraining?Status={1}&WCSID={2}&Surface={3}&PageIndex={4}&PageSize={5}", BlockOneID, Status, WCSID, Surface, PageIndex, PageSize);
            string responseString = await BlockOneHttpClient.GetStringAsync(requestUrl);
            string parseResult = GetResultFromJson(responseString);
            if (parseResult != string.Empty)
            {
                return JsonConvert.DeserializeObject<List<TrainingImageData>>(parseResult);
            }
            return new List<TrainingImageData> { };
        }

        public async Task<List<DailyImageData>> DownloadBoxVisionImagesForDaily(string BlockOneID, int WCSID, int IntervalDays, int PageIndex, int PageSize)
        {
            string requestUrl = string.Format("https://{0}.blockone.com/api/ai/GetBoxVisionImagesForDaily?WCSID={1}&IntervalDays={2}&PageIndex={3}&PageSize={4}", BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);
            string responseString = await BlockOneHttpClient.GetStringAsync(requestUrl);
            string parseResult = GetResultFromJson(responseString);
            if (parseResult != string.Empty)
            {
                return JsonConvert.DeserializeObject<List<DailyImageData>>(parseResult);
            }
            return new List<DailyImageData> { };
        }

        public async Task<List<NoReadImageData>> DownloadBoxVisionImagesForNoRead(string BlockOneID, int WCSID, int IntervalDays, int PageIndex, int PageSize)
        {
            string requestUrl = string.Format("https://{0}.blockone.com/api/ai/GetBoxVisionImagesForNoRead?WCSID={1}&IntervalDays={2}&PageIndex={3}&PageSize={4}", BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);
            string responseString = await BlockOneHttpClient.GetStringAsync(requestUrl);
            string parseResult = GetResultFromJson(responseString);
            if (parseResult != string.Empty)
            {
                return JsonConvert.DeserializeObject<List<NoReadImageData>>(parseResult);
            }
            return new List<NoReadImageData> { };
        }

        public string GetResultFromJson(string data)
        {
            string result = string.Empty;
            try
            {
                JToken? outData;
                JObject dataObj = JObject.Parse(data);
                bool parseResult = dataObj.TryGetValue("data", StringComparison.Ordinal, out outData);
                if (parseResult)
                {
                    result = outData.ToString();
                }

            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }
    }
}
