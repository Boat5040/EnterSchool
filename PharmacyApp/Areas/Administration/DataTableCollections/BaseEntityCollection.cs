using EnterSchoolApp.DAL;

namespace EnterSchoolApp.Areas.Administration.DataTableCollections
{
    public class BaseEntityCollection
    {
        protected readonly ApplicationDbContext DataContext;

        public BaseEntityCollection(ApplicationDbContext context)
        {
            DataContext = context;
        }

    }
}