using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Shopping.Areas.User.Controllers
{
  [Area("User")]
  public class OrderController : Controller
  {
    [HttpGet]
    public IActionResult Init(string id = "")
    {
      SessionService.SortColumn = "";
      SessionService.SortDirection = "asc";
      SessionService.SearchText = "";
      SessionService.BaseNo = id;
      return RedirectToAction("Index", "Order", new { area = "User" });
    }

    /// <summary>
    /// 員工資料列表
    /// </summary>
    /// <param name="page">目前頁數</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int page = 1, int pageSize = 10)
    {
      using var orders = new z_sqlOrders();
      var model = orders.GetOrderQueryList(SessionService.BaseNo, true).ToPagedList(page, pageSize);
      ViewBag.PageInfo = $"第 {page} 頁，共 {model.PageCount}頁";
      ViewBag.SearchText = SessionService.SearchText;
      return View(model);
    }

    [HttpGet]
    public IActionResult Detail(int id = 0)
    {
      var model = new vmOrderQuery();
      using var orders = new z_sqlOrders();
      using var details = new z_sqlOrderDetails();
      model.MasterData = orders.GetData(id);
      model.DetailData = details.GeOrderDetailList(model.MasterData.SheetNo);
      return View(model);
    }

    [HttpGet]
    public IActionResult Status(int id = 0)
    {
      using var orders = new z_sqlOrders();
      var model = orders.GetData(id);
      return View(model);
    }

    [HttpPost]
    public IActionResult Status(Orders model)
    {
      using var orders = new z_sqlOrders();
      orders.ChangeStatus(model.SheetNo, model.StatusCode);
      return RedirectToAction("Index", "Order", new { area = "User" });
    }

    /// <summary>
    /// 查詢
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Search()
    {
      object obj_text = Request.Form["SearchText"];
      SessionService.SearchText = (obj_text == null) ? string.Empty : obj_text.ToString();
      return RedirectToAction("Index", "Order", new { area = "User" });
    }

    /// <summary>
    /// 欄位排序
    /// </summary>
    /// <param name="id">指定排序的欄位</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Sort(string id)
    {
      if (SessionService.SortColumn == id)
      {
        SessionService.SortDirection = (SessionService.SortDirection == "asc") ? "desc" : "asc";
      }
      else
      {
        SessionService.SortColumn = id;
        SessionService.SortDirection = "asc";
      }
      return RedirectToAction("Index", "Order", new { area = "User" });
    }
  }
}
