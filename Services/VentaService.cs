﻿

namespace ConcesionariaVehiculos.Services
{
    public class VentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, IClienteRepository clienteRepository, IVehiculoRepository vehiculoRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _clienteRepository = clienteRepository;
            _vehiculoRepository = vehiculoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            return await _ventaRepository.GetAllAsync();
        }

        public async Task<Venta?> GetByIdAsync(int id)
        {
            return await _ventaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Venta venta)
        {
            await _ventaRepository.AddAsync(venta);
        }

        public async Task UpdateAsync(Venta venta)
        {
            await _ventaRepository.UpdateAsync(venta);
        }

        /// <summary>
        /// Elimina una venta por su identificador.
        /// </summary>
        /// <param name="id">El identificador de la venta a eliminar.</param>
        public async Task DeleteAsync(int id)
        {
            await _ventaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<VentaDTO>> GetAllVentasByClienteId(int clienteId)
        {
            var ventas = await _ventaRepository.GetAllVentasByClienteId(clienteId);
            return ventas.Select(v => new VentaDTO
            {
                Id = v.Id,
                Fecha = v.Fecha,
                Total = v.Total,
                ClienteId = v.ClienteId
            });
        }

        internal async Task<FacturaVentaDTO?> GetFacturaByVentaIdAsync(int id)
        {
            var factura = await _ventaRepository.GetByIdAsync(id);
            if (factura == null)
            {
                return null;
            }

            // Obtengo el nombre del cliente y el vehículo asociado a la venta
            var cliente = _clienteRepository.GetByIdAsync(factura.ClienteId);
            var clienteNombre = cliente.Result != null ? cliente.Result.Nombre : "Cliente no encontrado";
            var vehiculo = _vehiculoRepository.GetByIdAsync(factura.VehiculoId);
            var vehiculoCaracteristicas = vehiculo.Result != null ?
                _mapper.Map<VehiculoCaracteristicasDTO>(vehiculo.Result) : null;


            return new FacturaVentaDTO
            {
                VentaId = factura.Id,
                ClienteNombre = clienteNombre,
                Vehiculo = vehiculoCaracteristicas,
                Total = factura.Total,
                Fecha = factura.Fecha
            };
        }

       
        /// El monto total de las ganancias de todas las ventas
        public async Task<decimal> GetGananciasSecuencialesAllTime()
        {
            // Obtengo todas las ventas y utilizando paralelismo calculo las ganancias
            var ventas = await _ventaRepository.GetAllAsync();
            if (ventas == null || !ventas.Any())
            {
                return 0;
            }
            //  AsParallel para realizar el cálculo de manera paralela
            // y luego sumo los totales de cada venta
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var ganancias = ventas.Select(v =>
            { return v.Total % 2 == 0 ? v.Total * 2m : v.Total * 0.5m; }
            ).Sum();
            stopwatch.Stop();
            Console.WriteLine($"Tiempo de ejecución (secuencial): {stopwatch.ElapsedMilliseconds} ms");
            return ganancias;
        }


            // Secuencial
            var swSec = System.Diagnostics.Stopwatch.StartNew();
            var resultadoSecuencial = ventas.Select(OperacionCostosa).Sum();
            swSec.Stop();

            // Paralelo
            var swPar = System.Diagnostics.Stopwatch.StartNew();
            var resultadoParalelo = ventas.AsParallel().Select(OperacionCostosa).Sum();
            swPar.Stop();

            Console.WriteLine($"Tiempo secuencial: {swSec.ElapsedMilliseconds} ms");
            Console.WriteLine($"Tiempo paralelo: {swPar.ElapsedMilliseconds} ms");

            return (swSec.ElapsedMilliseconds, swPar.ElapsedMilliseconds, resultadoSecuencial, resultadoParalelo);
        }

        // en paralela todas las ventas que superen un monto específico.
      
        public async Task<IEnumerable<Venta>> GetVentasByMontoParalelo(decimal monto)
        {
            var ventas = await _ventaRepository.GetAllAsync();
            if (ventas == null || !ventas.Any())
            {
                return Enumerable.Empty<Venta>();
            }

            // Utilizo AsParallel para filtrar las ventas que superen el monto especificado
            var ventasFiltradas = ventas.AsParallel().Where(v => v.Total > monto).ToList();

            return ventasFiltradas;
        }
    }