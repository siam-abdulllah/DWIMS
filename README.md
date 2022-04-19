# SHALIM_AHMED


1. Open project in VS Code.
2. In terminal run command - "cd api"
3. Then run command "dotnet watch run" to run api.
4. You can data from api using following link - "https://localhost:5001/api/post"

5. Open another terminal and run command - "cd client"
6. Then run "ng serve" to run client application.
7. Browse "https://localhost:4200/" to get the reports.

Manual Code first migration command - 
dotnet ef migrations add InithialCreate -p Infrastructure -s API -o Data/Migrations -c StoreContext
dotnet ef database update -p Infrastructure -s API  -c StoreContext

Test API:
https://localhost:5001/swagger/index.html


Add Migration DB:

dotnet ef migrations add InithialCreate -p Infrastructure -s API -o Data/Migrations  -c StoreContext

 

Update Database:

dotnet ef database update -p Infrastructure -s API -c StoreContext