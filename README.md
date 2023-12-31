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

5. start mongodb 
6. use ngrok(or something else) to create tunnel to your server
```
    ngrok http port
```
copy link to appsettings.json: 
```
WebhookUrl: "your_link"
```

7. start: 
  ```
  dotnet run
  ```


Database scheme: 

<picture>
 <img alt="db scheme" src="db_scheme.png">
</picture>