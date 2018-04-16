using FunctionApp3;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ProductTaskDay3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductTaskDay3.Controllers
{
    public class ProfileController : Controller
    {
        public UserManager<ExtentedSignUp> UserManager => HttpContext.GetOwinContext().Get<UserManager<ExtentedSignUp>>();
        public SignInManager<ExtentedSignUp, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<ExtentedSignUp, string>>();
        SearchController searchController = new SearchController();

        public ActionResult SignUp()
        {


            return View();
        }


        public ActionResult SignUpCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUpMerchant(SignUp model)
        {
            try
            {
                var IdentityUser = await UserManager.FindByNameAsync(model.Username);
                if (IdentityUser != null)
                {
                  
                    return RedirectToAction("Login", "Profile");
                }
                var user = new ExtentedSignUp
                {
                    UserName = model.Username,
                    Fullname = model.Fullname,
                    Role="Merchant",
                    Address = model.BusinessAddress,
                    ContactNumber = model.ContactNumber,
                    ContactPerson=model.ContactPerson

                };
                Session["User"] = model.Username;


                var IdentityResult = await UserManager.CreateAsync(user, model.Password);
                if (IdentityResult.Succeeded)
                {
                    StorageInQueue g = new StorageInQueue();
                    // g.SendingToQueue(model.ContactPerson, model.BusinessAddress, model.ContactNumber);
                    //await g.SendingToServiceBusQueue(model.ContactPerson, model.BusinessAddress, model.ContactNumber);
                    TempData["user"] = user.UserName;
                    return RedirectToAction("Login", "Profile");
                }
                else

                    ModelState.AddModelError("", IdentityResult.Errors.FirstOrDefault());
                return View("SignUpCustomer");
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> SignUpCustomer(SignUp model)
        {
            try
            {
                var IdentityUser = await UserManager.FindByNameAsync(model.Username);
                if (IdentityUser != null)
                {
                    return RedirectToAction("Login", "Profile");
                }
                var user = new ExtentedSignUp
                {
                    UserName = model.Username,
                    Fullname = model.Fullname,
                    Address = model.Address,
                    Role = "Customer",
                    Mobilenumber = model.Mobilenumber
                };


                var IdentityResult = await UserManager.CreateAsync(user, model.Password);
                if (IdentityResult.Succeeded)
                {
                    return RedirectToAction("Login", "Profile");
                }
                else

                    ModelState.AddModelError("", IdentityResult.Errors.FirstOrDefault());
                return View();
            }catch(Exception e)
            {
                return View();
            }

        }

        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Login(Login model)
        {
            try
            {
                var SigninStatus = await SignInManager.PasswordSignInAsync(model.Username, model.Password, true, true);


                switch (SigninStatus)
                {

                    case SignInStatus.Success:
                        {

                            ExtentedSignUp n = await UserManager.FindAsync(model.Username, model.Password);
                            if (n.Role == "Customer")
                            {
                                Session["UserName"] = n.UserName;
                                Session["Role"] = "Customer";
                                ViewBag.Username = n.UserName;
                                ViewBag.fullname = n.Fullname;
                                ViewBag.address = n.Address;
                                ViewBag.MobileNumber = n.Mobilenumber;
                               //await searchController.StoringToCache(n.UserName,"Customer");
                                return View("DisplayProfile");
                            }
                            if (n.Role == "Merchant")
                            {
                                Session["UserName"] = n.UserName;
                                Session["User"] = n.UserName;
                                Session["Role"] = "Merchant";
                                return RedirectToAction("AddingProduct", "Home");
                            }

                            return View("DisplayProfile");
                        }

                    default:
                        ModelState.AddModelError("", "Invalid Credantials");
                        return View();
                }
            }
            catch (Exception e)
            {
                return View();
            }



        }

        public ActionResult SignUpMerchant()
        {
            return View();
        }
       public ActionResult Logout()
        {
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return RedirectToAction("SignUpCustomer", "Profile");
        }


    }
   
}