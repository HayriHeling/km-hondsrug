using Eduria.Models;
using EduriaData.Models.AnalyticLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EduriaData.Models;

namespace Eduria.Services
{
    public class AnalyticDefaultService : AService<AnalyticData>
    {
        public AnalyticDefaultService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Add an DataHasDefault object to the database.
        /// </summary>
        /// <param name="analyticDefaultId">The specific AnalyticDefaultId</param>
        /// <param name="analyticDataId">The specific AnalyticDataId</param>
        public DataHasDefault AddDataHasDefault(int analyticDefaultId, int analyticDataId)
        {
            // Make and new instance of DataHasDefault
            DataHasDefault dataHasDefault = new DataHasDefault
            {
                AnalyticDataId = analyticDataId,
                AnalyticDefaultId = analyticDefaultId
            };

            // Add the new instance to the database
            Context.DataHasDefaults.Add(dataHasDefault);
            Context.SaveChanges();

            return dataHasDefault;
        }

        /// <summary>
        /// Retrieve all the static analytic names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AnalyticDefault> GetAllAnalyticDefault()
        {
            return Context.AnalyticDefaults;
        }

        /// <summary>
        /// Retrieve all the AnalyticData objects.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AnalyticData> GetAll()
        {
            return Context.AnalyticDatas;
        }

