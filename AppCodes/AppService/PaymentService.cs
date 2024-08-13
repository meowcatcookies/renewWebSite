using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


public static class PaymentService
{
  /// <summary>
  /// HttpContextAccessor 物件
  /// </summary>
  /// <returns></returns>
  public static IHttpContextAccessor _contextAccessor { get; set; } = new HttpContextAccessor();
  /// <summary>
  /// HttpContext 物件
  /// </summary>
  public static HttpContext? _context { get { return _contextAccessor.HttpContext; } }
  /// <summary>
  /// 會員名稱
  /// </summary>
  /// <value></value>
  public static string MemberName
  {
    get { return GetSessionValue("MemberName"); }
    set { SetSessionValue("MemberName", value); }
  }
  /// <summary>
  /// 會員Email
  /// </summary>
  /// <value></value>
  public static string MemberEmail
  {
    get { return GetSessionValue("MemberEmail"); }
    set { SetSessionValue("MemberEmail", value); }
  }
  /// <summary>
  /// 會員電話
  /// </summary>
  /// <value></value>
  public static string MemberTel
  {
    get { return GetSessionValue("MemberTel"); }
    set { SetSessionValue("MemberTel", value); }
  }
  /// <summary>
  /// 會員地址
  /// </summary>
  /// <value></value>
  public static string MemberAddress
  {
    get { return GetSessionValue("MemberAddress"); }
    set { SetSessionValue("MemberAddress", value); }
  }
  /// <summary>
  /// 收件人資料與購買人資料不同
  /// </summary>
  /// <value></value>
  public static string IsDiffenceMember
  {
    get { return GetSessionValue("IsDiffenceMember"); }
    set { SetSessionValue("IsDiffenceMember", value); }
  }
  /// <summary>
  /// 收件人姓名
  /// </summary>
  /// <value></value>
  public static string ReceiveName
  {
    get { return GetSessionValue("ReceiveName"); }
    set { SetSessionValue("ReceiveName", value); }
  }
  /// <summary>
  /// 收件人信箱
  /// </summary>
  /// <value></value>
  public static string ReceiveEmail
  {
    get { return GetSessionValue("ReceiveEmail"); }
    set { SetSessionValue("ReceiveEmail", value); }
  }
  /// <summary>
  /// 收件人電話
  /// </summary>
  /// <value></value>
  public static string ReceiveTel
  {
    get { return GetSessionValue("ReceiveTel"); }
    set { SetSessionValue("ReceiveTel", value); }
  }
  /// <summary>
  /// 收件人地址
  /// </summary>
  /// <value></value>
  public static string ReceiveAddress
  {
    get { return GetSessionValue("ReceiveAddress"); }
    set { SetSessionValue("ReceiveAddress", value); }
  }
  /// <summary>
  /// 儲存購買資訊
  /// </summary>
  /// <param name="model">購買資訊</param>
  public static void SetPaymentData(vmOrders model)
  {
    MemberName = model.MemberName;
    MemberEmail = model.MemberEmail;
    MemberTel = model.MemberTel;
    MemberAddress = model.MemberAddress;
    ReceiveName = model.ReceiveName;
    ReceiveEmail = model.ReceiveEmail;
    ReceiveTel = model.ReceiveTel;
    ReceiveAddress = model.ReceiveAddress;
    IsDiffenceMember = model.IsDiffenceMember;
  }
  /// <summary>
  /// 取得購買資訊
  /// </summary>
  /// <returns></returns>
  public static vmOrders GetPaymentData()
  {
    var model = new vmOrders();
    model.MemberName = MemberName;
    model.MemberEmail = MemberEmail;
    model.MemberTel = MemberTel;
    model.MemberAddress = MemberAddress;
    model.ReceiveName = ReceiveName;
    model.ReceiveEmail = ReceiveEmail;
    model.ReceiveTel = ReceiveTel;
    model.ReceiveAddress = ReceiveAddress;
    model.IsDiffenceMember = IsDiffenceMember;
    return model;
  }
  /// <summary>
  /// 產生訂單
  /// </summary>
  /// <param name="model">訂單付款資訊</param>
  /// <returns></returns>
  public static string CreateOrder(vmOrders model)
  {
    using var orders = new z_sqlOrders();
    CartService.OrderNo = orders.CreateNewOrder(model);
    return CartService.OrderNo;
  }
  private static string GetSessionValue(string sessionName)
  {
    string str_value = "";
    if (_context != null) str_value = _context.Session.Get<string>(sessionName);
    if (str_value == null) str_value = "";
    return str_value;
  }
  public static void SetSessionValue(string sessionName, string value)
  {
    _context?.Session.Set<string>(sessionName, value);
  }
}
