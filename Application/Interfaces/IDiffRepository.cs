using System;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IDiffRepository
    {
        Task<Diff> GetDiffAsync(int id);    
        Task<bool> CreateDiffAsync(Diff diff);
        Task<bool> UpdateLeftDiffAsync(Diff diff,string left);
        Task<bool> UpdateRightDiffAsync(Diff diff, string right);
    }
}