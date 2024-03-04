using AIConsole.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIConsole.Controllers
{
    [Route("[controller]/[action]")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _logger;
        private readonly Download _download;

        public APIController(ILogger<APIController> logger, Download download)
        {
            _logger = logger;
            _download = download;
        }

        [HttpGet]
        public async Task<string> DownloadVisionModelImagesForTraining([FromQuery] string BlockOneID, [FromQuery] int Status, [FromQuery] int WCSID, [FromQuery] int Surface, [FromQuery] int PageIndex = 0, [FromQuery] int PageSize = 1000)
        {
            return await _download.DownloadVisionModelImagesForTraining(BlockOneID, Status, WCSID, Surface, PageIndex, PageSize);
        }

        [HttpGet]
        public async Task<string> DownloadBoxVisionImagesForDaily([FromQuery] string BlockOneID, [FromQuery] int WCSID, [FromQuery] int IntervalDays, [FromQuery] int PageIndex = 0, [FromQuery] int PageSize = 1000)
        {
            return await _download.DownloadBoxVisionImagesForDaily(BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);
        }

        [HttpGet]
        public async Task<string> DownloadBoxVisionImagesForNoRead([FromQuery] string BlockOneID, [FromQuery] int WCSID, [FromQuery] int IntervalDays, [FromQuery] int PageIndex = 0, [FromQuery] int PageSize = 1000)
        {
            return await _download.DownloadBoxVisionImagesForNoRead(BlockOneID, WCSID, IntervalDays, PageIndex, PageSize);
        }
    }
}
