using System;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IDiffRepository
    {
        Task<Diff> GetDiffAsync(int id);    
        Task<Boolean> CreateDiffAsync(Diff diff);
        Task<Boolean> UpdateDiffAsync(Diff diff,string left);
    }
}