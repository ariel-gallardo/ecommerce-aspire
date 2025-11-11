# Lista de proyectos disponibles
$projects = @(
    "Cart",
    "Inventory",
    "Invoice",
    "Logs",
    "Notification",
    "Order",
    "Payment",
    "Product",
    "Security",
    "Shipping"
)

# Mostrar lista de proyectos y permitir seleccionar
Write-Host "Seleccione el proyecto para generar la migración:" -ForegroundColor Cyan
for ($i = 0; $i -lt $projects.Count; $i++) {
    Write-Host "$($i+1). $($projects[$i])"
}

# Leer y convertir a número
$projectIndex = Read-Host "Ingrese el número del proyecto"
if (-not [int]::TryParse($projectIndex, [ref]$projectIndex) -or $projectIndex -lt 1 -or $projectIndex -gt $projects.Count) {
    Write-Host "Selección inválida." -ForegroundColor Red
    exit
}

# Pedir nombre de la migración
$migrationName = Read-Host "Ingrese el nombre de la migración"
if ([string]::IsNullOrWhiteSpace($migrationName)) {
    Write-Host "Debes ingresar un nombre para la migración." -ForegroundColor Red
    exit
}

$selectedProject = $projects[$projectIndex - 1]

# Construir paths según el proyecto seleccionado
$projectPath = "$selectedProject/Infrastructure/$selectedProject.Infrastructure.Persistence"
$startupPath = "$selectedProject/$selectedProject.API"

# Ejecutar comando de migración
Write-Host "Generando migración '$migrationName' para el proyecto '$selectedProject'..." -ForegroundColor Green
dotnet ef migrations add $migrationName -p $projectPath -s $startupPath
