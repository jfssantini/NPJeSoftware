using Newtonsoft.Json;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Controllers.CRUDs;
using NPJe.FrontEnd.Dtos;
using System;
using System.Web.Mvc;

namespace NPJe.FrontEnd.Controllers
{
    public class AgendamentoController : Controller
    {
        private AgendamentoDto Agendamento;

        public AgendamentoController()
        {
            Agendamento = new AgendamentoDto();
        }

        // GET: Agendamento
        public ActionResult Index()
        {
            if (Session?["IdUsuario"] == null)
                return RedirectToAction("Login", "Home");

            DefineViewDatas();
            DefineReturnObject();
            DefineNotifications();
            return View();
        }

        private void DefineNotifications()
        {
            ViewBag.Agendamentos = new AgendamentoCrudController().GetAtendimentosByIsuario();
        }

        private void DefineReturnObject()
        {
            var result = "0";
            if (ReturnSession.ReturnObj != null)
                result = JsonConvert.SerializeObject(ReturnSession.ReturnObj);

            ReturnSession.ReturnObj = null;
            ViewBag.Agendamento = result;
        }

        private void DefineViewDatas()
        {
            ViewData["Usuario"] = Session?["Usuario"] ?? "usuario";
            ViewData["Papel"] = Session?["Papel"] ?? "papel";
            ViewData["IdPapel"] = Session?["IdPapel"] ?? 0;
            ViewData["Status"] = Session?["Status"] ?? "offline";
        }
    }
}