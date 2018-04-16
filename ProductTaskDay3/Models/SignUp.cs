using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductTaskDay3.Models
{
    public class SignUp
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public long Mobilenumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string ContactPerson { get; set; }
        public string BusinessAddress { get; set; }
        public string ContactNumber { get; set; }

    }
    public class ExtentedSignUp:IdentityUser
    {
        public string Fullname { get; set; }
        public long Mobilenumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string ContactPerson { get; set; }
        public string BusinessAddress { get; set; }
        public string ContactNumber { get; set; }
    }
}