        /// <summary>
        /// Retrieve an specific AnalyticData object from the database.
        /// </summary>
        /// <param name="id">The specific id from the AnalyticData object.</param>
        /// <returns>The AnalyticData object.</returns>
        public override AnalyticData GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void AddAnalyticDataPerUser(IEnumerable<User> users, int periodId)
        {
            if (users.Count() != 0)
            {
                foreach(var item in users.ToList())
                {
                    AnalyticData analyticData = new AnalyticData
                    {
                        UserId = item.UserId,
                        PeriodId = periodId,
                        ExamCode = "1"
                    };

                    Context.Add(analyticData);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gets the specific Period by the periodNum and startYear.
        /// </summary>
        /// <param name="periodNum">The number of the period.</param>
        /// <param name="startYear">The startyear of the period.</param>
        /// <returns>Period object.</returns>
        public int GetByPeriodIdByPeriodNumAndStartYear(int periodNum, int startYear)
        {
            var query = from p in Context.Periods
                        where p.PeriodNum == periodNum && p.SchoolYearStart == startYear
                        select new Period
                        {
                            PeriodId = p.PeriodId
                        };
            return query.First().PeriodId;


            //return Context.Periods.Where(pn => pn.PeriodNum == periodNum).Where(sy => sy.SchoolYearStart == startYear).First().PeriodId;
        }

        /// <summary>
        /// Retrieve all DataHasDefault objects from the database.
        /// </summary>
        /// <returns>An IEnumberable with al the DataHasDefault objects.</returns>
        public IEnumerable<DataHasDefault> GetAllDataHasDefaults()
        {
            return Context.DataHasDefaults;
        }

        /// <summary>
        /// Retrieve all AnalyticHasDefault object from an specific user.
        /// </summary>
        /// <param name="id">The AnalyticData id.</param>
        /// <returns>An IEnumberable with all the AnlyticDataHasDefaultModel objects.</returns>
        public IEnumerable<AnalyticHasDefaultModel> GetAllDataByAnalyticDataId(int id)
        {
            var query = from dhd in Context.DataHasDefaults
                        join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                        // Use an join, but the outcome can be null
                        join dds in Context.DefaultDataScores on dhd.DataHasDefaultId equals dds.DataHasDefaultId into a
                        from dds in a.DefaultIfEmpty()
                            // Use an join, but the outcome can be null
                        join ddi in Context.DefaultDataInputs on dhd.DataHasDefaultId equals ddi.DataHasDefaultId into b
                        from ddi in b.DefaultIfEmpty()

                        where dhd.AnalyticDataId == id 

                        select new AnalyticHasDefaultModel
                        {
                            DataHasDefaultId = dhd.DataHasDefaultId,
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                            CategoryId = ad.AnalyticCategory,
                            Score = dds.Score,
                            Input = ddi.Text,
                            Option = ad.AnalyticDefaultOption,
                        };

            return query.ToList();
        }

        /// <summary>
        /// Retrieve all AnalyticHasDefault object from an specific user.
        /// </summary>
        /// <param name="id">The AnalyticData id.</param>
        /// <param name="userId">The UserID of the user that is currently logged in.</param>
        /// <returns>An IEnumberable with all the AnlyticDataHasDefaultModel objects.</returns>
        public IEnumerable<AnalyticHasDefaultModel> GetAllDataByAnalyticDataId(int id, int userId)
        {
            var query = from dhd in Context.DataHasDefaults
                        join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                        // Use an join, but the outcome can be null
                        join dds in Context.DefaultDataScores on dhd.DataHasDefaultId equals dds.DataHasDefaultId into a
                        from dds in a.DefaultIfEmpty()
                            // Use an join, but the outcome can be null
                        join ddi in Context.DefaultDataInputs on dhd.DataHasDefaultId equals ddi.DataHasDefaultId into b
                        from ddi in b.DefaultIfEmpty()
                        join add in Context.AnalyticDatas on dhd.AnalyticDataId equals add.AnalyticDataId
                        where dhd.AnalyticDataId == id && add.UserId == userId

                        select new AnalyticHasDefaultModel
                        {
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                            CategoryId = ad.AnalyticCategory,
                            Score = dds.Score,
                            Input = ddi.Text,
                            Option = ad.AnalyticDefaultOption
                        };

            return query.ToList();
        }

        /// <summary>
        /// Returns an tuple (to the view) with all the ojects that are needed at the view to represent the data.
        /// </summary>
        /// <param name="id">The AnalyticData id.</param>
        /// <param name="category">The AnalyticDefaults category.</param>
        /// <returns>An tuple with all the data needed to feed at the view.</returns>
        public Tuple<IEnumerable<AnalyticHasDefaultModel>, IEnumerable<AnalyticDefaultModel>> GetCombinedAnalyticDefaultAndData(int id, int category)
        {
            IEnumerable<AnalyticHasDefaultModel> analyticHasDefaultModels = GetAllDefaultsByAnalyticDataIdAndCategoryName(id, category);
            IEnumerable<AnalyticDefaultModel> analyticDefaultModels = GetAllAnalyticDefaultByCategoryId(category);

            var tuple = Tuple.Create(analyticHasDefaultModels, analyticDefaultModels);

            return tuple;
        }

        /// <summary>
        /// Retrieve all AnalyticDefaults from an specfic user.
        /// </summary>
        /// <param name="id">The AnalyticData id.</param>
        /// <param name="category">The AnalyticDefaults category.</param>
        /// <returns>An IEnumerable with all the AnalyticDefaultsModels.</returns>
        public IEnumerable<AnalyticHasDefaultModel> GetAllDefaultsByAnalyticDataIdAndCategoryName(int id, int category)
        {
            return GetAllDataByAnalyticDataId(id).Where(x => x.CategoryId == category);
        }

        /// <summary>
        /// Retrieve all AnalyticDefaults from the database by an specific category.
        /// </summary>
        /// <param name="category">The specific category.</param>
        /// <returns>An IEnumerable with all the AnalyticDefaultModels.</returns>
        public IEnumerable<AnalyticDefaultModel> GetAllAnalyticDefaultByCategoryId(int category)
        {
            var query = from ad in Context.AnalyticDefaults
                        where ad.AnalyticCategory == category
                        select new AnalyticDefaultModel
                        {
                            AnalyticDefaultId = ad.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                            AnalyticDefaultOption = ad.AnalyticDefaultOption,
                            IsChecked = false,
                            Text = ""
                        };

            return query.ToList();
        }

        /// <summary>
        /// Retrieve an specific AnalyticData per user, period and year.
        /// </summary>
        /// <param name="userId">The User id.</param>
        /// <param name="period">The specific period.</param>
        /// <param name="year">The specific year.</param>
        /// <returns>An AnalyticData with all the requirements met.</returns>
        public AnalyticData GetAnalyticDataByUserIdAndPeriodAndYear(int userId, int periodNum, int startYear)
        {
            var query = from ad in Context.AnalyticDatas
                        join p in Context.Periods on ad.PeriodId equals p.PeriodNum
                        where ad.UserId == userId && p.PeriodNum == periodNum && p.SchoolYearStart == startYear
                        select new AnalyticData
                        {
                            AnalyticDataId = ad.AnalyticDataId
                        };

            return query.First();
        }

        /// <summary>
        /// Add all defined methods to the Analytic.
        /// </summary>
        /// <param name="methodParam">The array of integers that stands for the AnalyticDefault Id.</param>
        /// <param name="analyticId">The Analytic Id from a specific user.</param>
        /// <param name="ownMethod">The own method that the user specified.</param>
        public void AddToAnalytic(int[] methodParam, int analyticId, string ownMethod)
        {
            if (methodParam.Length != 0)
            {
                foreach (var id in methodParam)
                {
                    //When a own method is filled in, use this Add method.
                    if (GetDefaultOptionByAnalyticDefaultId(id) == (int)DefaultOption.InputScore && !String.IsNullOrEmpty(ownMethod))
                    {
                        AddInputToAnalyticDefault(id, analyticId, ownMethod);
                    }
                    else
                    {
                        AddDataHasDefault(id, analyticId);
                    }
                }
            }
        }

        /// <summary>
        /// Add an DefaultDataInput object to the database.
        /// </summary>
        /// <param name="analyticDefaultId">The specific AnalyticDefaultId.</param>
        /// <param name="analyticDataId">The specific AnalyticDataId.</param>
        /// <param name="text">The text that the user filled in on the form.</param>
        public void AddInputToAnalyticDefault(int analyticDefaultId, int analyticDataId, string text)
        {
            if (analyticDefaultId != 0 && analyticDataId != 0 && !String.IsNullOrEmpty(text))
            {
                DataHasDefault dataHasDefault = GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(analyticDefaultId, analyticDataId);

                //When the DataHasDefault is null, adds it to the database.
                if (dataHasDefault == null)
                {
                    dataHasDefault = AddDataHasDefault(analyticDefaultId, analyticDataId);
                }

                //Make an new instance of DefaultDataInput object and fill it.
                DefaultDataInput defaultDataInput = new DefaultDataInput
                {
                    DataHasDefaultId = dataHasDefault.DataHasDefaultId,
                    Text = text
                };

                //Add the DefaultDataInput object to the database.
                Context.DefaultDataInputs.Add(defaultDataInput);
                Context.SaveChanges();
            }
        }

        public DefaultDataScore GetDefaultDataScoreByAnalyticHasDefaultId(int analyticHasDefaultId)
        {
            var query = from dds in Context.DefaultDataScores
                        where dds.DataHasDefaultId == analyticHasDefaultId
                        select new DefaultDataScore
                        {
                            DataHasDefaultId = dds.DataHasDefaultId,
                            DefaultDateScoreId = dds.DefaultDateScoreId,
                            Score = dds.Score
                        };


            return query.FirstOrDefault();
        }

        /// <summary>
        /// Add an DefaultDataScore object to the database.
        /// </summary>
        /// <param name="analyticDefaultId">The specific AnalyticDefaultId.</param>
        /// <param name="analyticDataId">The specific AnalyticDataId.</param>
        /// <param name="score">The score that the teacher filled in.</param>
        public void AddScoreToAnalyticDefault(int analyticDefaultId, int analyticDataId, int score)
        {
            if (analyticDefaultId != 0 && analyticDataId != 0)
            {
                DataHasDefault dataHasDefault = GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(analyticDefaultId, analyticDataId);

                //When the DataHasDefault is null, adds it to the database.
                if (dataHasDefault == null)
                {
                    dataHasDefault = AddDataHasDefault(analyticDefaultId, analyticDataId);
                }

                if (GetDefaultDataScoreByAnalyticHasDefaultId(dataHasDefault.DataHasDefaultId) == null)
                {
                    //Make an new instance of DefaultDataScore object and fill it.
                    DefaultDataScore defaultDataScore = new DefaultDataScore
                    {
                        DataHasDefaultId = dataHasDefault.DataHasDefaultId,
                        Score = score
                    };

                    //Add the DefaultDataScore object to the database.
                    Context.DefaultDataScores.Add(defaultDataScore);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Retrieve and specific DataHasDefault object from the database.
        /// </summary>
        /// <param name="analyticDefaultId">The specific AnalyticDefaultId.</param>
        /// <param name="analyticDataId">The specific AnalyticDataId.</param>
        /// <returns>An specifc DataHasDefault object the meets the requirements.</returns>
        public DataHasDefault GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(int analyticDefaultId, int analyticDataId)
        {
            var query = from dhd in Context.DataHasDefaults
                        where dhd.AnalyticDefaultId == analyticDefaultId && dhd.AnalyticDataId == analyticDataId
                        select new DataHasDefault
                        {
                            DataHasDefaultId = dhd.DataHasDefaultId,
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId
                        };


            return query.FirstOrDefault();
        }

        /// <summary>
        /// Retrieve the default option by AnalyticDefault id.
        /// </summary>
        /// <param name="id">The specific id from the AnalyticDefault.</param>
        /// <returns>The default option.</returns>
        public int GetDefaultOptionByAnalyticDefaultId(int id)
        {
            var defaultOption = Context.AnalyticDefaults.Where(x => x.AnalyticDefaultId == id).First();

            if (defaultOption != null)
            {
                return defaultOption.AnalyticDefaultOption;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// When a user goes to the Subject page for the first time, add the DefaultData.
        /// </summary>
        /// <param name="analyticId"></param>
        public bool AddSubjectToHasDefaults(int analyticId)
        {
            var query = from dhd in Context.DataHasDefaults
                        join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                        where dhd.AnalyticDataId == analyticId && ad.AnalyticCategory == (int)AnalyticCategory.Reflectie
                        select new DataHasDefault
                        {
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId,
                            DataHasDefaultId = dhd.DataHasDefaultId
                        };

            if (query.ToList().Count() == 0)
            {
                var addQuery = from ad in Context.AnalyticDefaults
                               where ad.AnalyticCategory == (int)AnalyticCategory.Reflectie
                               select new AnalyticDefault
                               {
                                   AnalyticCategory = ad.AnalyticCategory,
                                   AnalyticDefaultId = ad.AnalyticDefaultId,
                                   AnalyticDefaultName = ad.AnalyticDefaultName,
                                   AnalyticDefaultOption = ad.AnalyticDefaultOption
                               };

                // Loop through each item and add it to DataHasDefault table.
                foreach (var item in addQuery.ToList())
                {
                    AddDataHasDefault(item.AnalyticDefaultId, analyticId);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get all the input from the form and add all DefaultDataScore
        /// </summary>
        /// <param name="form">The IFormCollection the is passed into the controller back to this service method.</param>
        public void AddDefaultDataScore(IFormCollection form)
        {
            // Make an List for all the IFormCollection data.
            var listOfDefaultDataScore = form.ToList();
            // Remove unnecessary last data token.
            listOfDefaultDataScore.RemoveAt(listOfDefaultDataScore.Count - 1);

            foreach (var item in listOfDefaultDataScore)
            {
                int hasDefaultDataId = int.Parse(item.Key);

                if (GetDefaultDataScoreByAnalyticHasDefaultId(hasDefaultDataId) == null)
                {
                    DefaultDataScore defaultDataScore = new DefaultDataScore
                    {
                        DataHasDefaultId = hasDefaultDataId,
                        Score = int.Parse(item.Value)
                    };

                    Context.DefaultDataScores.Add(defaultDataScore);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gets a combined AnalyticDefault and AnalyticHasDefault model to send to a view.
        /// </summary>
        /// <param name="id">The AnalyticData id to get the models from.</param>
        /// <param name="category">The specific category to get a combined model from.</param>
        /// <returns>A combined AnalyticDefault and AnalyticHasDefault model.</returns>
        public AnalyticDefaultAndHasDefaultModel GetAnalyticDefaultAndHasDefaultModel(int id, int category)
        {
            AnalyticDefaultAndHasDefaultModel analyticDefaultAndHasDefaultModel = new AnalyticDefaultAndHasDefaultModel
            {
                AnalyticData = GetAnalyticDataById(id),
                AnalyticHasDefaultModels = GetAllDefaultsByAnalyticDataIdAndCategoryName(id, category).ToList(),
                AnalyticDefaultModels = GetAllAnalyticDefaultByCategoryId(category).ToList()
            };

            return analyticDefaultAndHasDefaultModel;
        }

        /// <summary>
        /// Return the AnalyticDataId by the UserId, Year and Period.
        /// </summary>
        /// <param name="form">The IFormCollection that contains the value of year and period.</param>
        /// <param name="userId">The UserId of the user that is currently logged in.</param>
        /// <returns>The AnalyticDataId.</returns>
        public int GetAnalyticDataIdByYearAndPeriodAndUserId(IFormCollection form, int userId)
        {
            var query = from ad in Context.AnalyticDatas
                        join p in Context.Periods on ad.PeriodId equals p.PeriodId
                        where p.SchoolYearStart == int.Parse(form["year"]) && p.PeriodNum== int.Parse(form["period"]) && ad.UserId == userId
                        select new AnalyticData
                        {
                            AnalyticDataId = ad.AnalyticDataId
                        };

            return query.First().AnalyticDataId;
        }

        public IEnumerable<AnalyticHasDefaultModel> GetAnalyticDataByYearAndPeriodAndUserId(IFormCollection form, int userId)
        {
            var query = from dhd in Context.DataHasDefaults
                        join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                        // Use an join, but the outcome can be null
                        join dds in Context.DefaultDataScores on dhd.DataHasDefaultId equals dds.DataHasDefaultId into a
                        from dds in a.DefaultIfEmpty()
                            // Use an join, but the outcome can be null
                        join ddi in Context.DefaultDataInputs on dhd.DataHasDefaultId equals ddi.DataHasDefaultId into b
                        from ddi in b.DefaultIfEmpty()
                        join add in Context.AnalyticDatas on dhd.AnalyticDataId equals add.AnalyticDataId
                        join p in Context.Periods on add.PeriodId equals p.PeriodId
                        where p.SchoolYearStart== int.Parse(form["year"]) && p.PeriodNum == int.Parse(form["period"]) && add.UserId == userId

                        select new AnalyticHasDefaultModel
                        {
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                            CategoryId = ad.AnalyticCategory,
                            Score = dds.Score,
                            Input = ddi.Text,
                            Option = ad.AnalyticDefaultOption
                        };

            return query.ToList();
        }

        public void AddPeriod(PeriodModel periodModel)
        {
            if (periodModel != null)
            {
                Period period = new Period
                {
                    PeriodNum = periodModel.PeriodNum,
                    PeriodStart = periodModel.PeriodStart.Date,
                    PeriodEnd = periodModel.PeriodEnd.Date,
                    SchoolYearStart = periodModel.SchoolYearStart,
                    SchoolYearEnd = periodModel.SchoolYearEnd
                };

                Context.Periods.Add(period);
                Context.SaveChanges();
            }
        }

        public int GetAnalyticDataIdByUserIdAndPeriodAndYear(int userId, int period, int year)
        {
            var query = from ad in Context.AnalyticDatas
                        join p in Context.Periods on ad.PeriodId equals p.PeriodId
                        where ad.UserId == userId && p.PeriodNum == period && p.SchoolYearStart == year
                        select new AnalyticData
                        {
                            AnalyticDataId = ad.AnalyticDataId
                        };

            if (query.Any())
            {
                return query.First().AnalyticDataId;
            }
            else
            {
                return -1;
            }
        }

        public IEnumerable<AnalyticDataModel> GetAllAnalyticDatasByUserId(int userId)
        {
            var query = from ad in Context.AnalyticDatas
                        join p in Context.Periods on ad.PeriodId equals p.PeriodId
                        where ad.UserId == userId
                        orderby p.SchoolYearStart
                        orderby p.PeriodNum
                        select new AnalyticDataModel
                        {
                            AnalyticDataId = ad.AnalyticDataId,
                            ExamCode = ad.ExamCode,
                            PeriodNum = p.PeriodNum,
                            PeriodStart = p.PeriodStart,
                            PeriodEnd = p.PeriodEnd,
                            SchoolYearStart = p.SchoolYearStart,
                            SchoolYearEnd = p.SchoolYearEnd
                        };

            return query.ToList();
        }

        public AnalyticDataAndHasDefaultModel GetAnalyticDataIdAndHasDefaults(int analyticDataId)
        {
            AnalyticDataModel analyticData = GetAnalyticDataById(analyticDataId);
            AnalyticDataAndHasDefaultModel analyticDataAndHasDefaultModel = new AnalyticDataAndHasDefaultModel
            {
                AnalyticData = GetAnalyticDataById(analyticDataId),
                AnalyticHasDefaultModels = GetAllDataByAnalyticDataId(analyticDataId).ToList()
            };

            return analyticDataAndHasDefaultModel;
        }

        public AnalyticDataModel GetAnalyticDataById(int analyticDataId)
        {
            IQueryable<AnalyticDataModel> query = from ad in Context.AnalyticDatas
                                                  join p in Context.Periods on ad.PeriodId equals p.PeriodId
                                                  where ad.AnalyticDataId == analyticDataId
                                                  select new AnalyticDataModel
                                                  {
                                                      AnalyticDataId = ad.AnalyticDataId,
                                                      ExamCode = ad.ExamCode,
                                                      PeriodNum = p.PeriodNum,
                                                      PeriodStart = p.PeriodStart,
                                                      PeriodEnd = p.PeriodEnd,
                                                      SchoolYearStart = p.SchoolYearStart,
                                                      SchoolYearEnd = p.SchoolYearEnd
                                                  };

            return query.First();
        }
    }
}
