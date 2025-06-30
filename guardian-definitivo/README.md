# Guardián Definitivo

**Visión del Proyecto:** Crear una aplicación multiplataforma potente, modular y visualmente atractiva que conecte con la API oficial de Bungie.net para extender las capacidades del juego Destiny 2.

Este proyecto está siendo desarrollado con la asistencia de una IA de codificación.

## Configuración para Desarrollo

Para ejecutar la aplicación localmente, necesitarás configurar tus credenciales de la API de Bungie.net.

1.  **Archivo de Configuración Principal (`guardian-definitivo/src/AppConfig.json`):**
    *   Este archivo contiene la `BungieApiKey` y el `OAuthClientId`.
    *   Puedes obtener estos valores registrando tu aplicación en [Bungie.net Developer Portal](https://www.bungie.net/en/Application).
    *   El `OAuthClientSecret` en este archivo es un placeholder. **No coloques tu secret real aquí si tu repositorio es público.**

2.  **Configuración del `OAuthClientSecret`:**
    El `OAuthClientSecret` es una credencial sensible y debe manejarse con cuidado. La aplicación lo cargará en el siguiente orden de prioridad:

    *   **Opción 1: Variable de Entorno (Recomendado)**
        *   Define una variable de entorno llamada `GUARDIAN_DEFINITIVO_CLIENT_SECRET` con tu OAuth Client Secret.
        *   Ejemplo (Linux/macOS): `export GUARDIAN_DEFINITIVO_CLIENT_SECRET="tu_secret_aqui"`
        *   Ejemplo (Windows PowerShell): `$env:GUARDIAN_DEFINITIVO_CLIENT_SECRET="tu_secret_aqui"`

    *   **Opción 2: Archivo `AppSecrets.json` (Local)**
        *   Crea un archivo llamado `AppSecrets.json` dentro del directorio `guardian-definitivo/src/`.
        *   Este archivo **está incluido en `.gitignore`** y no debe ser subido al repositorio.
        *   Contenido de `guardian-definitivo/src/AppSecrets.json`:
            ```json
            {
              "OAuthClientSecret": "tu_secret_real_aqui"
            }
            ```

    *   **Opción 3: Placeholder en `AppConfig.json` (No seguro para secrets reales en repos públicos)**
        *   Si el secret no se encuentra en las opciones anteriores, la aplicación usará el valor de `OAuthClientSecret` de `AppConfig.json`.
        *   Por defecto, este es `"SECRET_CONFIGURADO_EXTERNAMENTE_VER_README"`, lo que causará fallos en la autenticación OAuth de tipo "Confidencial".

**Importante para la API de Bungie:**
*   Asegúrate de que la **URL de redirección OAuth** en tu configuración de aplicación de Bungie.net coincida con la que usas. Para la ejecución actual en consola, el valor exacto no es crítico ya que copiamos el código manualmente, pero para una aplicación real sí lo es. El `BungieOAuthHandler.cs` usa `https://localhost:8000/auth/callback` como ejemplo en comentarios, pero la aplicación de consola no la implementa.
*   Para el tipo de cliente OAuth "Confidencial", el `client_secret` es necesario para intercambiar el código de autorización por un token de acceso.

## Stack Tecnológico
*   **Lenguaje:** C# 12
*   **Framework:** .NET 9.0 SDK (o el SDK de .NET que esté disponible y sea compatible)
*   **Herramientas:** Visual Studio Code
*   **APIs Externas:** Bungie.net REST API

## Flujo Principal de Usuario (MVP Inicial)
1.  Login con Bungie.net (simulado en consola).
2.  Obtener Perfil del Usuario.
3.  Cargar Inventario Básico (trabajo en progreso).

---

*Este README se irá actualizando a medida que el proyecto avance.*
