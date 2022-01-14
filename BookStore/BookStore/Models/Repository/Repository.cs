using System;
using System.Linq;
using System.Web;


using System.Collections.Generic;

namespace BookStore.Models.Repository
{
    public class Repository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get { return context.Books; }
        }
    }
}