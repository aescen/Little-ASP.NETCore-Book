using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreToDo.Data;
using AspNetCoreToDo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreToDo.Services{
    public class ToDoItemService : IToDoItemService{
        private readonly ApplicationDbContext _context;
        public ToDoItemService(ApplicationDbContext context){
            _context = context;
        }

        public async Task<ToDoItem[]> GetIncompleteItemsAsync(IdentityUser user){
            return await _context.Items
                .Where(x => x.IsDone == false && x.UserId == user.Id)
                .ToArrayAsync();
        }
        public async Task<bool> AddItemAsync(ToDoItem newItem, IdentityUser user){
            if(newItem.Id == null) newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            if(newItem.DueAt == null) newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            newItem.UserId = user.Id;
            _context.Items.Add(newItem);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user){
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();
            
            if(item == null) return false;

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}