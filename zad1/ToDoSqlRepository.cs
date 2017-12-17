using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using zad1.from_last_homework;

namespace zad1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }



        public void Add(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                throw new ArgumentNullException();
            }
            if (_context.ToDoItems.Any(t => t.Text == todoItem.Text))
            {
                throw new DuplicateTodoItemException("Duplicate id: " + todoItem.Id);
            }
            _context.ToDoItems.Add(todoItem);
            _context.SaveChanges();
        }


        public TodoItem Get(Guid todoId, Guid userId)
        {
            List<TodoItem> list = _context.ToDoItems.Where(t => t.Id == todoId).Include(l => l.Labels).ToList();
            if (!list.Any())
            {
                return null;
            }
            if (list.All(t => t.UserId != userId))
            {
                throw new TodoAccessDeniedException("User " + userId + " is not owner of the TodoItem!");
            }
            return _context.ToDoItems.FirstOrDefault(t => t.Id == todoId);
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.ToDoItems.Where(t => !t.IsCompleted && t.UserId == userId).Include(l => l.Labels).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.ToDoItems.Where(t => t.UserId == userId).OrderByDescending(t => t.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.ToDoItems.Where(t => t.IsCompleted && t.UserId == userId).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.ToDoItems.Where(t => t.UserId == userId).Where(t => filterFunction(t)).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem oldItem = Get(todoId, userId);
            if (oldItem == null)
            {
                return false;
            }
            bool b = oldItem.MarkAsCompleted();
            Update(oldItem, userId);
            return b;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }
            _context.ToDoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem item = Get(todoItem.Id, userId);
            if (item != null)
            {
                _context.ToDoItems.Remove(item);
                _context.SaveChanges();

                item.DateCompleted = todoItem.DateCompleted;
                item.DateCreated = todoItem.DateCreated;
                item.DateDue = todoItem.DateDue;
                item.IsCompleted = todoItem.IsCompleted;
                item.Text = todoItem.Text;
                item.UserId = userId;
                item.Labels = todoItem.Labels;
            }
            _context.ToDoItems.Add(item);
            _context.SaveChanges();
        }

        public void AddLabel(TodoItem item, string text)
        {
            TodoItemLabel label;
            if (_context.TodoItemLabels.Any(l => l.Value.ToLower().Equals(text.ToLower())))
            {
                label = _context.TodoItemLabels.First(l => l.Value.ToLower().Equals(text.ToLower()));
            }
            else
            {
                label = new TodoItemLabel(text);
                _context.TodoItemLabels.Add(label);
            }
            if (!item.Labels.Any(l => l.Value.ToLower().Equals(text.ToLower())))
            {
                item.Labels.Add(label);
            }
            _context.SaveChanges();
        }

        public void AddError(string text)
        {
            ErrorLogs error = new ErrorLogs(text);
            _context.Errors.Add(error);
            _context.SaveChanges();
        }
    }
}
