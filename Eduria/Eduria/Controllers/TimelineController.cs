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
        private UserService UserService;

        private static Random random = new Random();

        public TimelineController(TimeTableService timeTableService,
            TimeTableInformationService timeTableInformationService,
            TimeTableInfoMediaSrcService timeTableInfoMediaSrcService,
            MediaSourceService mediaSourceService, UserService userService)
        {
            TimeTableService = timeTableService;
            TimeTableInformationService = timeTableInformationService;
            TimeTableInfoMediaSrcService = timeTableInfoMediaSrcService;
            MediaSourceService = mediaSourceService;
            UserService = userService;
        }
        /// <summary>
        /// Returns the timeline
        /// </summary>
        /// <param name="id"> the id of the user of the timeline. default is -1</param>
        /// <returns></returns>
        public IActionResult Index(int id=-1)
        {
            if(id != -1 && UserService.GetById(id).UserType == (int)UserRoles.Admin)
            {
                return RedirectToAction("Index");
            }
            ViewBag.id = id;
            if(id == -1)
            {
                id = UserService.GetAllUsersByUserType((int)UserRoles.Admin).First().UserId;
            }
            ViewBag.user = UserService.GetById(id);            
            ViewBag.loggedUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            ViewBag.loggedUserType = UserService.GetById(ViewBag.loggedUser).UserType;
            return View(CreateTimeLineModel(id));
        }
        /// <summary>
        /// creates timeline model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TimelineModel CreateTimeLineModel(int id)
        {
            return new TimelineModel
            {
                Name = "Tijdlijn 1",
                TimeblockModels = CreateTimeblockModels(id)
            };
        }
        /// <summary>
        /// creates timeblock model
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                TimeBlockInformationId = timeTableInformation.TimeTableInformationId,
                Name = timeTableInformation.Name,
                Description = timeTableInformation.Description,
                BeforeChrist = (ChristNotation)timeTableInformation.BeforeChrist,
                Year = timeTableInformation.Year,
                MediaSourceModels = CreateMediaSourceModels(timeTableInformation.TimeTableInformationId),
                TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(timeTableInformation.TimeTableId))               
            };
        }
        /// <summary>
        /// Converts Timeblockinformationmodels to timetableinformation objects to use with db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TimeTableInformation ConvertToTimeTableInformation(TimeBlockInformationModel model)
        {
            return new TimeTableInformation()
            {
                TimeTableId = model.TimeTable.TimeTableId,                
                Name = model.Name,
                Description = model.Description,
                BeforeChrist = (int)model.BeforeChrist,
                Year = model.Year
            };
        }
        /// <summary>
        /// Returns the view where the user can add information to a timeline
        /// </summary>
        /// <param name="state"> The state of where the information needs to be saved. 0 is default timeline while 1 is personal timeline.</param>
        /// <returns></returns>
        public IActionResult CreateInformation(int state = 0)
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
            ViewBag.state = state;
            return View();
        }
        /// <summary>
        /// Saves the newly created information to the timeline
        /// </summary>
        /// <param name="info"> info model filled in by form</param>
        /// <param name="state"> to which timeline it needs to be saved. 0 is default timeline while 1 is personal timeline</param>
        /// <param name="timeTableId">the timetableId given because this can't be databinded</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateInformation(TimeBlockInformationModel info, int state ,int timeTableId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (userId == (int)UserRoles.Student)
                {
                    return RedirectToAction("Index");
                }

                info.TimeTable = ConvertToTimeTableModel(TimeTableService.GetById(timeTableId));
                TimeTableInformation dbInfo = ConvertToTimeTableInformation(info);
                
                if (state > -1)
                {
                    dbInfo.UserId = userId;
                }
                else
                {
                    dbInfo.UserId = UserService.GetAllUsersByUserType((int)UserRoles.Admin).First().UserId;
                }
                
                TimeTableInformationService.Add(dbInfo);
                return RedirectToAction("Index", new { id = state });
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Returns the view where the user van edit existing information.
        /// </summary>
        /// <param name="id">id of the informationmodel</param>
        /// <returns></returns>
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
        /// <summary>
        /// Saves the edited information to the database.
        /// </summary>
        /// <param name="info">infomodel filled in by the form</param>
        /// <param name="timeTableId">the timetableId given because this can't be databinded</param>
        /// <returns></returns>
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
                return RedirectToAction("index", new { id = toChange.UserId });
            }
            catch(Exception e)
            {
                throw e;
            }           
        }
        /// <summary>
        /// Deletes the information of given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteInformation(int id)
        {
            try
            {
                int userId = TimeTableInformationService.GetById(id).UserId;
                TimeTableInformationService.Delete(TimeTableInformationService.GetById(id));
                return RedirectToAction("Index", new { id = userId });
            }
            catch(Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// shows the view where the user can upload files to information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Uploads media to the database.
        /// </summary>
        /// <param name="files">list of files being uploaded</param>
        /// <param name="mType">mediatype of given files</param>
        /// <param name="infoId"> id of the information the media is for</param>
        /// <returns></returns>
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
                int userId = TimeTableInformationService.GetById(infoId).UserId;
                return RedirectToAction("Index", new { id = userId});
            }
            catch(Exception e)
            {
                throw e;
            }          
        }
        /// <summary>
        /// Saves the youtube link to the database
        /// </summary>
        /// <param name="source">the given youtube link</param>
        /// <param name="infoId">the id of the information model the link is for</param>
        /// <returns></returns>
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
                int userId = TimeTableInformationService.GetById(infoId).UserId;
                return RedirectToAction("Index", new { id = userId });
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Deletes reference to the media of given id
        /// </summary>
        /// <param name="infoId">the id of the information the media is from</param>
        /// <param name="mediaId">the id of the mediasource</param>
        /// <returns></returns>
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
                int userId = TimeTableInformationService.GetById(infoId).UserId;
                return RedirectToAction("upload", new { id = userId });
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// generates a random string 
        /// </summary>
        /// <param name="length">the length of the string</param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}