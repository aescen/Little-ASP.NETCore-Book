using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreToDo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreToDo.Services{
    public interface IToDoItemService{
        Task<ToDoItem[]> GetIncompleteItemsAsync(IdentityUser currentUser);
        Task<bool> AddItemAsync(ToDoItem newItem, IdentityUser currentUser);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser currentUser);
    }
}