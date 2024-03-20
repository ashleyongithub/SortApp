using Microsoft.EntityFrameworkCore;

namespace SortApp.Web.Data
{
    public class SortAppWebContext : DbContext
    {
        public SortAppWebContext (DbContextOptions<SortAppWebContext> options)
            : base(options)
        {
        }

        public DbSet<SortApp.Web.Models.SortResult> SortResult { get; set; } = default!;
    }
}
