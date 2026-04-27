namespace Dsw2026Ej5.Views;

public class ConsoleView
{
    
    public static void DibujarMenu()
    {
        string? opcion = null;
        do
        {
            LimpiarPantalla();
            DibujarLinea();
            CentrarTexto("Menú Principal - Empresa de Transporte", out int _);
            DibujarLinea();
            Console.WriteLine("Elija una opción: \n");
            Console.WriteLine("1. Listar vehículos");
            Console.WriteLine("2. Agregar vehículo");
            Console.WriteLine("3. Salir");
            Console.WriteLine("\n");
            Console.WriteLine("Ingrese su opción: ");
            opcion = Console.ReadLine();
            if (opcion == "1")
            {
                Console.WriteLine("Listando vehículos...");
                ListarVehiculos();
            }
            else if (opcion == "2")
            {
                Console.WriteLine("Agregando vehículo...");
                AgregarVehiculo();
            }
        }
        while (opcion != "3");
    }


    private static void AgregarVehiculo()
    {
        LimpiarPantalla();
        DibujarLinea();
        CentrarTexto("Agregar Vehículo", out int _);
        DibujarLinea();

        // Tipo de vehículo
        Console.WriteLine("Tipo de vehículo:");
        Console.WriteLine("1. Eléctrico");
        Console.WriteLine("2. Combustible");
        Console.Write("Opción: ");
        string? tipoOpcion = Console.ReadLine();
        if (tipoOpcion != "1" && tipoOpcion != "2")
        {
            MostrarMensaje("Opción inválida. Volviendo al menú...");
            return;
        }

        // Datos comunes
        string patente = LeerTexto("Patente (ej: AA123BB): ");
        string marca = LeerTexto("Marca: ");
        string modelo = LeerTexto("Modelo: ");
        int anio = LeerEntero("Año: ", 1900, DateTime.Now.Year + 1);
        double capacidadCarga = LeerDecimal("Capacidad de carga (kg): ", 0);

        // Sucursal
        List<string> sucursales = Controlador.GetCodigosSucursales();
        Console.WriteLine("Sucursales disponibles: " + string.Join(", ", sucursales));
        string codigoSucursal = LeerTexto("Código de sucursal: ");

        (bool ok, string mensaje) resultado;

        if (tipoOpcion == "1")
        {
            // Eléctrico
            double kwhBase = LeerDecimal("Consumo base (kWh cada 100 km): ", 0);
            resultado = Controlador.AgregarVehiculoElectrico(patente, marca, modelo, anio, capacidadCarga, codigoSucursal, kwhBase);
        }
        else
        {
            // Combustible
            double kmPorLitro = LeerDecimal("Kilómetros por litro: ", 0.1);
            double litrosExtra = LeerDecimal("Litros extra: ", 0);
            resultado = Controlador.AgregarVehiculoCombustible(patente, marca, modelo, anio, capacidadCarga, codigoSucursal, kmPorLitro, litrosExtra);
        }

        MostrarMensaje(resultado.mensaje);
    }




    public static void CentrarTexto(string? texto, out int usado, int? ancho = null, bool salto = true)
    {
        texto ??= string.Empty;
        ancho ??= Console.WindowWidth;
        int largo = texto.Length;
        if (largo > ancho)
        {
            largo = ancho.Value;
            texto = texto.Substring(0, ancho.Value);
        }
        int espacios = (ancho.Value - largo) / 2;
        espacios = espacios % 2 == 0 ? espacios : espacios + 1;
        string fin = salto ? "\n" : string.Empty;
        string final = new string(' ', espacios) + texto + fin;
        Console.Write(final);
        usado = final.Length;
    }

    public static void LimpiarPantalla() => Console.Clear();

