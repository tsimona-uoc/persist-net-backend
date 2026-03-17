namespace persist_net_backend.Services
{
    public interface IExportService
    {
        /// <summary>
        /// Exporta todos los datos de la base de datos a formato XML
        /// </summary>
        /// <returns>String con contenido XML</returns>
        Task<string> ExportAllToXmlAsync();

        /// <summary>
        /// Exporta datos de una tabla específica a formato XML
        /// </summary>
        /// <param name="tableName">Nombre de la tabla a exportar</param>
        /// <returns>String con contenido XML</returns>
        Task<string> ExportTableToXmlAsync(string tableName);

        /// <summary>
        /// Exporta todos los datos de la base de datos a formato JSON
        /// </summary>
        /// <returns>String con contenido JSON</returns>
        Task<string> ExportAllToJsonAsync();
    }
}
