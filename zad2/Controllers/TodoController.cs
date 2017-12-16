using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using zad2.Models;
using zad2.Models.TodoViewModels;

namespace zad2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> activeTodoItems = _repository.GetCompleted(new Guid(currentUser.Id));

            IndexViewModel model = new IndexViewModel();
            foreach (TodoItem item in activeTodoItems)
            {
                model.Items.Add(new TodoViewModel(item.Id, item.Text, item.DateCreated, item.DateCompleted, item.IsCompleted));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo(AddTodoViewModel item)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                TodoItem newItem = new TodoItem(item.Text, new Guid(currentUser.Id), item.DateDue);
                _repository.Add(newItem);
                return RedirectToAction("Index");
            }
            return View("Add", item);
        }

        public ViewResult Add()
        {
            return View();
        }

        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(id, new Guid(currentUser.Id));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> completedTodoItems = _repository.GetCompleted(new Guid(currentUser.Id));

            CompletedTodoViewModel model = new CompletedTodoViewModel();
            foreach (TodoItem item in completedTodoItems)
            {
                model.Items.Add(new TodoViewModel(item.Id, item.Text, item.DateCreated, item.DateCompleted, item.IsCompleted));
            }
            return View(model);
        }

        public async Task<IActionResult> RemoveFromCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Remove(id, new Guid(currentUser.Id));
            return RedirectToAction("Completed");
        }
    }
}