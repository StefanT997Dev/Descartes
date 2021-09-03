using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Persistence;

namespace Infrastructure
{
    public class DiffRepository : IDiffRepository
    {
        private readonly DataContext _context;
        public DiffRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateDiffAsync(Diff diff)
        {
            _context.Diffs.Add(diff);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Diff> GetDiffAsync(int id)
        {
            return await _context.Diffs.FindAsync(id);
        }

        public async Task<bool> UpdateDiffAsync(Diff diff, string left)
        {
            diff.Left=left;

            return await _context.SaveChangesAsync()>0;
        }
    }
}