using Microsoft.EntityFrameworkCore;


namespace service.indumepi.Infra.Data
{
    public class Repository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
