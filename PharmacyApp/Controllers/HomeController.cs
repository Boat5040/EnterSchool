namespace EnterSchoolApp.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EnterSchoolApp.Constants;
    using EnterSchoolApp.DAL;
    using EnterSchoolApp.Helper;
    using EnterSchoolApp.Models;
    using EnterSchoolApp.Services;
    using Microsoft.AspNet.Identity;


    public class HomeController : BaseController
    {
        #region Fields
        private readonly IBrowserConfigService browserConfigService;
        private readonly IFeedService feedService;
        private readonly IManifestService manifestService;
        private readonly IOpenSearchService openSearchService;
        private readonly IRobotsService robotsService;
        private readonly ISitemapService sitemapService;
        #endregion

        #region Constructors
        public HomeController(
            IBrowserConfigService browserConfigService,
            IFeedService feedService,
            IManifestService manifestService,
            IOpenSearchService openSearchService,
            IRobotsService robotsService,
            ISitemapService sitemapService) : base(context: new ApplicationDbContext())
        {

            this.browserConfigService = browserConfigService;
            this.feedService = feedService;
            this.manifestService = manifestService;
            this.openSearchService = openSearchService;
            this.robotsService = robotsService;
            this.sitemapService = sitemapService;
        }

        public HomeController() { }

        public HomeController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }

        #endregion

        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (TempData.Count == 0) // user was persisted
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user.ForceChangePassword && !user.HasPasswordChange)  // reset/create by super/admin
                {
                    TempData[Alerts.Warn] = $"{user.FirstName + "" + user.LastName}, please change your password";

                }
                else
                {
                    var institution = await DataContext.Institutions.FindAsync(user.InstitutionId);
                    var policy = institution?.SecurityPolicy.ToObject<SecurityPolicy>();

                }
            }
            DateTime now = DateTime.Now.Date;
            DateTime aWeekBefore = now.AddDays(-7);

            if (User.IsInRole(UserRoles.Administrator))
            {
                //var model = new ViewModels.AdminDashboardViewModel
                //{

                //};
                //return View(HomeControllerAction.Index, model);
            }
            else if (User.IsInRole(UserRoles.SuperAdministrator))
            {
                var model = new Areas.Administration.Models.DashBoard.SuperAdminDashboardViewModel
                {
                //    Administrators = await Task.Run(() => DataContext.Users.Count(u => u.InstitutionId.HasValue && u.InstitutionId.Value > 0 && u.Status != UserStatus.Deleted)),
                //    ExpiredUsers = await Task.Run(() => DataContext.Users.Count(u => !u.InstitutionId.HasValue && u.Status == UserStatus.Expired)),
                //    Institutions = await Task.Run(() => DataContext.Institutions.Count())
                };
                return View("IndexSuper");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        [Route("about", Name = HomeControllerRoute.GetAbout)]
        public ActionResult About()
        {

            return View(HomeControllerAction.About);
        }

        [Route("contact", Name = HomeControllerRoute.GetContact)]
        public ActionResult Contact()
        {

            return View(HomeControllerAction.Contact);
        }
    }
}