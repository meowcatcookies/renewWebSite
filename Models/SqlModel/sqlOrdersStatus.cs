using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopping.Models.SqlModel
{
  public class z_sqlOrdersStatus : DapperSql<OrdersStatus>
  {
    public z_sqlOrdersStatus()
    {
      OrderByColumn = SessionService.SortColumn;
      OrderByDirection = SessionService.SortDirection;
      DefaultOrderByColumn = "OrdersStatus.StatusNo";
      DefaultOrderByDirection = "ASC";
      if (string.IsNullOrEmpty(OrderByColumn)) OrderByColumn = DefaultOrderByColumn;
      if (string.IsNullOrEmpty(OrderByDirection)) OrderByDirection = DefaultOrderByDirection;
    }
  }
}
