namespace persist_net_backend.Repositories
{
    public interface IExportRepository
    {
        /// <summary>
        /// Obtiene todos los datos de la base de datos en formato dinámico para exportación
        /// </summary>
        /// <returns>Diccionario con nombres de tablas y sus datos</returns>
        Task<Dictionary<string, List<Dictionary<string, object?>>>> GetAllDataAsync();

        /// <summary>
        /// Obtiene datos de una tabla específica
        /// </summary>
        /// <param name="tableName">Nombre de la tabla/entidad</param>
        /// <returns>Lista de registros de la tabla</returns>
        Task<List<Dictionary<string, object?>>> GetTableDataAsync(string tableName);
    }
}
