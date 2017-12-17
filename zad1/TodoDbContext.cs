using System.Data.Entity;
using zad1.from_last_homework;

namespace zad1
{
    public class TodoDbContext : DbContext
    {
        public IDbSet<TodoItem> ToDoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoItemLabels { get; set; }
        public IDbSet<ErrorLogs> Errors { get; set; }

        public TodoDbContext(string cnnstr) : base(cnnstr)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoItem>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItem>().Property(i => i.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCompleted).IsOptional();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateDue).IsOptional();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasMany(i => i.Labels).WithMany(l => l.LabelTodoItems);

            modelBuilder.Entity<TodoItemLabel>().HasKey(l => l.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(i => i.Value).IsRequired();
            modelBuilder.Entity<TodoItemLabel>().HasMany(l => l.LabelTodoItems).WithMany(i => i.Labels);

            modelBuilder.Entity<ErrorLogs>().HasKey(e => e.Id);
            modelBuilder.Entity<ErrorLogs>().Property(e => e.Text).IsRequired();
        }
    }
}
