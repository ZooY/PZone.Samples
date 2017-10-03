using System.Web.Mvc;


namespace PZone.Samples.Controllers
{
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}