using NPJe.FrontEnd.Controllers.CRUDs;
using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            DefineViewDatas();
            
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Home");

            DefineNotifications();
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