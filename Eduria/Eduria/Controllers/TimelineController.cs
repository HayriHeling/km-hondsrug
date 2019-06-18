using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using EduriaData.Models.TimeLineLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class TimelineController : Controller
    {
        private TimeTableService TimeTableService;
        private TimeTableInformationService TimeTableInformationService;
        private TimeTableInfoMediaSrcService TimeTableInfoMediaSrcService;
        private MediaSourceService MediaSourceService;

        private static Random random = new Random();

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
                TimeBlockInformationId = timeTableInformation.TimeTableInformationId,
                Name = timeTableInformation.Name,
                Description = timeTableInformation.Description,
                BeforeChrist = (ChristNotation)timeTableInformation.BeforeChrist,
                Year = timeTableInformation.Year,
                MediaSourceModels = CreateMediaSourceModels(timeTableInformation.TimeTableInformationId),
                TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(timeTableInformation.TimeTableId))               
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
            ViewBag.infoModel = ConvertToTimeBlockInformationModel(TimeTableInformationService.GetById(id));
            return View();
        }

        [HttpPost]
        public IActionResult EditInformation(TimeBlockInformationModel info, int timeTableId)
        {
            try
            {
                info.TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(timeTableId));
                TimeTableInformation toChange = TimeTableInformationService.GetById(info.TimeBlockInformationId);
                TimeTableInformation changed = ConvertToTimeTableInformation(info);
                toChange.TimeTableId = changed.TimeTableId;
                toChange.Name = changed.Name;
                toChange.Description = changed.Description;
                toChange.BeforeChrist = changed.BeforeChrist;
                toChange.Year = changed.Year;
                TimeTableInformationService.Update(toChange);
                return RedirectToAction("index");
            }
            catch(Exception e)
            {
                throw e;
            }           
        }

        public IActionResult DeleteInformation(int id)
        {
            try
            {
                TimeTableInformationService.Delete(TimeTableInformationService.GetById(id));
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public IActionResult Upload(int id)
        {
            IEnumerable InfoHasMedia = TimeTableInfoMediaSrcService.GetAllByTimeTableInfoId(id);
            List<MediaSourceModel> media = new List<MediaSourceModel>();
            foreach (TimeTableInfoHasMediaSrc x in InfoHasMedia)
            {
                MediaSource ms = MediaSourceService.GetById(x.MediaSourceId);
                media.Add(ConvertToMediaSourceModel(ms));
            }
            ViewBag.media = media;
            ViewBag.infoId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMediaToInformationAsync(List<IFormFile> files, int mType, int infoId)
        {
            try
            {
                long size = files.Sum(f => f.Length);
                var filePath = "lol";
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string[] arr = formFile.FileName.Split(".");
                        string fileName = RandomString(20);
                        string ext = arr[arr.Length - 1];
                        string newName = "infoMedia" + infoId + "_" + fileName + "." + ext;
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\", newName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        MediaSource src = new MediaSource()
                        {
                            MediaType = mType,
                            Source = newName
                        };
                        MediaSourceService.Add(src);
                        TimeTableInfoHasMediaSrc infoHasMedia = new TimeTableInfoHasMediaSrc()
                        {
                            TimeTableInformationId = infoId,
                            MediaSourceId = MediaSourceService.GetBySource(newName).MediaSourceId
                        };
                        TimeTableInfoMediaSrcService.Add(infoHasMedia);
                    }
                }
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                throw e;
            }          
        }
        [HttpPost]
        public IActionResult SaveLink(string source, int infoId)
        {
            try
            {
                MediaSource src = new MediaSource()
                {
                    MediaType = (int)MediaType.Video,
                    Source = source
                };
                MediaSourceService.Add(src);
                TimeTableInfoHasMediaSrc infoHasMedia = new TimeTableInfoHasMediaSrc()
                {
                    TimeTableInformationId = infoId,
                    MediaSourceId = MediaSourceService.GetBySource(source).MediaSourceId
                };
                TimeTableInfoMediaSrcService.Add(infoHasMedia);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        public IActionResult DeleteMedia(int infoId, int mediaId)
        {
            try
            {
                List<TimeTableInfoHasMediaSrc> mediaToDelete = new List<TimeTableInfoHasMediaSrc>();
                IEnumerable links = TimeTableInfoMediaSrcService.GetAllByTimeTableInfoId(infoId);
                foreach (TimeTableInfoHasMediaSrc link in links)
                {
                    if (link.MediaSourceId == mediaId)
                    {
                        mediaToDelete.Add(link);
                    }
                }
                for(int i = 0; i < mediaToDelete.Count; i++)
                {
                    TimeTableInfoMediaSrcService.Delete(mediaToDelete[i]);
                }
                return RedirectToAction("upload", new { id = infoId });
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}