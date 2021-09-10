using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> UpdateLeftDiffAsync(Diff diff, string left)
        {
            diff.Left=left;

            await _context.SaveChangesAsync();

            return _context.Entry(diff).State != EntityState.Unchanged;
        }

         public async Task<bool> UpdateRightDiffAsync(Diff diff, string right)
        {
            diff.Right=right;

            await _context.SaveChangesAsync();

            return _context.Entry(diff).State != EntityState.Unchanged;
        }
    }
}