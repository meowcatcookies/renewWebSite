using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopping.Models
{
  public class z_sqlPayments : DapperSql<Payments>
  {
    public z_sqlPayments()
    {
      OrderByColumn = SessionService.SortColumn;
      OrderByDirection = SessionService.SortDirection;
      DefaultOrderByColumn = "Payments.PaymentNo";
      DefaultOrderByDirection = "ASC";
      if (string.IsNullOrEmpty(OrderByColumn)) OrderByColumn = DefaultOrderByColumn;
      if (string.IsNullOrEmpty(OrderByDirection)) OrderByDirection = DefaultOrderByDirection;
    }
    ///<summary>
    ///取得付款方式下拉選項資料
    ///</summary>
    ///<param name="textIncludeValue">是否包含代號</param>
    ///<return></return>
    public List<SelectListItem> GetDropDownList(bool textIncludeValue = false)
    {
      string str_query = "SELECT ";
      if (textIncludeValue) str_query += "PaymentNo + ' ' + ";
      str_query += "PaymentName As Text , PaymentNo AS Value FROM Payments ";
      str_query += "ORDER BY PaymentNo";
      var model = dpr.ReadAll<SelectListItem>(str_query);
      return model;
    }
  }
}
