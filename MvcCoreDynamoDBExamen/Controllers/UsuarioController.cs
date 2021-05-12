using Microsoft.AspNetCore.Mvc;
using MvcCoreDynamoDBExamen.Models;
using MvcCoreDynamoDBExamen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoDBExamen.Controllers
{
    public class UsuarioController : Controller
    {
        ServiceAWSDynamoDB ServiceDynamo;

        public UsuarioController(ServiceAWSDynamoDB service)
        {
            this.ServiceDynamo = service;
        }


        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceDynamo.GetUsuarios());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await this.ServiceDynamo.FindUser(id));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.ServiceDynamo.EliminarUsuario(id);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario user)
        {
            await this.ServiceDynamo.CreateUsuario(user);
            return RedirectToAction("Index");
        }

        public IActionResult Modificar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Modificar(Usuario user)
        {
            await this.ServiceDynamo.ModifyUsuario(user);
            return RedirectToAction("Index");
        }

    }
}
