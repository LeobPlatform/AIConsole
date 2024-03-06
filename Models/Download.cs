using AIConsole.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using Newtonsoft.Json;

namespace AIConsole.Models
{

    public class TrainingImageData
    {
        public int VisionModelImageID { get; set; }
        public int DocumentID { get; set; }
        public int ProductID { get; set; }
        public string FileName { get; set; }
        public string FileKey { get; set; }
        public string Tags { get; set; }
        public string WCSModuleName { get; set; }
        public string WCSName { get; set; }
        public string URL { get; set; }
    }

    public class DailyImageData
    {
        public int BoxVisionID { get; set; }
        public int ProductID { get; set; }
        public int WCSID { get; set; }
        public string TimeScanned { get; set; }
        public string WCSModuleName { get; set; }
        public string WCSName { get; set; }
        public string SideImageName { get; set; }
        public string TopImageName { get; set; }
        public string SideImageURL { get; set; }
        public string TopImageURL { get; set; }
    }

    public class NoReadImageData
    {
        public int BoxVisionID { get; set; }
        public string TimeScanned { get; set; }
        public string WCSModuleName { get; set; }
        public string FileName { get; set; }
        public string Tags { get; set; }
        public string WCSName { get; set; }
        public string URL { get; set; }
    }

    public class Download
    {
        private readonly BlockOneDataService _dataService;
        public Download(BlockOneDataService blockOneDataService)
        {
            _dataService = blockOneDataService;
        }

        public async Task<string> DownloadVisionModelImagesForTraining(string BlockOneID, int Status, int WCSID, int Surface, int PageIndex, int PageSize)
        {
            List<TrainingImageData> ImageData = await _dataService.GetVisionModelImagesForTraining(BlockOneID, Status, WCSID, Surface, PageIndex, PageSize);

            Dictionary<string,List<TrainingImageData>> result = new Dictionary<string,List<TrainingImageData>>();
            result.Add("Success",new List<TrainingImageData>());
            result.Add("Fsiled",new List<TrainingImageData>());
            foreach (var item in ImageData)
            {
                string savePath = $"\\\\10.0.20.73\\NetworkShare\\JianhuaTest\\Customer\\Savannah\\VisionModelImagesForTraining\\{item.WCSName}\\{item.WCSModuleName??"-"}\\{item.Tags}\\{item.ProductID}";
                if (await DownloadFile(item.URL, item.FileName, savePath))
                {
                    result["Success"].Add(item);
                }
                else
                {
                    result["Fsiled"].Add(item);
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<string> DownloadBoxVisionImagesForDaily(string BlockOneID, int WCSID, int IntervalDays, int PageIndex, int PageSize)
        {
            List<DailyImageData> ImageData = await _dataService.DownloadBoxVisionImagesForDaily(BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);

            Dictionary<string, List<DailyImageData>> result = new Dictionary<string, List<DailyImageData>>();
            result.Add("Success", new List<DailyImageData>());
            result.Add("Fsiled", new List<DailyImageData>());
            foreach (var item in ImageData)
            {
                string savePath = $"\\\\10.0.20.73\\NetworkShare\\JianhuaTest\\Customer\\Savannah\\BoxVisionImagesForDaily\\{item.WCSName}\\{item.WCSModuleName ?? "-"}\\{item.ProductID}";
                if (await DownloadFile(item.TopImageURL, item.TopImageName, savePath) && await DownloadFile(item.SideImageURL, item.SideImageName, savePath))
                {
                    result["Success"].Add(item);
                }
                else
                {
                    result["Fsiled"].Add(item);
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<string> DownloadBoxVisionImagesForNoRead(string BlockOneID, int WCSID, int IntervalDays, int PageIndex, int PageSize)
        {
            List<NoReadImageData> ImageData = await _dataService.DownloadBoxVisionImagesForNoRead(BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);

            Dictionary<string, List<NoReadImageData>> result = new Dictionary<string, List<NoReadImageData>>();
            result.Add("Success", new List<NoReadImageData>());
            result.Add("Fsiled", new List<NoReadImageData>());
            foreach (var item in ImageData)
            {
                string savePath = $"\\\\10.0.20.73\\NetworkShare\\JianhuaTest\\Customer\\Savannah\\VisionModelImagesForTraining\\{item.WCSName}\\{item.WCSModuleName ?? "-"}\\{item.Tags}";
                if (await DownloadFile(item.URL, item.FileName, savePath))
                {
                    result["Success"].Add(item);
                }
                else
                {
                    result["Fsiled"].Add(item);
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">客户端保存的文件名</param>
        /// <param name="filePath">保存的文件夹路径</param>
        /// <returns></returns>
        public async Task<bool> DownloadFile(string url, string fileName, string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                client.DownloadFile(url, filePath + "\\" + fileName);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
