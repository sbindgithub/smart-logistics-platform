dotnet new xunit -n SmartLogistics.Application.Tests
dotnet sln add SmartLogistics.Application.Tests/SmartLogistics.Application.Tests.csproj
dotnet add SmartLogistics.Application.Tests reference src/SmartLogistics.Application

dotnet add SmartLogistics.Application.Tests package Moq
dotnet add SmartLogistics.Application.Tests package Microsoft.NET.Test.Sdk

dotnet test


dotnet test tests/SmartLogistics.IntegrationTests --no-build

Run API:
dotnet run --project src/SmartLogistics.API --launch-profile http

Run prometheus:
docker run -d --name prometheus -p 9090:9090 -v D:\SaradaOwnFiles\Training_Learning\GIT\smart-logistics-platform\smart-logistics-platform\src\SmartLogistics.Observability\prometheus.yml:/etc/prometheus/prometheus.yml prom/prometheus

Stop Prometheus:
docker rm -f prometheus

verify removal of Prometheus container:
docker ps -a | findstr prometheus

Run Grafana:
	
docker restart prometheus

http://localhost:5000/metrics

prometheus URL:
http://localhost:9090/targets

Grafana URL:
http://localhost:3000

