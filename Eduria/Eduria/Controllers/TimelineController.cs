using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using EduriaData.Models.TimeLineLayer;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class TimelineController : Controller
    {
        private TimeTableService TimeTableService;
        private TimeTableInformationService TimeTableInformationService;
        private TimeTableInfoMediaSrcService TimeTableInfoMediaSrcService;
        private MediaSourceService MediaSourceService;

        public TimelineController(TimeTableService timeTableService,
            TimeTableInformationService timeTableInformationService,
            TimeTableInfoMediaSrcService timeTableInfoMediaSrcService,
            MediaSourceService mediaSourceService)
        {
            TimeTableService = timeTableService;
            TimeTableInformationService = timeTableInformationService;
            TimeTableInfoMediaSrcService = timeTableInfoMediaSrcService;
            MediaSourceService = mediaSourceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public TimelineModel CreateTimeLineModel()
        {
            return new TimelineModel
            {
                Name = "Tijdlijn 1",
                TimeblockModels = CreateTimeblockModels(1)
            };
        }

        public List<TimeblockModel> CreateTimeblockModels(int timeTableId)
        {
            List<TimeblockModel> outputTimeblockModels = new List<TimeblockModel>();
            outputTimeblockModels.Add(new TimeblockModel
            {
                TimeTableModel = ConvertToTimeTableModel(TimeTableService.GetById(timeTableId)),
                TimeBlockInformationModels = CreateTimeBlockInformationModels()
            });

            return outputTimeblockModels;
        }

        public List<TimeBlockInformationModel> CreateTimeBlockInformationModels()
        {
            List<TimeBlockInformationModel> outputTimeBlockInformationModels = new List<TimeBlockInformationModel>();
            outputTimeBlockInformationModels.Add(new TimeBlockInformationModel());

            return outputTimeBlockInformationModels;
        }

        public TimeBlockInformationModel ConvertToTimeBlockInformationModel(TimeTableInformation timeTableInformation)
        {
            return new TimeBlockInformationModel
            {
                Description = timeTableInformation.Description,
                MediaSourceModels = CreateMediaSourceModels(timeTableInformation.TimeTableInformationId)
            };
        }

        public List<MediaSourceModel> CreateMediaSourceModels(int id)
        {
            IEnumerable<TimeTableInfoHasMediaSrc> timeTableInfoHasMediaSrcs =
                TimeTableInfoMediaSrcService.GetAllByTimeTableInfoId(id);
            List<MediaSourceModel> mediaSourceModels = new List<MediaSourceModel>();
            foreach (TimeTableInfoHasMediaSrc src in timeTableInfoHasMediaSrcs)
            {
                mediaSourceModels.Add(ConvertToMediaSourceModel(MediaSourceService.GetById(src.MediaSourceId)));
            }

            return mediaSourceModels;
        }

        public TimeTableModel ConvertToTimeTableModel(TimeTable timeTable)
        {
            return new TimeTableModel
            {
                TimeTableId = timeTable.TimeTableId,
                MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(timeTable.MediaSourceId)),
                Text = timeTable.Text
            };
        }

        public MediaSourceModel ConvertToMediaSourceModel(MediaSource mediaSource)
        {
            return new MediaSourceModel
            {
                MediaSourceId = mediaSource.MediaSourceId,
                MediaType = (MediaType)mediaSource.MediaType,
                Source = mediaSource.Source
            };
        }
    }
}