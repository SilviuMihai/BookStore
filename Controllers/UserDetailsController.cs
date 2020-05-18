using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class UserDetailsController:Controller
    {
        public UserDetailsController()
        {

        }
        [HttpGet]
        public IActionResult CaptureUserDetails()
        {
            return View();
        }

        //[HttpPost]
       // public IActionResult CaptureUserDetails()
        //{
            
       // }

    }
}
