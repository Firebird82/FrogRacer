﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrogRacer.Controllers
{
    public class BettingController : Controller
    {
        public ActionResult CalculateFrogRace(int? frog1, int? frog2, int? frog3, int? frog4, int? frog5)
        {
            return View("Result");
        }

        // GET: Betting
        public ActionResult Index()
        {
            return View();
        }

        // GET: Betting/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Betting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Betting/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Betting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Betting/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Betting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Betting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
