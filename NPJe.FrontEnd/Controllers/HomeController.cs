using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Controllers.CRUDs;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Queries;
using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DefineViewDatas();
            
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login");

            DefineNotifications();
            return View();
        }

        public ActionResult Login()
        {
            DefineViewDatas();
            if (Session["IdUsuario"] == null)
                return View();
                
            return RedirectToAction("Index");
        }

        public ActionResult ConfirmLogin(string login, string password)
        {
            if(Session["IdUsuario"] == null)
            {
                var user = new QueriesRepository().ConfirmLogin(login, password);
                if (user != null)
                {
                    DefineSession(user);
                    DefineViewDatas();
                    return RedirectToAction("Index");
                }
                else
                {
                    DefineSession(null);
                    DefineViewDatas();
                    TempData["Error"] = "Usuário ou senha inválidos";
                    return View("Login");
                }
            }
            else
                return View("Index");
        }

        public ActionResult ExitLogin()
        {
            DefineSession(null);
            DefineViewDatas();
            return RedirectToAction("Login");
        }

        private void DefineSession(Usuario user)
        {
            if (user != null)
            {
                Session["IdUsuario"] = user.Id;
                SessionUser.IdUsuario = user.Id;
                Session["Usuario"] = user.UsuarioLogin;
                SessionUser.Usuario = user.UsuarioLogin;
                Session["IdPapel"] = (int) user.IdPapelUsuario;
                SessionUser.IdPapel = user.IdPapelUsuario;
                Session["IdStatus"] = StatusUsuarioEnum.Online;
                SessionUser.IdStatus = StatusUsuarioEnum.Online;

                Session["Status"] = StatusUsuarioEnum.Online.GetDescription().ToLower();
                Session["Papel"] = user.IdPapelUsuario.GetDescription().ToLower();
            }
            else
            {
                Session["IdUsuario"] = null;
                Session["Usuario"] = "usuario";
                Session["IdPapel"] = null;
                Session["Papel"] = "papel";
                Session["IdStatus"] = StatusUsuarioEnum.Offline;
                Session["Status"] = StatusUsuarioEnum.Offline.GetDescription().ToLower();
            }
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
