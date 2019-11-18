using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EnterSchoolApp.Areas.Administration.DataTableCollections;
using EnterSchoolApp.DAL;
using EnterSchoolApp.jQuery.DataTables.Mvc;
using EnterSchoolApp.Models;
using EnterSchoolApp.Areas.Administration.Models.Institution;
using jQuery.DataTables.Mvc;

namespace EnterSchoolAppAreas.Administration.DataTableCollections
{
    public class InstitutionCollection : BaseEntityCollection, IEntityCollection<InstitutionViewModel>
    {
        public InstitutionCollection(ApplicationDbContext context):base(context)
        {

        }

        public IList<InstitutionViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            var institutions = DataContext.Institutions.AsQueryable();

            totalRecordCount = institutions.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                institutions = institutions.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) ||
                x.Phone.StartsWith(searchString) || x.Email.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = institutions.Count();

            IOrderedEnumerable<Institution> sortedCollection = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Name);
                        break;
                    case "Subdomain":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Subdomain)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Subdomain);
                        break;

                    case "Phone":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Phone)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Phone);
                        break;
                    case "Email":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Email)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Email);
                        break;
                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new InstitutionViewModel
                {
                    
                    Name = x.Name,
                    Subdomain =x.Subdomain,
                    InstitutionId = x.InstitutionId,
                    Phone = x.Phone,
                    Email = x.Email
                }).ToList();
        }
    }
}