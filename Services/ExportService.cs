using System.Text.Json;
using System.Xml.Linq;
using persist_net_backend.Repositories;

namespace persist_net_backend.Services
{
    public class ExportService : IExportService
    {
        private readonly IExportRepository _exportRepository;

        public ExportService(IExportRepository exportRepository)
        {
            _exportRepository = exportRepository;
        }

        public async Task<string> ExportAllToXmlAsync()
        {
            var allData = await _exportRepository.GetAllDataAsync();

            var root = new XElement("Database",
                new XAttribute("ExportedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XAttribute("Tables", allData.Count)
            );

            foreach (var (tableName, rows) in allData)
            {
                var tableElement = new XElement("Table",
                    new XAttribute("Name", tableName),
                    new XAttribute("Rows", rows.Count)
                );

                foreach (var row in rows)
                {
                    var rowElement = new XElement("Row");

                    foreach (var (columnName, value) in row)
                    {
                        var columnElement = new XElement("Column",
                            new XAttribute("Name", columnName),
                            value?.ToString() ?? string.Empty
                        );
                        rowElement.Add(columnElement);
                    }

                    tableElement.Add(rowElement);
                }

                root.Add(tableElement);
            }

            var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            return xmlDocument.ToString();
        }

        public async Task<string> ExportTableToXmlAsync(string tableName)
        {
            var tableData = await _exportRepository.GetTableDataAsync(tableName);

            var root = new XElement("Table",
                new XAttribute("Name", tableName),
                new XAttribute("ExportedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XAttribute("Rows", tableData.Count)
            );

            foreach (var row in tableData)
            {
                var rowElement = new XElement("Row");

                foreach (var (columnName, value) in row)
                {
                    var columnElement = new XElement("Column",
                        new XAttribute("Name", columnName),
                        value?.ToString() ?? string.Empty
                    );
                    rowElement.Add(columnElement);
                }

                root.Add(rowElement);
            }

            var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            return xmlDocument.ToString();
        }

        public async Task<string> ExportAllToJsonAsync()
        {
            var allData = await _exportRepository.GetAllDataAsync();

            var json = new
            {
                ExportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Tables = allData.Count,
                Data = allData
            };

            return JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
