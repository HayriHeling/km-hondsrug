using EduriaData.Models.AnalyticLayer;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Services
{
    public class AnalyticMethodService : AService<AnalyticMethod>
    {
        public AnalyticMethodService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all AnalyticMethods from the database.
        /// </summary>
        /// <returns>All the AnalyticMethod objects.</returns>
        public override IEnumerable<AnalyticMethod> GetAll()
        {
            return Context.AnalyticMethods;
        }

        /// <summary>
        /// Get an specific AnalyticMethod from the database by id.
        /// </summary>
        /// <param name="id">The id of the AnalyticMethod object.</param>
        /// <returns>An specific AnlyticMethod object.</returns>
        public override AnalyticMethod GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.AnalyticMethodId == id);
        }

        /// <summary>
        /// Return all DataHasMethod objects.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataHasMethod> GetAllData()
        {
            return Context.DataHasMethods;
        }
    }
}
