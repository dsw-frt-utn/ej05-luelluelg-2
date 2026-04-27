using Dsw2026Ej5.Data;
using Dsw2026Ej5.Domain;

namespace Dsw2026Ej5.Views;

public class Controlador
{
    public static List<VehiculoViewModel> GetVehiculos()
    {
        List<VehiculoViewModel> vehiculos = new List<VehiculoViewModel>();
        foreach (Vehiculo vehiculo in Persistencia.GetVehiculos())
        {
            vehiculos.Add(new VehiculoViewModel(vehiculo));
        }
        return vehiculos;
    }

    public static (double, double) CalcularConsumos(Dictionary<string, double> vehiculos)
    {
        double consumoElectricos = 0;
        double consumoCombustible = 0;
        foreach (KeyValuePair<string, double> entry in vehiculos)
        {
            double consumo = 0;
            Vehiculo? vehiculo = Persistencia.GetVehiculo(entry.Key);
            if (vehiculo != null)
            {
                consumo = vehiculo.CalcularConsumo(entry.Value);
                consumoElectricos += vehiculo.EsDe(VehiculoTipo.Electrico) ? consumo : 0;
                consumoCombustible += vehiculo.EsDe(VehiculoTipo.Combustible) ? consumo : 0;
            }
        }
        return (consumoElectricos, consumoCombustible);
    }


    public static (bool ok, string mensaje) AgregarVehiculoElectrico(
      string patente, string marca, string modelo, int anio,
      double capacidadCarga, string codigoSucursal, double kwhBase)
    {
        Sucursal? sucursal = Persistencia.GetSucursales().Find(s => s.GetCodigo() == codigoSucursal);
        if (sucursal == null)
            return (false, "Sucursal no encontrada.");

        VehiculoElectrico v = new VehiculoElectrico(patente, marca, modelo, anio, capacidadCarga, sucursal, kwhBase);
        bool agregado = Persistencia.AgregarVehiculo(v);
        return agregado
            ? (true, "Vehículo eléctrico agregado correctamente.")
            : (false, "Ya existe un vehículo con esa patente.");
    }

    // método para agregar vehículo a combustible
    public static (bool ok, string mensaje) AgregarVehiculoCombustible(
        string patente, string marca, string modelo, int anio,
        double capacidadCarga, string codigoSucursal,
        double kmPorLitro, double litrosExtra)
    {
        Sucursal? sucursal = Persistencia.GetSucursales().Find(s => s.GetCodigo() == codigoSucursal);
        if (sucursal == null)
            return (false, "Sucursal no encontrada.");

        VehiculoCombustible v = new VehiculoCombustible(patente, marca, modelo, anio, capacidadCarga, sucursal, kmPorLitro, litrosExtra);
        bool agregado = Persistencia.AgregarVehiculo(v);
        return agregado
            ? (true, "Vehículo a combustible agregado correctamente.")
            : (false, "Ya existe un vehículo con esa patente.");
    }

    public static List<string> GetCodigosSucursales()
    {
        return Persistencia.GetSucursales().Select(s => s.GetCodigo()).ToList();
    }


}
