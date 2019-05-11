using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Repository.Queries;
using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class AlunoController : Controller
    {
        public ActionResult Index()
        {
            DefineViewDatas();
            new AlunoRepository().ExcluirRegistrosInconsistentes();

            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.Especialidades = EnumExtension.GetList<EspecialidadeEnum>();
            ViewBag.DiaSemana = EnumExtension.GetList<DiaSemanaEnum>();
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