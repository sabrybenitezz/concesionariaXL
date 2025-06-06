﻿

namespace ConcesionariaVehiculos.Services

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly VentaService _ventaService;

    public ClienteService(IClienteRepository clienteRepository, VentaService ventaService)
    {
        _clienteRepository = clienteRepository;
        _ventaService = ventaService;
    }

    public Task<IEnumerable<Cliente>> GetAllClientesAsync()
    {
        return _clienteRepository.GetAllAsync();
    }

    public Task<Cliente?> GetClienteByIdAsync(int id)
    {
        return _clienteRepository.GetByIdAsync(id);
    }

    public async Task<Cliente> AddClienteAsync(Cliente cliente)
    {
        await _clienteRepository.AddAsync(cliente);
        return cliente;
    }

    public async Task<Cliente> UpdateClienteAsync(Cliente cliente)
    {
        await _clienteRepository.UpdateAsync(cliente);
        return cliente;
    }

    public async Task<bool> DeleteClienteAsync(int id)
    {
        var existingCliente = await _clienteRepository.GetByIdAsync(id);
        if (existingCliente == null) return false;

        await _clienteRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> DeleteClienteAsync(Cliente cliente)
    {
        var existingCliente = await _clienteRepository.GetByIdAsync(cliente.Id);
        if (existingCliente == null) return false;

        await _clienteRepository.DeleteAsync(cliente.Id);
        return true;
    }

    public async Task<IEnumerable<VentaDTO>> GetAllCompras(int clienteId)
    {
        return await _ventaService.GetAllVentasByClienteId(clienteId);
    }
}