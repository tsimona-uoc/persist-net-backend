using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImportarController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportarController(IImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Importa datos desde un archivo XML
        /// </summary>
        /// <param name="file">Archivo XML a importar</param>
        /// <returns>Resultado de la importación con estadísticas</returns>
        [HttpPost("xml")]
        public async Task<IActionResult> ImportarXml(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { error = "El archivo no puede estar vacío" });

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var xmlContent = await reader.ReadToEndAsync();

                    // Validación para interceptar errores antes de pasarlo al servicio
                    new System.Xml.XmlDocument().LoadXml(xmlContent);

                    var result = await _importService.ImportFromXmlAsync(xmlContent);
                    return Ok(result);
                }
            }
            catch (System.Xml.XmlException xmlEx)
            {
                return BadRequest(new { error = "Error de formato en el documento XML: " + xmlEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Importa datos desde contenido XML en el cuerpo de la solicitud
        /// </summary>
        /// <param name="request">Request para leer el contenido XML</param>
        /// <returns>Resultado de la importación con estadísticas</returns>
        [HttpPost("xml/content")]
        public async Task<IActionResult> ImportarXmlContent()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var xmlContent = await reader.ReadToEndAsync();
                    
                    if (string.IsNullOrEmpty(xmlContent))
                        return BadRequest(new { error = "El contenido XML no puede estar vacío" });

                    // Validación estructural rápida
                    new System.Xml.XmlDocument().LoadXml(xmlContent);

                    var result = await _importService.ImportFromXmlAsync(xmlContent);
                    return Ok(result);
                }
            }
            catch (System.Xml.XmlException xmlEx)
            {
                return BadRequest(new { error = "El XML proporcionado tiene errores de sintaxis: " + xmlEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Importa datos de una tabla específica desde un archivo XML
        /// </summary>
        /// <param name="tableName">Nombre de la tabla a importar</param>
        /// <param name="file">Archivo XML con los datos de la tabla</param>
        /// <returns>Resultado de la importación con estadísticas</returns>
        [HttpPost("xml/table/{tableName}")]
        public async Task<IActionResult> ImportarTablaXml(string tableName, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { error = "El archivo no puede estar vacío" });

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var xmlContent = await reader.ReadToEndAsync();

                    // Validación estructural rápida
                    new System.Xml.XmlDocument().LoadXml(xmlContent);

                    var result = await _importService.ImportTableFromXmlAsync(tableName, xmlContent);
                    return Ok(result);
                }
            }
            catch (System.Xml.XmlException xmlEx)
            {
                return BadRequest(new { error = $"Error al leer el XML para la tabla {tableName}: " + xmlEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
