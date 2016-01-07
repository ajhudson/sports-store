using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SportsStore.Infrastructure.Identity;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    public class PrepController : Controller
    {
        IRepository repo;

        public PrepController()
        {
            repo = new ProductRepository();
        }

        // GET: Prep
        public ActionResult Index()
        {
            return View(repo.Products);
        }

        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await repo.DeleteProductsAsync(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> SaveProduct(Product p)
        {
            await repo.SaveProductsAsync(p);
            return RedirectToAction("Index");
        }

        public ActionResult Orders()
        {
            return View(repo.Orders);
        }

        public async Task<ActionResult> DeleteOrder(int id)
        {
            await repo.DeleteOrdersAsync(id);
            return RedirectToAction("Orders");
        }

        public async Task<ActionResult> SaveOrder(Order o)
        {
            await repo.SaveOrdersAsync(o);
            return RedirectToAction("Orders");
        }

        public async Task<ActionResult> SignIn()
        {
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            StoreUserManager userManager = HttpContext.GetOwinContext().GetUserManager<StoreUserManager>();

            StoreUser user = await userManager.FindAsync("Admin", "secret");
            authManager.SignIn(await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
            return RedirectToAction("Index");
        }

        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}