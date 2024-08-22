using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace shopping.Areas.User.Controllers
{
  [Area("User")]
  public class CategoryController : Controller
  {

    [HttpGet]
    public IActionResult Init(string id = "")
    {
      SessionService.SortColumn = "";
      SessionService.SortDirection = "";
      SessionService.SearchText = "";
      SessionService.BaseNo = id;
      SessionService.StringValue2 = id;
      SessionService.StringValue1 = "最上層";
      if (!string.IsNullOrEmpty(id))
      {
        using var cate = new z_sqlCategorys();
        var data = cate.GetData(id);
        if (data != null)
        {
          SessionService.StringValue2 = data.ParentNo;
          SessionService.StringValue1 = data.CategoryName;
        }
      }
      return RedirectToAction("Index", "Category", new { area = "User" });
    }

    [HttpGet]
    public IActionResult Index(int page = 1, int pageSize = 10)
    {
      using var cate = new z_sqlCategorys();
      var model = cate.GetDetailCategoryList(SessionService.BaseNo).ToPagedList(page, pageSize);
      ViewBag.PageInfo = $"第{page}頁，共{model.PageCount}頁";
      ViewBag.SubHeader = $"{SessionService.BaseNo}{SessionService.StringValue1}";
      ViewBag.SearchText = SessionService.SearchText;
      return View(model);

    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
      return RedirectToAction("CreateEdit", "Category", new { area = "User", id = 0 });

    }
    /// <summary>
    /// 新增/修改(id = 0為新增)
    /// </summary>
    /// <returns></returns>
    /// <param name="id">主鍵</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult CreateEdit(int id = 0)
    {
      using var cate = new z_sqlCategorys();
      var model = cate.GetData(id);
      if (id == 0)
      {
        model.IsEnabled = true;
        model.ParentNo = SessionService.BaseNo;
      }
      return View(model);
    }
    /// <summary>
    /// 新增/修改(id = 0為新增)
    /// </summary>
    /// <returns></returns>
    /// <param name="model">使用者輸入的資料結構</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateEdit(Categorys model)
    {

      if (!ModelState.IsValid)
        return View(model);
      using var cate = new z_sqlCategorys();
      if (model.Id == 0) model.ParentNo = SessionService.BaseNo;
      cate.CreateEdit(model);
      return RedirectToAction("Index", "Category", new { area = "User" });

    }
    [HttpGet]
    public IActionResult Delete(int id = 0)
    {
      using var cate = new z_sqlCategorys();
      cate.Delete(id);
      return RedirectToAction("Index", "Category", new { area = "User" });
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
