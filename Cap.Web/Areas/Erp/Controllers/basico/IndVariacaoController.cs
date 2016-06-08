﻿using Cap.Domain.Abstract;
using Cap.Domain.Abstract.Admin;
using Cap.Domain.Models.Gen;
using Cap.Web.Common;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cap.Web.Areas.Erp.Controllers.basico
{
    [AreaAuthorizeAttribute("Erp", Roles = "indvariacao-r")]
    public class IndVariacaoController : Controller
    {
        private IBaseService<IndVariacao> service;
        private IBaseService<Indice> serviceIndice;
        private ILogin login;

        public IndVariacaoController(IBaseService<IndVariacao> service, IBaseService<Indice> serviceIndice, ILogin login)
        {
            this.service = service;
            this.serviceIndice = serviceIndice;
            this.login = login;
        }

        // GET: Erp/IndVariacao
        public ActionResult Index(int id)
        {
            ViewBag.IdIndice = id;

            return View(serviceIndice.Listar()
                .Where(x => x.Ativo == true)
                .OrderBy(x => x.Descricao)
                .AsEnumerable());
        }

        [HttpPost]
        public PartialViewResult Variacoes(int idIndice, DateTime? inicial)
        {
            if (inicial == null)
            {
                inicial = DateTime.Today.Date.AddYears(-1);
            }

            var variacoes = service.Listar()
                .Where(x => x.IdIndice == idIndice
                && x.DataVariacao >= (DateTime)inicial)
                .AsEnumerable();

            ViewBag.IdIndice = idIndice;
            ViewBag.Indice = serviceIndice.Find(idIndice).Descricao;
            return PartialView(variacoes);
        }

        // GET: Erp/IndVariacao/Create
        [AreaAuthorizeAttribute("Erp", Roles = "indvariacao-c")]
        public ActionResult Create(int idIndice)
        {
            var item = new IndVariacao
            {
                AlteradoPor = login.GetIdUsuario(System.Web.HttpContext.Current.User.Identity.Name),
                DataVariacao = new DateTime(DateTime.Today.AddMonths(-1).Year, DateTime.Today.AddMonths(-1).Month, 1),
                IdIndice = idIndice,
                Variacao = 0
            };

            ViewBag.IdIndice = idIndice;
            ViewBag.Indice = serviceIndice.Find(idIndice).Descricao;
            return PartialView(item);
        }

        // POST: Erp/IndVariacao/Create
        [HttpPost]
        public ActionResult Create(IndVariacao item)
        {
            try
            {
                item.AlteradoEm = DateTime.Now;
                TryUpdateModel(item);

                if (ModelState.IsValid)
                {
                    service.Gravar(item);
                    return Json(new { success = true });
                }

                return PartialView(item);
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return PartialView(item);
            }
        }

        // GET: Erp/IndVariacao/Edit/5
        [AreaAuthorizeAttribute("Erp", Roles = "indvariacao-u")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IndVariacao item = service.Find((int)id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView(item);
        }

        // POST: Erp/IndVariacao/Edit/5
        [HttpPost]
        public ActionResult Edit(IndVariacao item)
        {
            try
            {
                item.AlteradoEm = DateTime.Now;
                TryUpdateModel(item);

                if (ModelState.IsValid)
                {
                    service.Gravar(item);
                    return Json(new { success = true });
                }

                return PartialView(item);
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return PartialView();
            }
        }

        // GET: Erp/IndVariacao/Delete/5
        [AreaAuthorizeAttribute("Erp", Roles = "indvariacao-d")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IndVariacao item = service.Find((int)id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView(item);
        }

        // POST: Erp/IndVariacao/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                service.Excluir(id);
                return Json(new { success = true });
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                var item = service.Find(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return PartialView(item);
            }
        }
    }
}
