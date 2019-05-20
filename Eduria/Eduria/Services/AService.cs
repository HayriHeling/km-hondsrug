using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Services
{
    public abstract class AService<T>
    {
        private EduriaContext Context { get; set; }
        public AService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Add an item to the database.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            Context.Add(item);
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete an item from the database.
        /// </summary>
        /// <param name="item"></param>
        public void Delete(T item)
        {
            Context.Remove(item);
            Context.SaveChanges();
        }

        /// <summary>
        /// Update an item from the database.
        /// </summary>
        /// <param name="item"></param>
        public void Update(T item)
        {
            Context.Update(item);
            Context.SaveChanges();
        }

        /// <summary>
        /// Get all items from the database.
        /// </summary>
        /// <returns>List of items</returns>
        public abstract IEnumerable<T> GetAll();

        /// <summary>
        /// Get an specific item from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific item.</returns>
        public abstract T GetById(int id);
    }
}
