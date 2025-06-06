
using ConcesionariaVehiculos.Models;

namespace ConcesionariaVehiculos.Repositories

{
    public interface IFacturaRepository
    {
        Task<List<Factura>> GetAllAsync();
        Task<Factura> GetByIdAsync(int id);
        Task AddAsync(Factura factura);
        Task UpdateAsync(Factura factura);
        Task DeleteAsync(int id);
    }

}