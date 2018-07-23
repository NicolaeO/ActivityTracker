using System.ComponentModel.DataAnnotations;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace ActivityTracker.Models
{
    public abstract class BaseEntity{}
    
    public class Person : BaseEntity
    {
        [Key]
        public int UserID { get; set; }
        
        [Required]
        [MinLength(2)]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabetsof First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(2)]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@$!%*#?&])[A-Za-z\\d$@$!%*#?&]{8,}$", 
       
        ErrorMessage = "Passwords must contain at least 1 letter, 1 number and 1 special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt {get; set;}

        [DataType(DataType.Date)]
        public DateTime UpdatedAt {get; set;}

        public List<MyActivity> myActivity { get; set; }
        public List<UserActivity> UserActivity { get; set; }
       
        public Person()
        {
            myActivity = new List<MyActivity>();
            UserActivity = new List<UserActivity>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}