using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFRepDemo.Models;

namespace EFRepDemo.Data
{
    internal class TodoContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\.;Initial Catalog=TodoDB;Integrated Security=True;Pooling=False");
        }
    }
}
