using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        /// <returns>Archivo XML con todos los datos</returns>
        [HttpGet("xml")]
        public async Task<IActionResult> ExportarXml()
        {
            try
            {
                var xmlContent = await _exportService.ExportAllToXmlAsync();
                var bytes = System.Text.Encoding.UTF8.GetBytes(xmlContent);
                
                return File(bytes, "application/xml", 
                    $"export_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Exporta los datos de una tabla específica en formato XML
        /// </summary>
        /// <param name="tableName">Nombre de la tabla a exportar</param>
        /// <returns>Archivo XML con datos de la tabla</returns>
        [HttpGet("xml/{tableName}")]
        public async Task<IActionResult> ExportarTablaXml(string tableName)
        {
            try
            {
                var xmlContent = await _exportService.ExportTableToXmlAsync(tableName);
                var bytes = System.Text.Encoding.UTF8.GetBytes(xmlContent);
                
                return File(bytes, "application/xml", 
                    $"export_{tableName}_{DateTime.Now:yyyyMMdd_HHmmss}.xml");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Exporta todos los datos de la base de datos en formato JSON
        /// </summary>
        /// <returns>Archivo JSON con todos los datos</returns>
        [HttpGet("json")]
        public async Task<IActionResult> ExportarJson()
        {
            try
            {
                var jsonContent = await _exportService.ExportAllToJsonAsync();
                var bytes = System.Text.Encoding.UTF8.GetBytes(jsonContent);
                
                return File(bytes, "application/json", 
                    $"export_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna el XML formateado en la respuesta (no descargable)
        /// </summary>
        /// <returns>XML como contenido de la respuesta</returns>
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
        /// Retorna el JSON formateado en la respuesta (no descargable)
        /// </summary>
        /// <returns>JSON como contenido de la respuesta</returns>
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
