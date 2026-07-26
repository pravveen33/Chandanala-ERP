$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host " CHANDANALA CORE Build Pipeline" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "[1/3] Cleaning..." -ForegroundColor Yellow
dotnet clean

Write-Host ""
Write-Host "[2/3] Restoring..." -ForegroundColor Yellow
dotnet restore

Write-Host ""
Write-Host "[3/3] Building..." -ForegroundColor Yellow

dotnet build --no-restore 2>&1 | Tee-Object build.log

Write-Host ""
Write-Host "Build finished." -ForegroundColor Green
Write-Host "Log saved to build.log" -ForegroundColor Cyan
