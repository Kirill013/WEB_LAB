using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data.Entity;

namespace BookStore.Models.Repository
{
    public class EFDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}