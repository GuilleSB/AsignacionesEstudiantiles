using AsignacionesEstudiantiles.Data;
using AsignacionesEstudiantiles.Models;
using AsignacionesEstudiantiles.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace AsignacionesEstudiantiles.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly string _connectionString;
        private readonly GetData _getData;
        public HomeController(ILogger<HomeController> logger, string connectionString, GetData getData)
        {
            _logger = logger;
            _connectionString = connectionString;
            _getData = getData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Programa(DateTime fechaInicio)
        {
            List<DateTime> fechas = new()
            {
                fechaInicio
            };

            for (var i = 0; i < 3; i++)
            {
                var aux = fechaInicio.AddDays(7);

                fechas.Add(aux);

                fechaInicio = aux;
            }

            ViewBag.Fechas = fechas;

            ViewBag.Estudiantes = GetEstudiantes();
            ViewBag.Asignaciones = GetAsignaciones();

            return View();
        }

        private List<EstudianteModel> GetEstudiantes()
        {
            try
            {
                var estudiantes = _getData.GetEstudiantes();

                return estudiantes;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<AsignacionModel> GetAsignaciones()
        {

            try
            {
                var asignaciones = _getData.GetAsignaciones();

                return asignaciones;

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult GenerarPrograma(string model)
        {
            try
            {
                var mod = JsonConvert.DeserializeObject<List<ProgramaModel>>(model);

                DrawOverImageUtil.Write(mod, _getData);

                return Ok("Generado correctamente!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Lista(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                if (fechaInicio  == null || fechaFin == null)
                {
                    fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    fechaFin = fechaInicio?.AddMonths(1).AddDays(-1);
                }
                var programa = _getData.GetHojita().Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin).OrderBy(x => x.Fecha).ToList();

                ViewBag.Lista = programa;
                ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Descargar(string id)
        {
            try
            {
                var programa = _getData.GetHojita();
                
                var hojita = programa.Where(x => x.Id == id).FirstOrDefault();

                var net = new WebClient();
                var data = net.DownloadData(hojita.Archivo);
                var content = new MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                var fileName = hojita.Asignacion + DateTime.Now.Ticks.ToString() + ".jpg";

                return File(content, contentType, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Eliminar(string id)
        {
            try
            {
                var result = _getData.DeletePrograma(id);

                return result > 0 ? Ok("Eliminado correctamente") : BadRequest("No se eliminó ningún registro"); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CompararFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}