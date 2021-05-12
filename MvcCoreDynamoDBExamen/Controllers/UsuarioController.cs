using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreDynamoDBExamen.Helpers;
using MvcCoreDynamoDBExamen.Models;
using MvcCoreDynamoDBExamen.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MvcCoreDynamoDBExamen.Controllers
{
    public class UsuarioController : Controller
    {
        ServiceAWSDynamoDB ServiceDynamo;
        private UploadHelper uploadhelper;
        public ServiceAWSS3 ServiceS3;
        const string url = "https://examenpractico2.s3-eu-west-1.amazonaws.com/";
        public UsuarioController(ServiceAWSDynamoDB serviceDynamo, UploadHelper uploadhelper, ServiceAWSS3 services3)
        {
            this.ServiceDynamo = serviceDynamo;
            this.uploadhelper = uploadhelper;
            this.ServiceS3 = services3;

        }


        public async Task<IActionResult> Index()
        {

            return View(await this.ServiceDynamo.GetUsuarios());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await this.ServiceDynamo.FindUser(id));
        }

        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(Usuario user, String incluirFotos, IFormFile foto)
        {
            if (incluirFotos != null)
            {
                user.Fotos = new List<Fotos>();
                Fotos fotos = new Fotos();
                fotos.Imagen = url + foto.FileName;
                fotos.Titulo = foto.FileName;
                user.Fotos.Add(fotos);
                String path = await this.uploadhelper.UploadFileAsync(foto, Folders.Images);

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    bool respuesta = await this.ServiceS3.UploadFileAsync(stream, foto.FileName);    
                }
            }
            user.FechaAlta = DateTime.Now.ToShortDateString();
            await this.ServiceDynamo.CreateUsuario(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {

            return View(await this.ServiceDynamo.FindUser(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Usuario user, String incluirFotos, IFormFile foto)
        {
            if (incluirFotos != null)
            {
                user.Fotos = new List<Fotos>();
                Fotos fotos = new Fotos();
                fotos.Imagen = url + foto.FileName;
                fotos.Titulo = foto.FileName;
                user.Fotos.Add(fotos);
                String path = await this.uploadhelper.UploadFileAsync(foto, Folders.Images);

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    bool respuesta = await this.ServiceS3.UploadFileAsync(stream, foto.FileName);
                }
            }
           await this.ServiceDynamo.Edit(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Usuario user = await this.ServiceDynamo.FindUser(id);
            
            if (user.Fotos != null)
            {
                await this.ServiceS3.DeleteFileAsync(user.Fotos);
            }
          
            await this.ServiceDynamo.EliminarUsuario(id);
            return RedirectToAction("Index");
        }

    }
}
