param(
    [switch]$OpenReport,
    [switch]$SkipTests
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$testProject = Join-Path $repoRoot "PN_InterviewTaskProject\PN_InterviewTaskProject.csproj"
$resultsDir = Join-Path $repoRoot "PN_InterviewTaskProject\bin\Debug\net8.0\allure-results"
$reportDir = Join-Path $repoRoot "allure-report"

if (-not (Get-Command allure -ErrorAction SilentlyContinue)) {
    throw "Allure CLI is not installed. Install it first: npm install -g allure-commandline --save-dev"
}

if (-not $SkipTests) {
    if (Test-Path $resultsDir) {
        Remove-Item -Path $resultsDir -Recurse -Force
    }

    dotnet test $testProject
}

if (-not (Test-Path $resultsDir)) {
    throw "No Allure results found in '$resultsDir'. Run tests first."
}

if (Test-Path $reportDir) {
    Remove-Item -Path $reportDir -Recurse -Force
}

allure generate $resultsDir --clean -o $reportDir

if ($OpenReport) {
    allure open $reportDir
}
else {
    Write-Host "Allure report generated at: $reportDir"
}
