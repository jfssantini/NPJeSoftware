using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Controllers.CRUDs;
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

            DefineNotifications();
            ViewBag.Especialidades = EnumExtension.GetList<EspecialidadeEnum>();
            ViewBag.DiaSemana = EnumExtension.GetList<DiaSemanaEnum>();
            return View("Index");
        }


        private void DefineNotifications()
        {
            ViewBag.Agendamentos = new AgendamentoCrudController().GetAtendimentosByIsuario();
        }

        private void DefineViewDatas()
        {
            ViewData["Usuario"] = Session?["Usuario"] ?? "usuario";
            ViewData["Papel"] = Session?["Papel"] ?? "papel";
            ViewData["IdPapel"] = Session["IdPapel"] ?? 0;
            ViewData["Status"] = Session?["Status"] ?? "offline";
        }
    }
}