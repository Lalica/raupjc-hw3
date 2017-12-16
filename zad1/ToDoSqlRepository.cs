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

        public TodoItem Get(Guid todoId, Guid userId)
        {
            using (_context)
            {
                if (!_context.ToDoItems.Any(i => i.Id == todoId && i.UserId == userId))
                {
                    throw new TodoAccessDeniedException("User: " + userId + " doesn't have permission to view item: " + todoId);
                }
                if(_context.ToDoItems.Any(i => i.UserId == userId)) return _context.ToDoItems.First(i => i.UserId == userId);
                return null;
            }
        }

        public void Add(TodoItem todoItem)
        {
            using (_context)
            {
                if (todoItem == null)
                {
                    return;
                }
                if (_context.ToDoItems.Any(i => i.Id == todoItem.Id))
                {
                    throw new DuplicateTodoItemException("Duplicate id: " + todoItem.Id);
                }
                _context.ToDoItems.Add(todoItem);
                _context.SaveChangesAsync();
            }
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            using (_context)
            {
                TodoItem itemToRemove = Get(todoId, userId);
                if (itemToRemove == null)
                {
                    return false;
                }

                _context.ToDoItems.Remove(itemToRemove);
                _context.SaveChangesAsync();
                return true;
            }
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            using (_context)
            {
                if (!_context.ToDoItems.Any(i => i.Id == todoItem.Id && i.UserId == userId))
                {
                    throw new TodoAccessDeniedException("User: " + userId + " doesn't have permission to view item: " + todoItem.Id);
                }
                _context.ToDoItems.AddOrUpdate(todoItem);
                _context.SaveChangesAsync();
            }
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

        public List<TodoItem> GetAll(Guid userId)
        {
            using (_context)
            {
               return _context.ToDoItems.Where(i => i.UserId == userId).OrderByDescending( m => m.DateCreated).ToList();
            }
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(i => !i.IsCompleted && i.UserId == userId).ToList();
            }
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(i => i.IsCompleted && i.UserId == userId).ToList();
            }
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            using (_context)
            {
                return _context.ToDoItems.Where(filterFunction).Where(i => i.UserId == userId).ToList();
            }
        }
    }
}
