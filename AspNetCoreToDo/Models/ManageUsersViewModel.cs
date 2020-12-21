using Microsoft.AspNetCore.Identity;

namespace AspNetCoreToDo.Models{
    public class ManageUsersViewModel{
        public IdentityUser[] Administrator{get; set;}
        public IdentityUser[] Everyone{get; set;}
    }
}