
using AutoMapper;
using MyApp.Application.Interface;
using MyApp.Domain.Entities;
using MyApp.MVC.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyApp.MVC.Controllers
{
    public class WebScrapingController : Controller
    {
        //private readonly WebScrapingResultRepository _webScrapingResultRepository = new WebScrapingResultRepository();
        private readonly IWebScrapingResultAppService _webScrapingResultAppService;

        public WebScrapingController(IWebScrapingResultAppService webScrapingResultAppService)
        {
            _webScrapingResultAppService = webScrapingResultAppService;
        }

        // GET: WebScraping
        public ActionResult Index()
        {
            var webScrapingResultViewModel = Mapper.Map<IEnumerable<WebScrapingResult>, IEnumerable<WebScrapingResultViewModel>>(_webScrapingResultAppService.GetAll());
            return View(webScrapingResultViewModel);
        }

        // GET: WebScraping/Details/5
        public ActionResult Details(int id)
        {
            var webScrapingResult = _webScrapingResultAppService.GetById(id);
            var webScrapingResultViewModel = Mapper.Map<WebScrapingResult, WebScrapingResultViewModel>(webScrapingResult);

            return View(webScrapingResultViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: WebScraping/Create
        public ActionResult Create(WebScrapingResultViewModel webScrapingResult)
        {
            if (ModelState.IsValid)
            {
                var webScrapingResultDomain = Mapper.Map<WebScrapingResultViewModel, WebScrapingResult>(webScrapingResult);
                _webScrapingResultAppService.Add(webScrapingResultDomain);
                return RedirectToAction("Index");
            }
            return View(webScrapingResult);
        }

        // POST: WebScraping/Create
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

        // GET: WebScraping/Edit/5
        public ActionResult Edit(int id)
        {
            var webScrapingResult = _webScrapingResultAppService.GetById(id);
            var webScrapingResultViewModel = Mapper.Map<WebScrapingResult, WebScrapingResultViewModel>(webScrapingResult);
            
            return View(webScrapingResultViewModel);
        }

        // POST: WebScraping/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebScrapingResultViewModel webScrapingResult)
        {
            if(ModelState.IsValid)
            {
                var webScrapingResultDomain = Mapper.Map<WebScrapingResultViewModel, WebScrapingResult>(webScrapingResult);
                _webScrapingResultAppService.Update(webScrapingResultDomain);

                return RedirectToAction("Index");
            }

            return View(webScrapingResult);
        }



        // POST: WebScraping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var webScrapingResult = _webScrapingResultAppService.GetById(id);
            _webScrapingResultAppService.Remove(webScrapingResult);

            return RedirectToAction("Index");
        }

        // POST: WebScraping/Delete/5
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
