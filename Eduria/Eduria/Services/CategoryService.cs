using EduriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Services
{
    public class CategoryService : AService<Category>
    {
        private EduriaContext Context { get; set; } 
        public CategoryService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all categories from the database.
        /// </summary>
        /// <returns>A List with all categories.</returns>
        public override IEnumerable<Category> GetAll()
        {
            return Context.Categories;
        }

        /// <summary>
        /// Get an specific category. 
        /// </summary>
        /// <param name="id">The id from the category.</param>
        /// <returns>The specific category.</returns>
        public override Category GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }
    }
}
