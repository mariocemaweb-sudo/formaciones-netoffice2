@echo off
echo ============================================
echo  PUBLICACION DE PLATAFORMA DE FORMACIONES
echo ============================================
echo.

REM Verificar si dotnet está instalado
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK no encontrado
    echo Por favor, instala .NET 8.0 SDK desde:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo Restaurando paquetes NuGet...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Fallo al restaurar paquetes
    pause
    exit /b 1
)

echo.
echo Compilando aplicacion en modo Release...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo ERROR: Fallo la compilacion
    pause
    exit /b 1
)

echo.
echo Publicando aplicacion...
dotnet publish -c Release -o ./publish
if %errorlevel% neq 0 (
    echo ERROR: Fallo la publicacion
    pause
    exit /b 1
)

echo.
echo ============================================
echo  PUBLICACION COMPLETADA EXITOSAMENTE
echo ============================================
echo.
echo La carpeta 'publish' contiene todos los archivos necesarios.
echo.
echo PROXIMOS PASOS:
echo 1. Copia la carpeta 'publish' a tu servidor IIS
echo 2. Configura un nuevo sitio web en IIS apuntando a esta carpeta
echo 3. Asegurate de tener instalado: .NET 8.0 Runtime - ASP.NET Core Hosting Bundle
echo 4. Configura los permisos de IIS_IUSRS en la carpeta
echo 5. Accede a la aplicacion y usa: admin@formaciones.com / Admin123!
echo.
echo Consulta README.md para instrucciones detalladas.
echo.
pause
