using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreToDo.Models;
using AspNetCoreToDo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreToDo.Controllers {
    
    [Authorize]
    public class ToDoController : Controller {
        private readonly IToDoItemService _toDoItemService;
        private readonly UserManager<IdentityUser> _userManager;
        public ToDoController(IToDoItemService toDoItemService,
                              UserManager<IdentityUser> userManager) {
            _toDoItemService = toDoItemService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(){
            // Check user
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            // Get user's to-do items from database 
            var items = await _toDoItemService.GetIncompleteItemsAsync(currentUser);

            // Put items into a model
            var model = new ToDoViewModel(){
                Items = items
            };
            
            // Pass the view to a model and render
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(ToDoItem newItem){
            if(!ModelState.IsValid){
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            var successful = await _toDoItemService
                .AddItemAsync(newItem, currentUser);
            
            if(!successful){
                return BadRequest("Could not add a new item!");
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id){
            if(id == Guid.Empty){
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            
            var successful = await _toDoItemService
                .MarkDoneAsync(id, currentUser);
            
            if(!successful){
                return BadRequest("Could not mark item as done!");
            }

            return RedirectToAction("Index");
        }

    }
}