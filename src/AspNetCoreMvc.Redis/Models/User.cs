namespace AspNetCoreMvc.Redis.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public Guid Id { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}