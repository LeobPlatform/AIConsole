using AIConsole.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

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
        public string TopImageURL { get; set; }
        public string SideImageURL { get; set; }
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
            int index = 0;
            foreach (var item in ImageData)
            {
                await DownloadFile("https://dubhe-tenant-public-files.s3.amazonaws.com/" + item.FileKey, index + ".jpg", "\\\\10.0.20.73\\NetworkShare\\JianhuaTest");
                index += 1;
            }

            return "";
        }

        public async Task<string> DownloadBoxVisionImagesForDaily(string BlockOneID, int WCSID, int IntervalDays, int PageIndex, int PageSize)
        {
            List<DailyImageData> ImageData = await _dataService.DownloadBoxVisionImagesForDaily(BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);

            return "";
        }

        public async Task<string> DownloadBoxVisionImagesForNoRead(string BlockOneID, int WCSID, int IntervalDays, int PageIndex, int PageSize)
        {
            List<NoReadImageData> ImageData = await _dataService.DownloadBoxVisionImagesForNoRead(BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);

            return "";
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
