// Archivo de entrada principal para la aplicación Guardián Definitivo
using System;
using System.Net.Http;
using System.Threading.Tasks;
using GuardianDefinitivo.Data;
using GuardianDefinitivo.Models;

public class Program
{
    // HttpClient debe ser instanciado una vez y reutilizado
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main(string[] args)
    {
        Console.WriteLine("¡Bienvenido a Guardián Definitivo!");

        // 1. Cargar la configuración de la aplicación
        Console.WriteLine("[Main] Cargando configuración...");
        var authConfig = BungieAuthConfig.Load();
        if (authConfig == null || string.IsNullOrEmpty(authConfig.BungieApiKey) || string.IsNullOrEmpty(authConfig.OAuthClientId))
        {
            Console.WriteLine("[Main] Error: No se pudo cargar la configuración o falta API Key/Client ID. Verifica AppConfig.json.");
            Console.WriteLine("[Main] Asegúrate de reemplazar 'TU_API_KEY_AQUI', 'TU_CLIENT_ID_AQUI', etc., en guardian-definitivo/src/AppConfig.json");
            return;
        }
        Console.WriteLine("[Main] Configuración cargada.");

        // 2. Instanciar BungieOAuthHandler y BungieApiClient
        var oAuthHandler = new BungieOAuthHandler(authConfig, httpClient);
        var apiClient = new BungieApiClient(httpClient, authConfig, oAuthHandler);

        // 3. Simular flujo de autenticación OAuth
        string? accessToken = await oAuthHandler.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("[Main] Iniciando autenticación OAuth...");
            string authUrl = oAuthHandler.GetAuthorizationUrl();
            Console.WriteLine($"[Main] Por favor, visita esta URL en tu navegador para autorizar la aplicación:");
            Console.WriteLine(authUrl);
            Console.WriteLine();
            Console.WriteLine("[Main] Después de autorizar, Bungie te redirigirá a una URL (probablemente localhost).");
            Console.WriteLine("[Main] Copia el valor del parámetro 'code' de esa URL de redirección y pégalo aquí:");
            Console.Write("Introduce el código de autorización: ");
            string? authorizationCode = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                Console.WriteLine("[Main] No se introdujo ningún código. Saliendo.");
                return;
            }

            // Aquí también necesitaríamos el parámetro 'state' si lo estuviéramos validando seriamente.
            // Por simplicidad en este ejemplo de consola, lo omitimos, pero en una app real es crucial.
            bool tokenObtained = await oAuthHandler.HandleCallbackAsync(authorizationCode.Trim(), "dummy_state_for_console_app");

            if (!tokenObtained)
            {
                Console.WriteLine("[Main] No se pudo obtener el token de acceso. Verifica el código o la configuración.");
                return;
            }
            Console.WriteLine("[Main] Token de acceso obtenido con éxito.");
        }
        else
        {
            Console.WriteLine("[Main] Usando token de acceso existente/refrescado.");
        }

        // 4. Obtener y mostrar información del perfil
        Console.WriteLine("[Main] Obteniendo datos del perfil de Bungie.net...");
        var membershipData = await apiClient.GetCurrentUserMembershipDataAsync();

        if (membershipData != null)
        {
            Console.WriteLine($"[Main] ¡Hola, {membershipData.bungieNetUser?.displayName} (ID: {membershipData.bungieNetUser?.membershipId})!");
            if (membershipData.destinyMemberships != null && membershipData.destinyMemberships.Count > 0)
            {
                Console.WriteLine("[Main] Perfiles de Destiny 2 vinculados:");
                foreach (var profile in membershipData.destinyMemberships)
                {
                    Console.WriteLine($"  - Plataforma: {profile.membershipType} (ID: {profile.membershipId})");
                    Console.WriteLine($"    Nombre Global: {profile.bungieGlobalDisplayName}#{profile.bungieGlobalDisplayNameCode}");
                    Console.WriteLine($"    Nombre en Plataforma: {profile.displayName}");
                }

                if (membershipData.primaryMembershipId.HasValue)
                {
                    Console.WriteLine($"[Main] ID de membresía primaria: {membershipData.primaryMembershipId.Value}");
                }
            }
            else
            {
                Console.WriteLine("[Main] No se encontraron perfiles de Destiny 2 vinculados.");
            }
        }
        else
        {
            Console.WriteLine("[Main] No se pudo obtener la información del perfil.");
        }

        Console.WriteLine("[Main] Fin de la demostración. Presiona cualquier tecla para salir.");
        Console.ReadKey();
    }
}
