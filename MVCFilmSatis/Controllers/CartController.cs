using Microsoft.AspNet.Identity;
using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);
            if (c.ShoppingCart == null)
            {
                c.ShoppingCart = new ShoppingCart();
                c.ShoppingCart.Movies = new List<Movie>();
            }
            return View(c.ShoppingCart);
        }

        public ActionResult DeleteFromCart(int id)
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);
            Movie m = c.ShoppingCart.Movies.Where(x => x.MovieId == id).FirstOrDefault();
            c.ShoppingCart.Movies.Remove(m);
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int id, int page)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { error = "Login to buy movies." });
            }
            //Giriş yapmış kişinin Id'si
            string uid = User.Identity.GetUserId();
            //giriş yapmıs kısının tum bılgılerı.
            Customer c = db.Users.Find(uid);

            if (c.ShoppingCart == null)
                c.ShoppingCart = new ShoppingCart();

            if (c.ShoppingCart.Movies == null)
                c.ShoppingCart.Movies = new List<Movie>();

            if (c.ShoppingCart.Movies.Any(x => x.MovieId == id))
            {
                return RedirectToAction("Index", "Home", new { error = "You already have this movie in your cart." });
            }
            else
            {
                Movie chosenMovie = db.Movies.Find(id);
                c.ShoppingCart.Movies.Add(chosenMovie);

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home", new { page = page });
            }
        }

        public ActionResult Checkout()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);

            ViewBag.CartNo = c.ShoppingCart.ShoppingCartId;
            ViewBag.Total = c.ShoppingCart.SubTotal;
            return View();
        }

        public ActionResult PayBankTransfer(int? approve)
        {
            if (approve.HasValue && approve.Value == 1)
            {
                string uid = User.Identity.GetUserId();
                Customer c = db.Users.Find(uid);

                BankTransferPayment p1 = new BankTransferPayment();
                p1.IsApproved = false;
                p1.NameSurname = User.Identity.GetNameSurname();
                p1.TC = User.Identity.GetTC();

                BankTransferService service = new BankTransferService();

                bool isPaid = service.MakePayment(p1);

                if (isPaid)
                {
                    CreateOrder(isPaid);
                    ResetShoppingCart();
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Checkout");
        }

        private void ResetShoppingCart()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);
            c.ShoppingCart.Movies.Clear();
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Order CreateOrder(bool isPaid)
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);

            Order order = new Order();
            order.Date = DateTime.Now;
            order.Customer = c;
            order.IsPaid = isPaid;
            order.OrderItems = new List<OrderItem>();
            foreach (var item in c.ShoppingCart.Movies)
            {
                OrderItem oi = new OrderItem();
                oi.Movie = item;
                oi.Count = 1;
                oi.Price = item.Price;
                order.OrderItems.Add(oi);
            }
            order.SubTotal = c.ShoppingCart.SubTotal;
            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }
    }
}