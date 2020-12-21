using System;
using System.Threading.Tasks;
using AspNetCoreToDo.Data;
using AspNetCoreToDo.Models;
using AspNetCoreToDo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AspNetCoreToDo.UnitTests{
    public class ToDoItemServiceShould{
        private const string DatabaseName = "Test_Db";
        private const string fakeUserId = "fake-000";
        private const string fakeUserName = "fake0@todo.local";
        private static IdentityUser fakeUser;
        private static Guid guidId = Guid.NewGuid();
        private const string stringTitle = "Testing?";
        private static ToDoItem newItem;
        private static DbContextOptions<ApplicationDbContext> options;

        
        [Fact]
        public async Task AddNewItemAsIncompleteDueDate(){ 
            // The UserId property should be set to the user's ID
            // New items should always be incomplete ( IsDone = false )
            // The title of the new item should be copied from newItem.Title
            // New items should always be due 3 days from now
            

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: DatabaseName).Options;
            fakeUser = new IdentityUser{
                Id = fakeUserId,
                UserName = fakeUserName
            };
            newItem = new ToDoItem{
                Id = guidId,
                Title = stringTitle
            };
            
            //Set up a context (connection to the database) for writing
            using (var context = new ApplicationDbContext(options)){
                var service = new ToDoItemService(context);

                await service.AddItemAsync(newItem, fakeUser);
            }

            using (var context = new ApplicationDbContext(options)){
                var itemsInDatabase = await context
                    .Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var item = await context.Items.FirstAsync();
                Assert.Equal(stringTitle, item.Title);
                Assert.Equal(false, item.IsDone);

                //Item should be due 3 days from now if DueAt is null
                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                Assert.True(difference < TimeSpan.FromSeconds(10));
            }
        }

        [Fact]
        public async Task MarkItemAsDone(){
            using (var context = new ApplicationDbContext(options)){
                var service = new ToDoItemService(context);

                await service.MarkDoneAsync(guidId, fakeUser);
            }

            using (var context = new ApplicationDbContext(options)){
                var itemsInDatabase = await context
                    .Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                //Item isDone should be true
                var item = await context.Items.FirstAsync();
                Assert.Equal(guidId, item.Id);
                Assert.Equal(true, item.IsDone);
            }
        }
        
        [Fact]
        public async Task GetIncompleteItemAsyncShould(){
            using (var context = new ApplicationDbContext(options)){
                var service = new ToDoItemService(context);

                var items = await service.GetIncompleteItemsAsync(fakeUser);
                Assert.Equal(1, items.Length);
                var itemUserId = items[0];
                Assert.Equal(fakeUserId, itemUserId.UserId);
            }

        } 
       
    }
}