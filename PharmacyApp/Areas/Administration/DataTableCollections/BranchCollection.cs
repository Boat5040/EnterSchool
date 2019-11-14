using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EnterSchoolApp.DAL;
using EnterSchoolApp.jQuery.DataTables.Mvc;
using EnterSchoolApp.Models;
using EnterSchoolAppAreas.Administration.DataTableCollections;
using EnterSchoolApp.Areas.Administration.Models.Branch;
using jQuery.DataTables.Mvc;

namespace EnterSchoolApp.Areas.Administration.DataTableCollections
{
    public class BranchCollection:BaseEntityCollection,IEntityCollection<BranchViewModel>
    {
        private int _institutionId;

        public BranchCollection(ApplicationDbContext context, int institutionId) : base(context)
        {
            _institutionId = institutionId;
        }

        public IList<BranchViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            var branches = DataContext.Branches.Where(x => x.InstitutionId == _institutionId);

            totalRecordCount = branches.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                branches = branches.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = branches.Count();

            IOrderedEnumerable<Branch> sortedCollection = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedCollection = sortedCollection == null ? branches.CustomSort(sortedColumn.Direction, x => x.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Name);
                        break;
                    case "Phone":
                        sortedCollection = sortedCollection == null ? branches.CustomSort(sortedColumn.Direction, x => x.Phone)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Phone);
                        break;
                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new BranchViewModel
                {
                    BranchId = x.BranchId,
                    Phone = x.Phone,
                    Name = x.Name
                }).ToList();

        }
    }
}