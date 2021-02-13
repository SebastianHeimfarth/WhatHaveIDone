using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public virtual DbSet<TaskCategory> TaskCategories { get; set; }

        public async Task<TaskModel> CreateTaskAsync(string taskName, TaskCategory category, string comment, DateTime begin)
        {
            var task = new TaskModel { Name = taskName, Comment = comment, Begin = begin, Category = category };

            await Tasks.AddAsync(task);
            await SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskById(Guid id)
        {
            var task = await GetTaskByIdAsync(id);
            if (task != null)
            {
                Tasks.Remove(task);
                await SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Task<IReadOnlyList<TaskCategory>> GetAllTaskCategories()
        {
            return TaskCategories.ToListAsync().ContinueWith(x => (IReadOnlyList<TaskCategory>)x.Result);
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
                entity.Property(x => x.Begin).HasConversion(new DateTimeToUtcDateTimeConverter());
                entity.Property(x => x.End).HasConversion(new DateTimeToUtcDateTimeConverter());
            });

            var colorToIntegerConverter = new ColorToIntegerConverter();

            modelBuilder.Entity<TaskCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Color)
                    .HasConversion(colorToIntegerConverter);

                entity.HasData(
                    new TaskCategory { Name = "Work", Color = Color.Beige, Id = Guid.Parse("31F59466-711A-46FE-B3F9-D6DB633440B1") },
                    new TaskCategory { Name = "Meeting", Color = Color.FromArgb(19, 173, 158), Id = Guid.Parse("44435569-C463-40AF-8F78-34CDBE035D8D") },
                    new TaskCategory { Name = "Pause", Color = Color.FromArgb(55, 173, 19), Id = Guid.Parse("D57D7417-C7F2-4872-A0D6-1D68C9BDC13A") }
                );
            });
        }

        Task ITaskDbContext.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}