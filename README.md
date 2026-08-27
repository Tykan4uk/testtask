# testtask

## Local setup

1. Start the database from Docker (Tools -- Command line -- Developer Command Promt):
```
docker complose up -d
```

2. Install dotnet ef using the following command:
```
dotnet tool install --global dotnet-ef
```

3. Create a local database from migrations with the following command:
```
dotnet ef database update --project src\Api
```
