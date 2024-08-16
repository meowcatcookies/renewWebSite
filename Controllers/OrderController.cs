using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace shopping.Controllers
{
  public class OrderController : Controller
  {

    public IActionResult Init(string id = "")
    {
      SessionService.BaseNo = id;
      return RedirectToAction("Index", "Order", new { area = "" });
    }
    public IActionResult Index()
    {
      using var orders = new z_sqlOrders();
      var model = orders.GetOrderQueryList(SessionService.BaseNo);
      return View(model);
    }
    [HttpGet]
    public IActionResult Details(int id = 0)
    {
      var model = new vmOrderQuery();
      using var orders = new z_sqlOrders();
      using var details = new z_sqlOrderDetails();
      model.MasterData = orders.GetData(id);
      model.DetailData = details.GeOrderDetailList(model.MasterData.SheetNo);
      return View(model);
    }
    [HttpGet]
    public IActionResult Cancel(int id = 0)
    {
      using var orders = new z_sqlOrders();
      orders.CancelOrder(id);
      return RedirectToAction("Index","Order",new{area=""});
    }
  }
}
