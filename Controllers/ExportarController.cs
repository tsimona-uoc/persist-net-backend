using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Services;
using System.Diagnostics;
using System.Text;

namespace persist_net_backend.Controllers
{
    public class ExportarOdooRequest
    {
        public string NombreLote { get; set; } = "Lote_Default";
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

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
        public async Task<IActionResult> ExportarAOdoo([FromBody] ExportarOdooRequest request)
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
                process.StartInfo.FileName = "/home/python/bin/python3";

                // 4. Pasamos los parámetros al script de python
                process.StartInfo.Arguments = $"odoo_integration/generar_xml.py \"{request.NombreLote}\"";

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // 5. Ejecutar Python
                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                process.WaitForExit();

                // 6. Control de errores
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
        /// Vista previa de la exportación en XML
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
        /// Vista previa de la exportación en JSON
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

        /// <summary>
        /// Descarga un archivo de Odoo XML desde la carpeta export
        /// Solo permite descargar archivos que contengan "odoo" y ".xml" en el nombre
        /// </summary>
        [HttpGet("download-odoo/{fileName}")]
        public IActionResult DescargarArchivoOdoo(string fileName)
        {
            try
            {
                // Validar que el nombre de archivo no sea vacío
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest(new { error = "El nombre del archivo no puede estar vacío" });
                }

                // Validar que contenga "odoo" y ".xml"
                if (!fileName.Contains("odoo", StringComparison.OrdinalIgnoreCase) || !fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "El archivo debe contener 'odoo' en el nombre y tener extensión .xml" });
                }

                // Validar que no contenga caracteres peligrosos de ruta
                if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                {
                    return BadRequest(new { error = "Nombre de archivo inválido" });
                }

                // Construir ruta segura dentro de la carpeta export
                var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "export");
                var filePath = Path.Combine(baseDir, fileName);

                // Verificar que el archivo está dentro de la carpeta export (evitar directory traversal)
                var fullPath = Path.GetFullPath(filePath);
                var fullBaseDir = Path.GetFullPath(baseDir);

                if (!fullPath.StartsWith(fullBaseDir, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "Acceso denegado. El archivo no está en la carpeta permitida" });
                }

                // Verificar que el archivo existe
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound(new { error = "El archivo no existe" });
                }

                // Descargar el archivo
                var bytes = System.IO.File.ReadAllBytes(fullPath);
                return File(bytes, "application/xml", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}