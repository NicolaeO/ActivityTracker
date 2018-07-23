using System.ComponentModel.DataAnnotations;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace ActivityTracker.Models{
    public class UserActivity : BaseEntity{
        [Key]
        public int UserActivityID {get; set;}

        public int UserID {get; set;}
        public Person User {get; set;}

        public int ActivityID {get; set;}
        public MyActivity Activity {get; set;}

        [DataType(DataType.Date)]
        public DateTime CreatedAt {get; set;}

        [DataType(DataType.Date)]
        public DateTime UpdatedAt {get; set;}

        public UserActivity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
    
}