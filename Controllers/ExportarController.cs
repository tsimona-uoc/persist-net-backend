using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Services;
using System.Diagnostics;
using System.Text;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ExportarController : ControllerBase
    {
        private readonly IExportService _exportService;

        public ExportarController(IExportService exportService)
        {
            _exportService = exportService;
        }

        /// <summary>
        /// Exporta todos los datos de la base de datos en formato XML
        /// </summary>
        [HttpGet("xml")]
        public async Task<IActionResult> ExportarXml()
        {
            try
            {
                var xmlContent = await _exportService.ExportAllToXmlAsync();
                var bytes = Encoding.UTF8.GetBytes(xmlContent);

                return File(bytes, "application/xml",
                    $"export_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Exporta datos a Odoo usando Python
        /// </summary>
        [HttpPost("odoo")]
        public async Task<IActionResult> ExportarAOdoo()
        {
            try
            {
                // 1. Obtener datos en JSON desde tu servicio existente
                var jsonContent = await _exportService.ExportAllToJsonAsync();

                // 2. Guardar JSON en archivo temporal
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "data_export.json");
                await System.IO.File.WriteAllTextAsync(filePath, jsonContent);

                // 3. Configurar proceso Python
                var process = new Process();
                process.StartInfo.FileName = "python";

                process.StartInfo.Arguments = "odoo_integration/generar_xml.py";

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // 4. Ejecutar Python
                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                process.WaitForExit();

                // 5. Control de errores
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(new { error = error });
                }

                return Ok(new
                {
                    mensaje = "Exportación a Odoo realizada correctamente",
                    detalle = output
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Exporta los datos de una tabla específica en formato XML
        /// </summary>
        [HttpGet("xml/{tableName}")]
        public async Task<IActionResult> ExportarTablaXml(string tableName)
        {
            try
            {
                var xmlContent = await _exportService.ExportTableToXmlAsync(tableName);
                var bytes = Encoding.UTF8.GetBytes(xmlContent);

                return File(bytes, "application/xml",
                    $"export_{tableName}_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Exporta todos los datos en formato JSON
        /// </summary>
        [HttpGet("json")]
        public async Task<IActionResult> ExportarJson()
        {
            try
            {
                var jsonContent = await _exportService.ExportAllToJsonAsync();
                var bytes = Encoding.UTF8.GetBytes(jsonContent);

                return File(bytes, "application/json",
                    $"export_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Preview XML
        /// </summary>
        [HttpGet("preview/xml")]
        public async Task<IActionResult> PreviewXml()
        {
            try
            {
                var xmlContent = await _exportService.ExportAllToXmlAsync();
                return Content(xmlContent, "application/xml");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Preview JSON
        /// </summary>
        [HttpGet("preview/json")]
        public async Task<IActionResult> PreviewJson()
        {
            try
            {
                var jsonContent = await _exportService.ExportAllToJsonAsync();
                return Content(jsonContent, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}