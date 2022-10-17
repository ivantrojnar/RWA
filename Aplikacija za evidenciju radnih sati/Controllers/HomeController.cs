﻿using Aplikacija_za_administraciju.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Aplikacija_za_evidenciju_radnih_sati.Models;

namespace Aplikacija_za_evidenciju_radnih_sati.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Djelatnik djelatnik = Session["djelatnik"] as Djelatnik;

            IEnumerable<Projekt> projektiDjelatnika = Repozitorij.GetProjektiDjelatnika(djelatnik.IDDjelatnik);

            var projekti = new List<SelectListItem> { new SelectListItem { Text = " -- odaberi projekt --", Value = (-1).ToString() } };

            foreach (var projekt in projektiDjelatnika)
            {
                projekti.Add(new SelectListItem { Text = projekt.Naziv, Value = projekt.IDProjekt.ToString() });
            }

            ViewBag.projekti = projekti;

            return View();
        }

        [HttpPost]
        public ActionResult Index(Satnica satnica)
        {
            return View();
        }

        public ActionResult GetSatnicaForProjekti(int idProjekt)
        {
            Djelatnik djelatnik = Session["djelatnik"] as Djelatnik;

            Satnica model = Repozitorij.GetZadnjaSatnicaDjelatnikaZaProjekt(djelatnik, Repozitorij.GetProjekt(idProjekt));

            return View(model);
        }

        public ActionResult ShowProfil()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction(actionName: "Login", controllerName: "Home", routeValues: null);
        }

        [HttpGet]
        public ActionResult PromjenaSifre()
        {
            var model = new ChangePasswordViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PromjenaSifre(ChangePasswordViewModel changePassword)
        {
            Djelatnik djelatnik = Session["djelatnik"] as Djelatnik;
            djelatnik.Lozinka = changePassword.NewPassword;
            Repozitorij.PromijeniDjelatnikuLozinku(djelatnik);
            return View();
        }
    }
}