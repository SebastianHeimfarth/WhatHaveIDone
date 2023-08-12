using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.Persistence
{
    public class TaskDbContext : DbContext, ITaskDbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<TaskModel> Tasks { get; set; }

        public virtual DbSet<TaskProperty> TaskProperties { get; set; }

        public virtual DbSet<TaskType> TaskTypes { get; set; }

        public virtual DbSet<TaskPropertyType> TaskPropertyTypes { get; set; }

        public Task<bool> TaskTypeExists(int id)
        {
            return TaskTypes.AnyAsync(x => x.Id == id);
        }

        public async Task AddTaskType(TaskType taskType)
        {
            await TaskTypes.AddAsync(taskType);
            await SaveChangesAsync();
        }

        public async Task<TaskModel> CreateTaskAsync(string taskName, TaskType type, string comment, DateTime begin)
        {
            var task = new TaskModel { Name = taskName, Comment = comment, Begin = begin, TaskType = type };

            var commentProperty = task.Properties.FirstOrDefault(x => x.Name == "Comment");
            if (commentProperty != null && comment != null)
            {
                commentProperty.Value = comment;
            }

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

        public Task<IReadOnlyList<TaskType>> GetAllTypes()
        {
            return TaskTypes.ToListAsync().ContinueWith(x => (IReadOnlyList<TaskType>)x.Result);
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
                        Include(x => x.TaskType).
                        Include(x => x.Properties).
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
                entity.HasMany(x => x.Properties).WithOne().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.TaskType).WithMany().OnDelete(DeleteBehavior.NoAction);

                entity.Navigation(x => x.Properties).AutoInclude();
                entity.Navigation(x => x.TaskType).AutoInclude();
            });

            var colorToIntegerConverter = new ColorToIntegerConverter();

            modelBuilder.Entity<TaskType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Color)
                    .HasConversion(colorToIntegerConverter);


                entity.HasMany(x => x.DefaultProperties).WithOne().OnDelete(DeleteBehavior.Cascade);

                entity.Navigation(x => x.DefaultProperties).AutoInclude();
            });

            modelBuilder.Entity<TaskProperty>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Value);
                entity.HasOne(x => x.TaskPropertyType).WithMany().OnDelete(DeleteBehavior.NoAction);
                entity.Navigation(x => x.TaskPropertyType).AutoInclude();
            });
        }

        Task ITaskDbContext.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}