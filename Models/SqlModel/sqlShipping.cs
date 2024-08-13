using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopping.Models
{
  public class z_sqlShippings : DapperSql<Shippings>
  {
    public z_sqlShippings()
    {
      OrderByColumn = SessionService.SortColumn;
      OrderByDirection = SessionService.SortDirection;
      DefaultOrderByColumn = "Shippings.ShippingNo";
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
      if (textIncludeValue) str_query += "ShippingNo+' '+ ";
      str_query += "ShippingName As Text , ShippingNo AS Value FROM Shippings ";
      str_query += "ORDER BY ShippingNo";
      var model = dpr.ReadAll<SelectListItem>(str_query);
      return model;
    }
  }
}
