﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc_WebApp_Level2.Models;
using Mvc_WebApp_Level2.Models.BusinessLogic;
using Mvc_WebApp_Level2.Models.Interfaces_Layer;

namespace Mvc_WebApp_Level2.Controllers
{
    public class StudentController : Controller
    {
        private readonly IclsTrainee model;
        private readonly IclsDepartment depts;

        public StudentController(IclsTrainee _model, IclsDepartment _depts)
        {
            model = _model;
            depts = _depts;
        }

        public IActionResult Index()
        {
            CookieOptions options = new CookieOptions()
            {
                Domain = "localhost",
                Expires = DateTime.Now.AddYears(1),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                Path = "/"
            };
            Response.Cookies.Append("UserId", "MobP");
            Response.Cookies.Append("UserName", "Abdelhady Ashraf");
            Response.Cookies.Append("Password", "y8882gy98", options);

            SessionOptions sessionOptions = new SessionOptions()
            {
                IdleTimeout = TimeSpan.FromMinutes(20),
                IOTimeout = TimeSpan.FromMinutes(20),
            };
            HttpContext.Session.SetInt32("Id", 1534);
            HttpContext.Session.SetString("Name", "Session1");
            return View("List", model.GetAll());
        }

        public IActionResult Details(int id)//Details 
        {
            var viewModel = model.GetOne(id);

            ViewBag.Previous = model.GetPreviousId();
            ViewBag.Next = model.GetNextId();

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Add(int id)
        {
            ViewBag.DeptIds = (from dept in depts.GetAll()
                               select new { dept.Id, dept.Name }).ToList();
            if (id == 0)
                return View(new Trainee());
            return View("Add", model.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Save(Trainee row)
        {
            if (ModelState.IsValid)
            {
                if (row.Id == 0)
                    model.Add(row);
                else
                    model.Edit(row);
                return RedirectToAction("Index");
            }
            ViewBag.DeptIds = (from dept in depts.GetAll()
                               select new { dept.Id, dept.Name }).ToList();
            return View("Add", row);
            
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            model.Delete(id);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult RelativeInfo(int deptId)
        {
            return PartialView("List", model.GetRelativeDataToDepartment(deptId));
        }
    }
}
