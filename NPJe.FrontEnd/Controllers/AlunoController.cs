using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class AlunoController : Controller
    {
        public ActionResult Index()
        {
            DefineViewDatas();
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Home");

            return View("Index");
        }

        public void DefineViewDatas()
        {
            ViewData["Usuario"] = Session?["Usuario"] ?? "usuario";
            ViewData["Papel"] = Session?["Papel"] ?? "papel";
            ViewData["Status"] = Session?["Status"] ?? "offline";
        }
    }
}