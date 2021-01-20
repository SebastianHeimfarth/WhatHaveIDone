using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;
using WhatHaveIDone.Core.Persistence;

namespace WhatHaveIDone.Persistence
{
    public class TaskDbContext : DbContext, ITaskDbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<TaskModel> Tasks { get; set; }

        public async Task<TaskModel> CreateTaskAsync(string taskName, string comment, DateTime begin)
        {
            var task = new TaskModel { Name = taskName, Comment = comment, Begin = begin };

            await Tasks.AddAsync(task);
            await SaveChangesAsync();
            return task;
        }

        public Task<TaskModel> GetTaskByIdAsync(Guid id)
        {
            return Tasks.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<IReadOnlyList<TaskModel>> GetTasksInIntervalAsync(DateTime start, DateTime end)
        {
            return Tasks.Where(
                x => x.Begin < end 
                     && 
                     (!x.End.HasValue || 
                        x.End.HasValue && x.End.Value > start)).
                ToListAsync().
                ContinueWith(x => (IReadOnlyList<TaskModel>)x.Result);
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

        Task ITaskDbContext.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
