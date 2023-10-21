# Seting up the Developer environment  

## Requirements  
- Docker  
- Azure Data Studio or MSSQL Server Management  
- Visual Studio Comunity 2022  
- .NET >= 6.0.20  
- Github CLI  
- Git CLI  
- EntityFrameworkCore Tools 6.0.20 (NuGet package or CLI client)  
- Thunder client or Postman (or any Rest Client for Testing APIs)  

## Steps
1. Install all the [requirements](#requirements)  
- Pull and run the MSSQL container  
- Clone the repo



Docker
```
docker pull mcr.microsoft.com/mssql/server:2017-CU31-ubuntu-18.04  
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-CU31-ubuntu-18.04
```

EntityFrameworkCore Tools
dotnet tool install --global dotnet-ef --version 6.0.20

Visual Studio

`secrets.json`
```  
{
    "ASPNETCORE_DBCON": "Data Source=localhost;Database=ALT;User id=sa;Password=yourStrong(!)Password; TrustServerCertificate=true"
}
``````
