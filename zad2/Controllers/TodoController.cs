using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zad1.from_last_homework;
using zad1;
using zad2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> todoItems;
            TodoViewModel todoView;
            IndexViewModel model = new IndexViewModel();
            try
            {
                todoItems = _repository.GetActive(Guid.Parse(currentUser.Id));
                foreach (TodoItem i in todoItems)
                {
                    todoView = new TodoViewModel(i);
                    model.Items.Add(todoView);
                }
            }
            catch (ArgumentNullException ignorable)
            {
                return View("Error");
            }
            catch (FormatException ignorable)
            {
                return View("Error");
            }
            return View(model);
        }


        public IActionResult Add()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                TodoItem item;
                TodoItemLabel label;

                try
                {
                    item = new TodoItem(model.Text, Guid.Parse(await _userManager.GetUserIdAsync(currentUser)));
                }
                catch (FormatException ex)
                {
                    // logger
                    return View("Error");
                }
                catch (ArgumentNullException ex)
                {
                    //logger
                    return View("Error");
                }
                try
                {
                    if (model.Label != null)
                    {
                        char[] separator = { ',' };
                        string[] labels = model.Label.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string l in labels)
                        {
                            label = _repository.GetLabel(l);
                            if (label == null)
                            {
                                label = new TodoItemLabel(l);
                                label.LabelTodoItems.Add(item);
                            }
                            else
                            {
                                _repository.AddItemToLabel(item, label);
                            }
                            item.Labels.Add(label);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
                try
                {
                    item.DateDue = model.DateDue;
                    _repository.Add(item);
                    return RedirectToAction("Index");
                }
                catch (DuplicateTodoItemException ex)
                {
                    // logger
                    return View();
                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> todoItems;
            TodoViewModel todoView;
            CompletedViewModel model = new CompletedViewModel();
            try
            {
                todoItems = _repository.GetCompleted(Guid.Parse(currentUser.Id));
                foreach (TodoItem i in todoItems)
                {
                    todoView = new TodoViewModel(i);
                    model.Items.Add(todoView);
                }
            }
            catch (ArgumentNullException ignorable)
            {
                return View("Error");
            }
            catch (FormatException ignorable)
            {
                return View("Error");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            try
            {
                _repository.Remove(id, Guid.Parse(currentUser.Id));
            }
            catch (TodoAccessDeniedException ex)
            {
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Completed");
        }

        [HttpGet]
        public async Task<IActionResult> MarkAsCompleted(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            try
            {
                if (!_repository.MarkAsCompleted(Id, Guid.Parse(currentUser.Id)))
                {
                    return View("Error");
                }
            }
            catch (TodoAccessDeniedException ex)
            {
                // logger
                return RedirectToAction("Index");
            }
            catch (ArgumentNullException ignorable)
            {
                return View("Error");
            }
            catch (FormatException ignorable)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}