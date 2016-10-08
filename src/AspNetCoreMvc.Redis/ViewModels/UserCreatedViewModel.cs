namespace AspNetCoreMvc.Redis.ViewModels
{
    using System;

    public class UserCreatedViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}