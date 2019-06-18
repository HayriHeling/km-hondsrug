using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Index(int id=1)
        {
            return View(CreateTimeLineModel(id));
        }

        public TimelineModel CreateTimeLineModel(int id)
        {
            return new TimelineModel
            {
                Name = "Tijdlijn 1",
                TimeblockModels = CreateTimeblockModels(id)
            };
        }

        public List<TimeblockModel> CreateTimeblockModels(int userId)
        {
            List<TimeblockModel> outputTimeblockModels = new List<TimeblockModel>();
            IEnumerable<TimeTable> timeTables = TimeTableService.GetAll();
            foreach (TimeTable timeTable in timeTables)
            {
                outputTimeblockModels.Add(new TimeblockModel
                {
                    TimeTableModel = ConvertToTimeTableModel(timeTable),
                    TimeBlockInformationModels = CreateTimeBlockInformationModels(timeTable.TimeTableId, userId)
                });
            }

            return outputTimeblockModels;
        }

        /// <summary>
        /// Method that creates TimeBlockInformationModels. If the userId is not specified it will just show all timeblockinformations.
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimeBlockInformationModel> CreateTimeBlockInformationModels(int timeTableId, int userId=-1)
        {
            List<TimeBlockInformationModel> outputTimeBlockInformationModels = new List<TimeBlockInformationModel>();
            IEnumerable<TimeTableInformation> timeTableInformations = GetCorrectTableInformations(timeTableId, userId);
            

            List<TimeBlockInformationModel> bcModels = new List<TimeBlockInformationModel>();
            List<TimeBlockInformationModel> acModels = new List<TimeBlockInformationModel>();

            foreach (TimeTableInformation timeTableInformation in timeTableInformations)
            {
                if (timeTableInformation.BeforeChrist == 1)
                {
                    bcModels.Add(ConvertToTimeBlockInformationModel(timeTableInformation));
                }
                else
                {
                    acModels.Add(ConvertToTimeBlockInformationModel(timeTableInformation));
                }
            }

            bcModels = SortTimeBlockInfoModels(bcModels, true);
            acModels = SortTimeBlockInfoModels(acModels, false);

            outputTimeBlockInformationModels.AddRange(bcModels);
            outputTimeBlockInformationModels.AddRange(acModels);

            return outputTimeBlockInformationModels;
        }

        /// <summary>
        /// Sorting method for TimeBlockInformationModels, descending if the input bool is true. It returns a sorted list.
        /// </summary>
        /// <param name="tBImodels"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public List<TimeBlockInformationModel> SortTimeBlockInfoModels(List<TimeBlockInformationModel> tBImodels, bool isDescending)
        {
            List<TimeBlockInformationModel> outputList = tBImodels;
            if (isDescending)
            {
                outputList = tBImodels.OrderByDescending(x => x.Year).ToList();
            }
            else
            {
                outputList = tBImodels.OrderBy(x => x.Year).ToList();
            }

            return outputList;
        }

        /// <summary>
        /// Method that gets the correct TableInformation depending on the userid and the timetableId.
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <param name="userId"></param>
        /// <returns>An IEnumerable with TimeTableInformation objects inside.</returns>
        public IEnumerable<TimeTableInformation> GetCorrectTableInformations(int timeTableId, int userId)
        {
            IEnumerable<TimeTableInformation> timeTableInformations = new List<TimeTableInformation>();
            if (userId >= 0)
            {
                timeTableInformations = TimeTableInformationService.GetAllByTimeTableUserId(timeTableId, userId);
            }
            else
            {
                timeTableInformations = TimeTableInformationService.GetAllByTimeTableId(timeTableId);
            }

            return timeTableInformations;
        }

        /// <summary>
        /// Method that creates and returns a list of MediaSourceModels from a given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converter for Timetable>TimeTableModel
        /// </summary>
        /// <param name="timeTable"></param>
        /// <returns></returns>
        public TimeTableModel ConvertToTimeTableModel(TimeTable timeTable)
        {
            return new TimeTableModel
            {
                TimeTableId = timeTable.TimeTableId,
                MediaSourceModel = ConvertToMediaSourceModel(MediaSourceService.GetById(timeTable.MediaSourceId)),
                Text = timeTable.Text
            };
        }

        /// <summary>
        /// Converter for MediaSource>MediaSourceModel
        /// </summary>
        /// <param name="mediaSource"></param>
        /// <returns></returns>
        public MediaSourceModel ConvertToMediaSourceModel(MediaSource mediaSource)
        {
            return new MediaSourceModel
            {
                MediaSourceId = mediaSource.MediaSourceId,
                MediaType = (MediaType)mediaSource.MediaType,
                Source = mediaSource.Source
            };
        }

        /// <summary>
        /// Converter for TimeTableInformation>TimeBlockInformationModel
        /// </summary>
        /// <param name="timeTableInformation"></param>
        /// <returns></returns>
        public TimeBlockInformationModel ConvertToTimeBlockInformationModel(TimeTableInformation timeTableInformation)
        {
            return new TimeBlockInformationModel
            {
                Name = timeTableInformation.Name,
                Description = timeTableInformation.Description,
                BeforeChrist = timeTableInformation.BeforeChrist.Equals(1),
                Year = timeTableInformation.Year,
                MediaSourceModels = CreateMediaSourceModels(timeTableInformation.TimeTableInformationId)
            };
        }
    }
}