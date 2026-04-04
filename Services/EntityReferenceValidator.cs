using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;

namespace persist_net_backend.Services
{
    public class EntityReferenceValidator : IEntityReferenceValidator
    {
        private readonly AppDbContext _context;

        public EntityReferenceValidator(AppDbContext context)
        {
            _context = context;
        }

        public async Task EnsureExistsAsync<TEntity>(int id, string entityName) where TEntity : class
        {
            var exists = await _context.Set<TEntity>()
                .AnyAsync(entity => EF.Property<int>(entity, "Id") == id);

            if (!exists)
            {
                throw new EntityReferenceValidationException($"{entityName} con id {id} no existe.");
            }
        }
    }
}