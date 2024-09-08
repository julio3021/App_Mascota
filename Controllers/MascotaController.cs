using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using App_Mascota.Data;
using App_Mascota.Models;
using App_Mascota.ViewModel;

namespace App_Mascota.Controllers
{
    public class MascotaController : Controller
    {
        private readonly ILogger<MascotaController> _logger;
        private readonly ApplicationDbContext _context;

        public MascotaController(ILogger<MascotaController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var mismascotas = _context.DataMascota.ToList();
            var viewModel = new MascotaViewModel
            {
                FormMascota = new Mascota(),
                ListMascota = mismascotas
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Enviar(MascotaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var mascota = new Mascota
                {
                    Nombre = viewModel.FormMascota.Nombre,
                    Raza = viewModel.FormMascota.Raza,
                    Color = viewModel.FormMascota.Color,
                    FechaNacimiento = viewModel.FormMascota.FechaNacimiento.ToUniversalTime() // Convertir a UTC
                };

                _context.Add(mascota);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), viewModel);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var mascota = _context.DataMascota.Find(id);
            if (mascota == null)
            {
                return NotFound();
            }

            var viewModel = new MascotaViewModel
            {
                FormMascota = mascota,
                ListMascota = _context.DataMascota.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(MascotaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var mascota = _context.DataMascota.Find(viewModel.FormMascota.Id);
                if (mascota == null)
                {
                    return NotFound();
                }

                mascota.Nombre = viewModel.FormMascota.Nombre;
                mascota.Raza = viewModel.FormMascota.Raza;
                mascota.Color = viewModel.FormMascota.Color;
                mascota.FechaNacimiento = viewModel.FormMascota.FechaNacimiento.ToUniversalTime(); // Convertir a UTC

                _context.Update(mascota);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(long id)
        {
            var mascota = _context.DataMascota.Find(id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(long id)
        {
            var mascota = _context.DataMascota.Find(id);
            if (mascota != null)
            {
                _context.DataMascota.Remove(mascota);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
