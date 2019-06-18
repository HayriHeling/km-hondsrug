using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            return View(CreateTimeLineModel());
        }

        public TimelineModel CreateTimeLineModel()
        {
            return new TimelineModel
            {
                Name = "Tijdlijn 1",
                // TODO: userid ophalen en toevoegen
                TimeblockModels = CreateTimeblockModels(userId: 1)
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
            IEnumerable<TimeTableInformation> timeTableInformations = new List<TimeTableInformation>();
            if (userId >= 0)
            {
                timeTableInformations = TimeTableInformationService.GetAllByTimeTableUserId(timeTableId, userId);
            }
            else
            {
                timeTableInformations = TimeTableInformationService.GetAllByTimeTableId(timeTableId);
            }

            foreach (TimeTableInformation timeTableInformation in timeTableInformations)
            {
                outputTimeBlockInformationModels.Add(ConvertToTimeBlockInformationModel(timeTableInformation));
            }

            return outputTimeBlockInformationModels;
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

        public TimeBlockInformationModel ConvertToTimeBlockInformationModel(TimeTableInformation timeTableInformation)
        {
            return new TimeBlockInformationModel
            {
                Name = timeTableInformation.Name,
                Description = timeTableInformation.Description,
                BeforeChrist = (ChristNotation)timeTableInformation.BeforeChrist,
                Year = timeTableInformation.Year,
                MediaSourceModels = CreateMediaSourceModels(timeTableInformation.TimeTableInformationId)
            };
        }
        public TimeTableInformation ConvertToTimeTableInformation(TimeBlockInformationModel model)
        {
            return new TimeTableInformation()
            {
                TimeTableId = model.TimeTable.TimeTableId,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Name = model.Name,
                Description = model.Description,
                BeforeChrist = (int)model.BeforeChrist,
                Year = model.Year
            };
        }

        public IActionResult CreateInformation(int timeTableId)
        {
            IEnumerable<TimeTable> tables = TimeTableService.GetAll();
            List<TimeTableModel> tableModels = new List<TimeTableModel>();
            foreach(TimeTable table in tables)
            {
                TimeTableModel tableModel = new TimeTableModel()
                {
                    TimeTableId = table.TimeTableId,
                    Text = table.Text
                };
                tableModels.Add(tableModel);
            }
            ViewBag.timetables = tableModels;
            ViewBag.timetableId = timeTableId;
            return View();
        }

        [HttpPost]
        public IActionResult CreateInformation(TimeBlockInformationModel info, int timeTableId)
        {
            IEnumerable<TimeTable> tables = TimeTableService.GetAll();
            List<TimeTableModel> tableModels = new List<TimeTableModel>();
            foreach (TimeTable table in tables)
            {
                TimeTableModel tableModel = new TimeTableModel()
                {
                    TimeTableId = table.TimeTableId,
                    Text = table.Text
                };
                tableModels.Add(tableModel);
            }
            ViewBag.timetables = tableModels;
            ViewBag.timetableId = timeTableId;
            try
            {
                info.TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(timeTableId));
                TimeTableInformationService.Add(ConvertToTimeTableInformation(info));
            }
            catch(Exception e)
            {
                throw e;
            }
            return View();
        }

        public IActionResult EditInformation(int id)
        {
            ViewBag.infoModel = ConvertToTimeBlockInformationModel(TimeTableInformationService.GetById(id));
            return View();
        }

        [HttpPost]
        public IActionResult EditInformation(TimeBlockInformationModel info, int timeTableId)
        {
            try
            {
                info.TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(timeTableId));
                TimeTableInformationService.Update(ConvertToTimeTableInformation(info));
                return View();
            }
            catch(Exception e)
            {
                throw e;
            }            
        }

        public IActionResult DeleteInformation(int id)
        {
            ViewBag.infoModel = ConvertToTimeBlockInformationModel(TimeTableInformationService.GetById(id));
            return View();
        }

        public void DeleteInformationFromDatabase(int id)
        {
            try
            {
                TimeTableInformationService.Delete(TimeTableInformationService.GetById(id));
            }
            catch(Exception e)
            {
                throw e;
            }

        }
    }
}