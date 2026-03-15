using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;

namespace persist_net_backend.Repositories
{
    public class ExportRepository : IExportRepository
    {
        private readonly AppDbContext _context;

        public ExportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, List<Dictionary<string, object?>>>> GetAllDataAsync()
        {
            var allData = new Dictionary<string, List<Dictionary<string, object?>>>();

            // Obtener todas las entidades registradas en el DbContext
            var entityTypes = _context.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetTableName() ?? entityType.Name;
                
                try
                {
                    // Usar el método genérico Set<T>() con reflexión
                    var setMethod = typeof(DbContext).GetMethod("Set")?.MakeGenericMethod(entityType.ClrType);
                    if (setMethod != null)
                    {
                        var dbSet = setMethod.Invoke(_context, null) as IQueryable;
                        if (dbSet != null)
                        {
                            var data = await ConvertQueryToListAsync(dbSet, entityType.ClrType);
                            if (data.Count > 0)
                            {
                                allData[tableName] = data;
                            }
                        }
                    }
                }
                catch
                {
                    // Si hay error al obtener los datos de una tabla, continuar con las demás
                    continue;
                }
            }

            return allData;
        }

        public async Task<List<Dictionary<string, object?>>> GetTableDataAsync(string tableName)
        {
            var entityType = _context.Model.GetEntityTypes()
                .FirstOrDefault(e => (e.GetTableName() ?? e.Name).Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType == null)
            {
                return new List<Dictionary<string, object?>>();
            }

            var setMethod = typeof(DbContext).GetMethod("Set")?.MakeGenericMethod(entityType.ClrType);
            if (setMethod == null)
            {
                return new List<Dictionary<string, object?>>();
            }

            var dbSet = setMethod.Invoke(_context, null) as IQueryable;
            if (dbSet == null)
            {
                return new List<Dictionary<string, object?>>();
            }

            return await ConvertQueryToListAsync(dbSet, entityType.ClrType);
        }

        private async Task<List<Dictionary<string, object?>>> ConvertQueryToListAsync(IQueryable query, Type entityType)
        {
            // Usar ToListAsync con reflexión
            var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethod("ToListAsync", new[] { typeof(IQueryable) })?
                .MakeGenericMethod(entityType);

            List<object>? results = null;

            if (toListAsyncMethod != null)
            {
                var task = toListAsyncMethod.Invoke(null, new object[] { query }) as Task;
                if (task != null)
                {
                    await task.ConfigureAwait(false);
                    var resultProperty = task.GetType().GetProperty("Result");
                    results = resultProperty?.GetValue(task) as List<object>;
                }
            }

            var list = new List<Dictionary<string, object?>>();

            if (results == null)
            {
                return list;
            }

            foreach (var item in results)
            {
                var dict = new Dictionary<string, object?>();
                var properties = item.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    
                    // Evitar incluir colecciones complejas
                    if (IsSimpleType(property.PropertyType))
                    {
                        dict[property.Name] = value;
                    }
                }

                list.Add(dict);
            }

            return list;
        }

        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(Guid)
                || type == typeof(byte[])
                || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    && IsSimpleType(type.GetGenericArguments()[0]));
        }
    }
}

