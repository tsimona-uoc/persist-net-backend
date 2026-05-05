using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persist_net_backend.Services;
using ClosedXML.Excel;
using persist_net_backend.Models;

namespace persist_net_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImportarController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IImportService _importService;

        public ImportarController(IImportService importService, IAuthService authService)
        {
            _importService = importService;
            _authService = authService;
        }

        /// <summary>
        /// Importa correos electrónicos desde un archivo Excel (.xlsx)
        /// </summary>
        /// <param name="file">Archivo Excel a importar</param>
        /// <returns>Lista de emails obtenidos</returns>
        [HttpPost("xlsx")]
        public async Task<IActionResult> ImportarExcel(IFormFile file)
        {
            try
            {
                // 1. Validaciones básicas
                if (file == null || file.Length == 0)
                    return BadRequest(new { error = "El archivo no puede estar vacío" });

                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { error = "El formato debe ser .xlsx (Excel)" });

                var emails = new List<string>();

                // 2. Procesar el archivo con ClosedXML
                using (var stream = file.OpenReadStream())
                {
                    using (var workbook = new XLWorkbook(stream))
                    {
                        // Obtenemos la primera hoja de trabajo
                        var worksheet = workbook.Worksheet(1);

                        // Suponiendo que los emails están en la columna A, a partir de la fila 2 (saltando cabecera)
                        var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                        foreach (var row in rows)
                        {
                            // Obtenemos el valor de la primera celda de la fila
                            var fullName = row.Cell(11).GetValue<string>().Trim();
                            var email = row.Cell(13).GetValue<string>().Trim();


                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                string name = fullName.Contains(' ') ? fullName.Split(' ')[0] : fullName;
                                string surname = fullName.Contains(' ') ? fullName.Split(' ')[1] : string.Empty;
                                await this._authService.RegisterAsync(name, surname, email, email, "RECEPCIONISTA");
                            }
                        }
                    }
                }

                // 3. Retornar los datos obtenidos (o procesarlos con tu servicio)
                return Ok(new
                {
                    totalRegistros = emails.Count,
                    data = emails
                });
            }
            catch (Exception ex)
            {
                // Capturamos errores específicos de formato o lectura
                return BadRequest(new { error = "Error al procesar el Excel: " + ex.Message });
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
