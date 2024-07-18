using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace shopping.Controllers
{
    public class CartController : Controller
    {
        /// <summary>
        /// 購物車列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            using var cart = new z_sqlCarts();
            var model = cart.GetDataList();
            return View(model);
        }

        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="id">商品編號</param>
        /// <param name="qty">數量</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddCart(string id, int qty = 1)
        {
            using var cart = new z_sqlCarts();
            cart.AddCart(id, "", 1);
            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        /// <summary>
        /// 更新購物車
        /// </summary>
        /// <param name="id">商品編號</param>
        /// <param name="qty">數量</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UpdateCart(string id, int qty)
        {
            using var cart = new z_sqlCarts();
            cart.UpdateCart(id, qty);
            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        /// <summary>
        /// 更新購物車數量
        /// </summary>
        /// <param name="prodNo">商品編號</param>
        /// <param name="qty">數量</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UpdateQty(string prodNo, int qty)
        {
            using var cart = new z_sqlCarts();
            cart.UpdateCart(prodNo, qty);
            var CartTotal = cart.GetCartTotals();
            return Json(new { success = true, value = CartTotal });
        }

        /// <summary>
        /// 刪除購物車
        /// </summary>
        /// <param name="id">購物車Id</param>
        [HttpGet]
        public IActionResult DeleteCart(int id)
        {
            using var cart = new z_sqlCarts();
            cart.DeleteCart(id);
            return RedirectToAction("Index", "Cart", new { area = "" });
        }
    }
}