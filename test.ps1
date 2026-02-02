dotnet new xunit -n SmartLogistics.Application.Tests
dotnet sln add SmartLogistics.Application.Tests/SmartLogistics.Application.Tests.csproj
dotnet add SmartLogistics.Application.Tests reference src/SmartLogistics.Application

dotnet add SmartLogistics.Application.Tests package Moq
dotnet add SmartLogistics.Application.Tests package Microsoft.NET.Test.Sdk

dotnet test
