using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Services
{
    public class ConfigsService : AService<Config>
    {
        public ConfigsService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all exams from the database.
        /// </summary>
        /// <returns>A List with all exams.</returns>
        public override IEnumerable<Config> GetAll()
        {
            return Context.Configs;
        }
        /// <summary>
        /// Get an specific exam. 
        /// </summary>
        /// <param name="id">The id from the exam.</param>
        /// <returns>The specific category.</returns>
        public override Config GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.ConfigId == id);
        }

        public Config GetNewest()
        {
            return GetAll().Last();
        }
    }
}

