using System;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Json;

public class Program
{
    public static async Task Main()
    {
        string rutaCarpeta = @"C:\Users\valsisdes1\Source\repos\HilosFast\HilosFast\NOTA";
        if (!Directory.Exists(rutaCarpeta))
        {
            Directory.CreateDirectory(rutaCarpeta);
        }
        int contador = 0;
        while (contador < 5) 
        {
            var task1 = Task.Run(() => Proceso1());
            var task2 = Task.Run(() => Proceso2());
            var tasks = new[] { task1, task2 };

            // Esperar a que cualquiera de las tareas termine
     
            Task tareaTerminada = await Task.WhenAny(tasks);

            string resultado = tareaTerminada == task1 ? "Proceso 1 terminó primero." : "Proceso 2 terminó primero.";

            // Escribir el resultado en un archivo .txt
            string rutaArchivo = Path.Combine(rutaCarpeta, "notaprocesos.txt");

            File.AppendAllText(rutaArchivo, resultado + Environment.NewLine);

            Console.WriteLine("Resultado guardado en " + rutaArchivo);

            contador++;
        }
    }


    public static void Proceso1()
    {
        Console.WriteLine("Proceso 1 iniciado en 2 segundos.");
        Task.Delay(2000).Wait();

        RealizarSolicitudHTTP();
        //Console.WriteLine("Proceso 1 finalizado ");

    }

    public static void RealizarSolicitudHTTP()
    {
        string url = "https://webq.odm.com.mx/WSVentaWeb/api/RecuperaCorridas";

        using (HttpClient client = new HttpClient())
        {
            var postData = new
            {
                pE_aDestino = "SWN",
                pE_aFecha = "31/10/2023",
                pE_aOrigen = "DGO",
                pE_aViajeRedondo = "V1",
                pE_nAdultos = 4,
                pE_nEstudiantes = 0,

                pE_nMaestro = 0,
                pE_nModo = 1,
                pE_nNinos = 0,
                pS_nActualizaPasajeros = 1,
                pS_nConsecutivo = 72530429
            };

            HttpResponseMessage response = client.PostAsJsonAsync(url, postData).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("soy el proceso 1" + jsonResponse);
            }
            else
            {
                Console.WriteLine("Error en la solicitud HTTP: " + response.StatusCode);
            }
        }
    }

    public static void Proceso2()
    {
        Console.WriteLine("Proceso 2 inicia en 2 segundos iniciado.");

        Task.Delay(2000).Wait();
        RealizarCrearSesion();
        //Console.WriteLine("Finaliza Proceso 2 finalizado.");
    }


    public static void RealizarCrearSesion()
    {
        string url = "https://webq.odm.com.mx/WSVentaWeb/api/RecuperaCorridas";

        using (HttpClient client = new HttpClient())
        {
            var postData = new
            {
                pE_aDestino = "SWN",
                pE_aFecha = "30/10/2023",
                pE_aOrigen = "DGO",
                pE_aViajeRedondo = "V1",
                pE_nAdultos = 4,
                pE_nEstudiantes = 0,
                pE_nInsen = 0,
                pE_nMaestro = 0,
                pE_nModo = 1,
                pE_nNinos = 0,
                pS_nActualizaPasajeros = 1,
                pS_nConsecutivo = 72530429
            };

            HttpResponseMessage response = client.PostAsJsonAsync(url, postData).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("soy del proceso 2" + jsonResponse);
            }
            else
            {
                Console.WriteLine("Error en la solicitud HTTP: " + response.StatusCode);
            }
        }
    }
   
    //public static void Proceso3()
    //{
    //    Console.WriteLine("Proceso 3 iniciado.");
    //    Thread.Sleep(1500);
    //    Console.WriteLine("Proceso 3 finalizado.");
    //}
}
