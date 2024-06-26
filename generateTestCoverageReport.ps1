$servicePath = $Args[0]
if (!$servicePath) {
  Write-Error 'Please provide a back-end service path as argument'
  exit
}
Push-Location -Path $servicePath

$coveragePath = "./coverage"
if (Test-Path $coveragePath) {
  Remove-Item -Path $coveragePath -Force -Recurse
}

dotnet tool install --global dotnet-coverage
dotnet tool install --global dotnet-reportgenerator-globaltool

dotnet test -c Release --collect:"XPlat Code Coverage" --results-directory $coveragePath
dotnet-coverage merge "$($coveragePath)/*.cobertura.xml" --recursive --output "$($coveragePath)/coverage.merged.xml" --output-format cobertura
reportgenerator -reports:"$($coveragePath)/coverage.merged.xml" -targetdir:"$($coveragePath)/report" -reporttypes:Html

Invoke-Expression "$($coveragePath)/report/index.html"

Pop-Location