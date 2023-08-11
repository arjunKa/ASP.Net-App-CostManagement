using Microsoft.AspNetCore.Mvc;

namespace BillingManagement.Pages.Shared
{
    public class HomeController : Controller
    {
        public MainLayoutViewModel MainLayoutViewModel { get; set; }

        public IActionResult Index()
        {
            return View();
        }
        public HomeController()
        {
            this.MainLayoutViewModel = new MainLayoutViewModel();//has property PageTitle
            this.MainLayoutViewModel.PageTitle = "my title";

            this.ViewData["MainLayoutViewModel"] = this.MainLayoutViewModel;
        }
    }

    public class MainLayoutViewModel
    {
        public String PageTitle;
    }
}
