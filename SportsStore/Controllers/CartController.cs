using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Infrastructure;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product p = repository.Products
                .FirstOrDefault(c => c.ProductId == productId);

            if (p != null)
            {
                Cart cart = GetCart();
                cart.AddItem(p, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart (int productId, string returnUrl)
        {
            Product p = repository.Products
                .FirstOrDefault(c => c.ProductId == productId);

            if (p != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(p);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart (Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}