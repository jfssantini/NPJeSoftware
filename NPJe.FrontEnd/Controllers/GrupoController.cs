using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class GrupoController : Controller
    {
        // GET: Grupo
        public ActionResult Index()
        {
            DefineViewDatas();
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.Especialidades = EnumExtension.GetList<EspecialidadeEnum>();
            return View();
        }
        public void DefineViewDatas()
        {
            ViewData["Usuario"] = Session?["Usuario"] ?? "usuario";
            ViewData["Papel"] = Session?["Papel"] ?? "papel";
            ViewData["IdPapel"] = Session["IdPapel"] ?? 0;
            ViewData["Status"] = Session?["Status"] ?? "offline";
        }
    }
}