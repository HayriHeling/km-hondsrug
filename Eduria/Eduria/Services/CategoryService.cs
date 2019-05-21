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
        public override IEnumerable<Category> GetAll()
        {
            return Context.Categories;
        }

        public override Category GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }
    }
}
