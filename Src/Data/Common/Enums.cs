using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public enum Genders
    {
        Female, Male
    }
    public enum OrderBy
    {
        None, Ascending, Descending
    }
    public enum OperationDb
    {
        New = 0,
        Update = 1,
        Delete = 2,
        Nothing = 3,
    }

    public enum WarehouseStatus
    {
        Active, Inactive
    }

}
