namespace persist_net_backend.Services
{
    public interface IImportService
    {
        /// <summary>
        /// Importa datos desde una cadena XML. 
        /// Si el identificador ya existe, actualiza el registro. Si no existe, lo crea.
        /// </summary>
        /// <param name="xmlContent">Contenido XML con los datos a importar</param>
        /// <returns>Objeto con información sobre los registros procesados</returns>
        Task<ImportResult> ImportFromXmlAsync(string xmlContent);

        /// <summary>
        /// Importa datos de una tabla específica desde XML
        /// Si el identificador ya existe, actualiza el registro. Si no existe, lo crea.
        /// </summary>
        /// <param name="tableName">Nombre de la tabla a importar</param>
        /// <param name="xmlContent">Contenido XML con los datos de la tabla</param>
        /// <returns>Objeto con información sobre los registros procesados</returns>
        Task<ImportResult> ImportTableFromXmlAsync(string tableName, string xmlContent);
    }

    /// <summary>
    /// Resultado de una operación de importación
    /// </summary>
    public class ImportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int TotalRecords { get; set; }
        public int CreatedRecords { get; set; }
        public int UpdatedRecords { get; set; }
        public int FailedRecords { get; set; }
        public List<string> Errors { get; set; } = new();
        public Dictionary<string, int>? TableSummary { get; set; }
    }
}
