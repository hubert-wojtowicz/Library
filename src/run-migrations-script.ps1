cd .\src\Library.Infrastructure\

dotnet ef migrations add InitialSql -s ..\Library.Api\Library.Api.csproj -p .\Library.Infrastructure.csproj -o .\Database\Migrations

dotnet ef database update -p Library.Infrastructure -s KryptoKickStake.Api
