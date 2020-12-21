using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreToDo.Models {
    public class ToDoItem {
        public Guid Id{get; set;}

        public bool IsDone {get; set;}

        [Required]
        public String Title {get; set;}

        public DateTimeOffset? DueAt {get; set;}

        public String UserId {get; set;}
    }
}