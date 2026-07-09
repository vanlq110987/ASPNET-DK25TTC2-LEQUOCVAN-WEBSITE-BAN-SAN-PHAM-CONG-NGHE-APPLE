# ============================================================
# download-offline-installers.ps1
# Tải bộ cài OFFLINE dung lượng lớn (không kèm sẵn trong repo
# vì vượt giới hạn 100MB/file của GitHub).
# Cách chạy:  mở PowerShell tại thư mục soft\ rồi gõ:
#   powershell -ExecutionPolicy Bypass -File .\download-offline-installers.ps1
# ============================================================

$ErrorActionPreference = "Stop"
$here = Split-Path -Parent $MyInvocation.MyCommand.Path

function Download-File($url, $outName) {
    $out = Join-Path $here $outName
    if (Test-Path $out) {
        Write-Host "[Bo qua] $outName da ton tai." -ForegroundColor Yellow
        return
    }
    Write-Host "[Dang tai] $outName ..." -ForegroundColor Cyan
    # curl.exe có sẵn trên Windows 10+, tự theo redirect (-L)
    & curl.exe -L --progress-bar -o $out $url
    Write-Host "[Xong] $outName" -ForegroundColor Green
}

# 1. SQL Server Management Studio (SSMS) — ~700MB
Download-File "https://aka.ms/ssmsfullsetup" "SSMS-Setup-ENU.exe"

# 2. .NET Framework 4.8 Developer Pack — ~120MB (chỉ cần nếu KHÔNG cài
#    workload "ASP.NET and web development" của Visual Studio)
Download-File "https://go.microsoft.com/fwlink/?linkid=2088517" "ndp48-devpack.exe"

Write-Host ""
Write-Host "Hoan tat! Cai dat theo thu tu trong soft/README.md" -ForegroundColor Green
