using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            using (_context)
            {
                if (todoItem == null)
                {
                    throw new ArgumentNullException();
                }
                if (_context.ToDoItems.Any(t => t.Id == todoItem.Id))
                {
                    throw new DuplicateTodoItemException("Duplicate id: " + todoItem.Id);
                }
                _context.ToDoItems.Add(todoItem);
                _context.SaveChanges();
            }
        }


        public TodoItem Get(Guid todoId, Guid userId)
        {
            using (_context)
            {
                return InternalGet(todoId, userId);
            }
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(t => !t.IsCompleted && t.UserId == userId).ToList();

            }
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(t => t.UserId == userId).OrderByDescending(t => t.DateCreated).ToList();

            }
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(t => t.IsCompleted && t.UserId == userId).ToList();

            }
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(t => t.UserId == userId).Where(t => filterFunction(t)).ToList();

            }
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            using (_context)
            {
                // throws a TodoAccesDeniedException
                TodoItem item = InternalGet(todoId, userId);

                if (item == null)
                {
                    return false;
                }
                _context.ToDoItems.Remove(item);
                _context.SaveChanges();

                item.IsCompleted = true;
                item.DateCompleted = DateTime.Now;
                _context.ToDoItems.Add(item);
                _context.SaveChanges();
                return true;
            }
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            using (_context)
            {
                // throws a TodoAccesDeniedException
                TodoItem item = InternalGet(todoId, userId);
                if (item == null)
                {
                    return false;
                }
                _context.ToDoItems.Remove(item);
                _context.SaveChanges();
                return true;
            }
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            using (_context)
            {
                // throws a TodoAccesDeniedException
                TodoItem item = InternalGet(todoItem.Id, userId);
                if (item != null)
                {
                    _context.ToDoItems.Remove(item);
                    _context.SaveChanges();

                    item.DateCompleted = todoItem.DateCompleted;
                    item.DateCreated = todoItem.DateCreated;
                    item.IsCompleted = todoItem.IsCompleted;
                    item.Text = todoItem.Text;
                    item.UserId = userId;
                }
                _context.ToDoItems.Add(item);
                _context.SaveChanges();
            }
        }

        private TodoItem InternalGet(Guid todoId, Guid userId)
        {
            List<TodoItem> list = _context.ToDoItems.Where(t => t.Id == todoId).ToList();
            if (list == null || list.Count() == 0)
            {
                return null;
            }
            if (list.Select(t => t.UserId == userId).Count() == 0)
            {
                throw new TodoAccessDeniedException("User " + userId + " is not owner of the TodoItem!");
            }
            return _context.ToDoItems.FirstOrDefault(t => t.Id == todoId);
        }
    }
}
