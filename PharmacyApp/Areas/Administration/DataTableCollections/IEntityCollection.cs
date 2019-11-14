using System.Collections.Generic;
using System.Collections.ObjectModel;
using jQuery.DataTables.Mvc;

namespace EnterSchoolAppAreas.Administration.DataTableCollections
{
    public interface IEntityCollection<T> where T : class, new()
    {
        IList<T> GetEntityData(int startIndex,
            int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount,
            string searchString);
    }
}
