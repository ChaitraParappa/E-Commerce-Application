using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using ProductTaskDay3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using ProductTaskDay3.ProductTask.DAL;

[assembly: OwinStartup(typeof(ProductTaskDay3.Startup))]

namespace ProductTaskDay3
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new ProductDbContext());
            app.CreatePerOwinContext<UserStore<ExtentedSignUp>>((opt,cont) => new UserStore<ExtentedSignUp>(cont.Get<ProductDbContext>()));
            app.CreatePerOwinContext<UserManager<ExtentedSignUp>>((opt, cont) => new UserManager<ExtentedSignUp>(cont.Get<UserStore<ExtentedSignUp>>()));
            app.CreatePerOwinContext<SignInManager<ExtentedSignUp, string>>((opt, cont) => new SignInManager<ExtentedSignUp, string>(cont.Get<UserManager<ExtentedSignUp>>(),cont.Authentication));


            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType=DefaultAuthenticationTypes.ApplicationCookie
            });
        }
    }
}
