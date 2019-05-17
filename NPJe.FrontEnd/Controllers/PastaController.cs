using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Repository.Queries;
using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class PastaController : Controller
    {
        // GET: Pasta
        public ActionResult Index()
        {
            DefineViewDatas();
            new PastaRepository().ExcluirRegistrosInconsistentes();

            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.SituacaoAtendimento = EnumExtension.GetList<SituacaoAtendimentoEnum>();
            return View("Index");
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