    public static void DibujarLinea()
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    private static void DibujarEncabezado(params string[] columnas)
    {
        DibujarLinea();
        int ancho = Console.WindowWidth / columnas.Length;
        foreach (var columna in columnas)
        {
            Console.Write("|");
            CentrarTexto(columna, out int l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));
        }
        Console.WriteLine("|");
        DibujarLinea();
    }


   
    private static void DibujarDatos(List<VehiculoViewModel> vehiculos, int cantColumnas)
    {
        int ancho = Console.WindowWidth / cantColumnas;
        foreach (var vehiculo in vehiculos)
        {
            Console.Write("|");
            CentrarTexto(vehiculo.GetPatente(), out int l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            Console.Write("|");
            CentrarTexto(vehiculo.GetVehiculo(), out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            Console.Write("|");
            CentrarTexto(vehiculo.GetTipo(), out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            Console.Write("|");
            CentrarTexto(vehiculo.GetCapacidadCarga().ToString("F0"), out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            // Para eléctricos mostraria kWhBase, para combustibles muestra km/l
            string rendimiento = vehiculo.GetKmPorLitro() > 0
                ? vehiculo.GetKmPorLitro().ToString("F1")
                : "-";
            Console.Write("|");
            CentrarTexto(rendimiento, out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            Console.Write("|");
            CentrarTexto(vehiculo.GetAnio().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            string litrosExtra = vehiculo.GetLitrosExtra() > 0
                ? vehiculo.GetLitrosExtra().ToString("F1")
                : "-";
            Console.Write("|");
            CentrarTexto(litrosExtra, out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            Console.Write("|");
            CentrarTexto(vehiculo.GetKmARecorrer().ToString("F0"), out l, ancho - 1, false);
            Console.Write("".PadRight(Math.Max(0, ancho - 1 - l)));

            // salto de línea al final de cada fila
            Console.WriteLine("|");
        }
    }

    private static void ListarVehiculos()
    {
        // recargar la lista en cada llamada para incluir vehículos recién agregados
        List<VehiculoViewModel> vehiculos = Controlador.GetVehiculos();

        LimpiarPantalla();

        //  permitir editar los km a recorrer de cada vehículo
        Console.WriteLine("Ingrese los km a recorrer por cada vehículo (Enter para usar 100 km):\n");
        foreach (VehiculoViewModel v in vehiculos)
        {
            Console.Write($"  {v.GetPatente()} - {v.GetVehiculo()} [{v.GetTipo()}] km a recorrer (default 100): ");
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double km) && km > 0)
                v.SetKmARecorrer(km);
        }

        LimpiarPantalla();
        string[] columnas = { "Patente", "Vehículo", "Tipo", "Cap.Carga", "Km/l o kWh", "Año", "L.Extra", "Km recorrer" };
        DibujarEncabezado(columnas);
        DibujarDatos(vehiculos, columnas.Length);
        DibujarLinea();
        Console.Write("\n\n");
        Console.WriteLine("Presione Enter para calcular el total de consumos...");
        Console.ReadLine();

        // Armar diccionario patente -> km
        Dictionary<string, double> kmPorVehiculo = new Dictionary<string, double>();
        foreach (VehiculoViewModel v in vehiculos)
            kmPorVehiculo.Add(v.GetPatente(), v.GetKmARecorrer());

        (double consumoElectrico, double consumoCombustible) = Controlador.CalcularConsumos(kmPorVehiculo);

        DibujarLinea();
        Console.WriteLine($"Total consumo Vehículos Eléctricos : {consumoElectrico:F2} kWh");
        Console.WriteLine($"Total consumo Vehículos Combustible: {consumoCombustible:F2} Litros");
        DibujarLinea();
        Console.Write("\n\n");
        Console.WriteLine("Presione Enter para volver al menú...");
        Console.ReadLine();
    }


    private static string LeerTexto(string prompt)
    {
        string? valor;
        do
        {
            Console.Write(prompt);
            valor = Console.ReadLine()?.Trim();
        } while (string.IsNullOrEmpty(valor));
        return valor;
    }

    private static int LeerEntero(string prompt, int min, int max)
    {
        int resultado;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out resultado) && resultado >= min && resultado <= max)
                return resultado;
            Console.WriteLine($"  Ingrese un número entre {min} y {max}.");
        }
    }

    private static double LeerDecimal(string prompt, double min)
    {
        double resultado;
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out resultado) && resultado >= min)
                return resultado;
            Console.WriteLine($"  Ingrese un número mayor o igual a {min}.");
        }
    }

    private static void MostrarMensaje(string mensaje)
    {
        Console.WriteLine($"\n{mensaje}");
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

}
