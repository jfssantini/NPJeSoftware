using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            ViewData["Status"] = Session?["Status"] ?? "offline";
        }
    }
}