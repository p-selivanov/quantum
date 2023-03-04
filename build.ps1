Write-Host "> dotnet build:"
dotnet build .

Write-Host "> ZIP StreamConsumerLambda:"
Compress-Archive -Path ./src/Quantum.Customer.StreamConsumerLambda/bin/Debug/net6.0/* -DestinationPath ./src/Quantum.Customer.StreamConsumerLambda/bin/function.zip -Force
Write-Host "OK"