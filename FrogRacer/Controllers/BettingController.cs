﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrogRacer.Models;

namespace FrogRacer.Controllers
{
    public class BettingController : Controller
    {
        public ActionResult CalculateFrogRace(int? frog1, int? frog2, int? frog3, int? frog4, int? frog5)
        {
            List<Frog> lineUpFrogs = (List<Frog>)Session["frogList"];

            Random rnd = new Random();

            var winnerFrogsNumber = rnd.Next(0,lineUpFrogs.Count);
            var winnerFrog = lineUpFrogs[winnerFrogsNumber];

            ViewBag.winnerFrog = winnerFrog;

            int newBalance = 0;

            if (frog1 != null && frog2 != null && frog3 != null && frog4 != null && frog5 != null)
            {
                newBalance = (int)Session["balance"];

                List<int?> bettingsList = new List<int?>();
                bettingsList.Add(frog1);
                bettingsList.Add(frog2);
                bettingsList.Add(frog3);
                bettingsList.Add(frog4);
                bettingsList.Add(frog5);

                for (int i = 0; i < lineUpFrogs.Count; i++)
                {
                    if (i == winnerFrogsNumber && bettingsList[winnerFrogsNumber] != null)
                    {
                        newBalance += (int)bettingsList[winnerFrogsNumber] * 2;
                    }
                    else
                    {
                        if (bettingsList[i] != null)
	                    {
                            newBalance -= (int)bettingsList[i];
	                    }
                        
                    }
                }
            }

            //Storage - Spara saldo börjar här

            newBalance += 1; // <-- Save newBalance to storage and remove this line

            //Storage - Slut

            return View("Result", winnerFrog);
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
