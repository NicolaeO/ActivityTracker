using System.ComponentModel.DataAnnotations;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace ActivityTracker.Models{

    public class MyActivity : BaseEntity
    {
        [Key]
        public int ActivityID { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Title { get; set; }
        
        [Required]        
        [DataType(DataType.Date)]
        
        public DateTime ActivityDate { get; set; }

        [Required]        
        public TimeSpan Time { get; set; }

       
        [Required]
        public string Duration {get; set;}

        [Required]
        [MinLength(10)]
        public string Description { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime CreatedAt {get; set;}

        [DataType(DataType.Date)]
        public DateTime UpdatedAt {get; set;}

        public int UserID {get; set;}
        public Person User {get; set;}

        public List<UserActivity> UserActivity { get; set; }
       
        public MyActivity()
        {
            UserActivity = new List<UserActivity>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}