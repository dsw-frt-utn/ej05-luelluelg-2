using Dsw2026Ej5.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej5.Views;

public class VehiculoViewModel
{
    private string patente = string.Empty;
    private string? vehiculo;
    private string? tipo;
    private string? sucursal;
    private double capacidadCarga;
    private double kmPorLitro;
    private int anio;
    private double litrosExtra;
    private double kmARecorrer;

    public VehiculoViewModel(Vehiculo vehiculo)
    {
        if (vehiculo == null) return;
        this.patente = vehiculo.GetPatente();
        this.vehiculo = vehiculo.ToString();
        this.tipo = vehiculo.GetTipo().ToString();
        this.sucursal = vehiculo.GetSucursal().GetCodigo();
        this.capacidadCarga = vehiculo.GetCapacidadCarga();
        this.anio = vehiculo.GetAnio();
        this.kmPorLitro = vehiculo is VehiculoCombustible combustible ? combustible.GetKilometrosPorLitro() : 0;
        this.litrosExtra = vehiculo is VehiculoCombustible combustible1 ? combustible1.GetLitrosExtra() : 0;
        this.kmARecorrer = 100;
    }

    public string GetPatente()
    {
        return patente;
    }

    public string? GetVehiculo()
    {
        return vehiculo;
    }

    public string? GetTipo()
    {
        return tipo;
    }

    public string? GetSucursal()
    {
        return sucursal;
    }

    public double GetCapacidadCarga()
    {
        return capacidadCarga;
    }

    public double GetKmPorLitro()
    {
        return kmPorLitro;
    }

    public int GetAnio()
    {
        return anio;
    }

    public double GetLitrosExtra()
    {
        return litrosExtra;
    }

    public double GetKmARecorrer()
    {
        return kmARecorrer;
    }


    public void SetKmARecorrer(double km)
    {
        kmARecorrer = km;
    }
}
