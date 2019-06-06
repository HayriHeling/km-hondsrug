using Eduria.Models;
using EduriaData.Models.AnalyticLayer;
using System;
using System.Collections.Generic;
using System.Linq;

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
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                            CategoryId = ad.AnalyticCategory,
                            Score = dds.Score,
                            Input = ddi.Text
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
        public AnalyticData GetAnalyticDataByUserIdAndPeriodAndYear(int userId, int period, int year)
        {
            var query = from ad in Context.AnalyticDatas
                        where ad.UserId == userId && ad.Period == period && ad.Year == year
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
                    if (GetDefaultOptionByAnalyticDefaultId(id) == (int)DefaultOption.InputScore)
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
            if (analyticDefaultId != 0 && analyticDataId != 0)
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
    }
}
