Write-Host "> docker build AccountSearch.Api:"
docker build ../ -f ../src/Quantum.AccountSearch.Api/Dockerfile -t quantum-account-search-api:1.0.1