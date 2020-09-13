# ParperaEngineering

Parpera Engineering submission.

## Getting started

1. clone the repository

<code>git clone https://github.com/TomHamer/ParperaEngineering</code>

2. install ef tools 

From the root of the project directory (within the ParperaEngineeringTest project)

<code>dotnet tool install --global dotnet-ef</code>

3. migrate the database

<code>dotnet ef migrations add InitialCreate</code>

<code>dotnet ef database update # you should see a file called "Transactions.db" appear in the directory</code>

4. Disable Authorization

Comment out "[Authorize]" on line 13 of Controllers/TransactionController.cs to disable authorization.

5. Recommended: run the project using VS Studio.

## Notes

- The unit tests call the DB using memory
- Status must be either Completed, Cancelled or Pending
- An acceptable json blob for a status update is {"Status": "Pending"}
- the required data is loaded into the database automatically on startup

Example curl call for status update:

<code>curl -H "Content-Type: application/json" -X PUT -d '{"Status": "Completed"}' --insecure https://localhost:5001/api/transactions/modifystatus/1 </code>

