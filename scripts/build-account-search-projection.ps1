Write-Host "> docker build AccountSearch.Projection:"
docker build ../ -f ../src/AccountSearch.Projection/Dockerfile -t quantum-account-search-projection:1.0.1