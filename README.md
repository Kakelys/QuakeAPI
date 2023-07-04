How to start? 
===

1. clone repo
```
git clone https://github.com/Kakelys/QuakeAPI.git
```
2. go to cloned repo
3. go to API/QuakeAPI
```
    cd API/QuakeAPI
```
4. start sql server and create database from migrations using CLI:
```
    dotnet ef database update
```
if dotnet ef not istalled: 
```
    dotnet tool install --global dotnet-ef
```
if you have specific database name or several servers change it in appsettings

5. start: 
  ```
  dotnet run
  ```