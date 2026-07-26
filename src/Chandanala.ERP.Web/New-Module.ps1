param(
    [Parameter(Mandatory = $true)]
    [string]$ModuleName
)

$ProjectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$ModulesRoot = Join-Path $ProjectRoot "Modules"

if (!(Test-Path $ModulesRoot)) {
    Write-Host "Modules folder not found: $ModulesRoot" -ForegroundColor Red
    exit 1
}

$ModuleRoot = Join-Path $ModulesRoot $ModuleName

$Folders = @(
    "Application",
    "Domain",
    "Infrastructure",
    "Presentation",
    "Contracts",
    "Specifications",
    "Policies",
    "Workflows",
    "OmniRoute",
    "Intelligence",
    "Reports",
    "Tests",
    "Docs"
)

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "CHANDANALA CORE Module Generator" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

if (!(Test-Path $ModuleRoot)) {
    New-Item -ItemType Directory -Path $ModuleRoot | Out-Null
    Write-Host "[CREATED] Module : $ModuleName" -ForegroundColor Yellow
}
else {
    Write-Host "[EXISTS ] Module : $ModuleName" -ForegroundColor Green
}

foreach ($Folder in $Folders) {

    $Path = Join-Path $ModuleRoot $Folder

    if (Test-Path $Path) {
        Write-Host "[OK]       $Folder" -ForegroundColor Green
    }
    else {
        New-Item -ItemType Directory -Path $Path | Out-Null
        Write-Host "[CREATED]  $Folder" -ForegroundColor Yellow
    }
}

$Readme = Join-Path $ModuleRoot "README.md"

if (!(Test-Path $Readme)) {

@"
# $ModuleName Module

Generated automatically.

"@ | Set-Content $Readme
}

Write-Host ""
Write-Host "Module generation completed." -ForegroundColor Cyan
