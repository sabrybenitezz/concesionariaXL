﻿
namespace ConcesionariaVehiculos.Services
{
    public class VehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;

        public VehiculoService(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<IEnumerable<Vehiculo>> GetAllAsync()
        {
            return await _vehiculoRepository.GetAllAsync();
        }

        public async Task<Vehiculo?> GetByIdAsync(int id)
        {
            return await _vehiculoRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Vehiculo vehiculo)
        {
            await _vehiculoRepository.AddAsync(vehiculo);
        }

        public async Task UpdateAsync(Vehiculo vehiculo)
        {
            var existingVehiculo = await _vehiculoRepository.GetByIdAsync(vehiculo.Id);
            if (existingVehiculo == null)
            {
                throw new KeyNotFoundException("Vehículo no encontrado");
            }
            await _vehiculoRepository.UpdateAsync(vehiculo);
        }

        public async Task DeleteAsync(int id)
        {
            await _vehiculoRepository.DeleteAsync(id);
        }

        public async Task<int> GetStockByIdAsync(int id)
        {
            var vehiculo = await _vehiculoRepository.GetByIdAsync(id);
            if (vehiculo == null)
            {
                throw new KeyNotFoundException("Vehículo no encontrado");
            }
            return vehiculo.Stock;
        }
    }