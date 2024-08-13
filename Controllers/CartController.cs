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
      cart.AddCart(id, "", qty);
      string str_button = SessionService.StringValue1;
      SessionService.StringValue1 = "";
      if (str_button == "buy")
        return RedirectToAction("Payment", "Cart", new { area = "" });
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
    [HttpPost]
    public IActionResult AddToCart()
    {
      string str_button = "";
      object obj_add = Request.Form["add"];
      object obj_buy = Request.Form["buy"];
      object obj_prod_no = Request.Form["ProdNo"];
      object obj_qty = Request.Form["qtybutton"];
      string str_prod_no = (obj_prod_no == null) ? "" : obj_prod_no.ToString();
      string str_qty = (obj_qty == null) ? "1" : obj_qty.ToString();
      string str_add = (obj_add == null) ? "" : obj_add.ToString();
      string str_buy = (obj_buy == null) ? "" : obj_buy.ToString();
      if (str_add == "加入購物車") str_button = "add";
      if (str_buy == "立即結帳") str_button = "buy";

      if (str_button != "add" && str_button != "buy")
        return RedirectToAction("Index", "Home", new { area = "" });
      if (str_button == "buy" && !SessionService.IsLogin)
        return RedirectToAction("Login", "User", new { area = "" });
      SessionService.StringValue1 = str_button;

      int int_qty = int.Parse(str_qty);

      if (!string.IsNullOrEmpty(str_prod_no) && int_qty > 0)
      {

        return RedirectToAction("AddCart", "Cart", new { area = "", id = str_prod_no, qty = int_qty });
      }
      return RedirectToAction("Index", "Home", new { area = "" });
    }
    [HttpGet]
    [Login(RoleList = "Member")]
    public IActionResult Payment()
    {
      using var users = new z_sqlUsers();
      var model = users.GetPaymentUser();
      return View(model);
    }
    [HttpPost]
    [Login(RoleList = "Member")]
    public IActionResult Payment(vmOrders model)
    {
      if (string.IsNullOrEmpty(model.ReceiveName)) model.ReceiveName = model.MemberName;
      if (string.IsNullOrEmpty(model.ReceiveTel)) model.ReceiveTel = model.ReceiveTel;
      if (string.IsNullOrEmpty(model.ReceiveEmail)) model.ReceiveEmail = model.ReceiveEmail;
      if (string.IsNullOrEmpty(model.ReceiveAddress)) model.ReceiveAddress = model.ReceiveAddress;
      model.IsDiffenceMember = (model.ReceiveName != model.MemberName) ? "on" : "off";
      PaymentService.SetPaymentData(model);
      return RedirectToAction("PaymentConfirm", "Cart", new { are = "" });
    }
    [HttpGet]
    [Login(RoleList = "Member")]
    public IActionResult PaymentConfirm()
    {

      var model = PaymentService.GetPaymentData();
      model.ShippingNo = "Delivery"; //宅配到府
      model.PaymentNo = "Cash"; //貨到付款
      return View(model);
    }
    [HttpPost]
    [Login(RoleList = "Member")]
    public IActionResult PaymentConfirm(vmOrders model)
    {
      //產生訂單
      var data = PaymentService.GetPaymentData();
      data.ShippingNo = model.ShippingNo;
      data.PaymentNo = model.PaymentNo;
      string str_order_no = PaymentService.CreateOrder(data);
      using var carts = new z_sqlCarts();
      carts.DeleteCart();
      SessionService.StringValue1 = "HomeIndex";
      SessionService.MessageText = $"您的訂單已建立，訂單編號為:{str_order_no}，請注意到貨訊息!!";

      return RedirectToAction("Index", "Message", new { area = "" });
    }
  }


}
