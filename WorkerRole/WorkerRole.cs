using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        string qname = "frogracingqueue";

        string tableConnectionString = CloudConfigurationManager.GetSetting("TableStorageConnection");

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            catch (Exception e)
            { 
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);

                //Skapa ny Queueclient
                QueueClient qc = QueueClient.CreateFromConnectionString(connectionString, qname);

                //Ta emot det meddelande som kommer fr�n web role.                
                BrokeredMessage msg = qc.Receive();

                //det namn v�r table ska ha
                string tableName = "users";
                //Connection till table storage account
                CloudStorageAccount account = CloudStorageAccount.Parse(tableConnectionString);
                //Klient f�r table storage
                CloudTableClient tableStorage = account.CreateCloudTableClient();
                //H�mta en reference till tablen, om inte finns, skapa table
                CloudTable table = tableStorage.GetTableReference(tableName);
                table.CreateIfNotExists();

                try
                {
                    if (msg.Properties["action"].ToString() == "create")
                    {
                        msg.Complete();

                        SaveToStorage(msg.Properties["userName"].ToString(), table);
                    }
                    else if (msg.Properties["action"].ToString() == "update")
                    {
                        msg.Complete();

                        UpdateToStorage(msg.Properties["balance"].ToString(), msg.Properties["userName"].ToString(), table);
                    }
                    else if (msg.Properties["action"].ToString() == "delete")
                    {
                        msg.Complete();
                        DeleteFromStorage(msg.Properties["userName"].ToString(), table);
                    }
                }
                catch (Exception)
                {
                    msg.Abandon();
                }
            }
        }

        private void SaveToStorage(string username, CloudTable table)
        {
            //Skapar den entitet som ska in i storage
            User user = new User(username);           
            user.UserName = username;

            //Sparar personen i signups table
            TableOperation insertOperation = TableOperation.Insert(user);
            table.Execute(insertOperation);
        }

        private void UpdateToStorage(string balance, string userName, CloudTable table)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<User>("users",userName);
            
            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            User updateEntity = (User)retrievedResult.Result;

            if (updateEntity != null)
            {                
                updateEntity.Balance = Int32.Parse(balance);

                // Create the InsertOrReplace TableOperation
                TableOperation insertOrReplaceOperation = TableOperation.Replace(updateEntity);

                try
                {
                    // Execute the operation.
                    table.Execute(insertOrReplaceOperation);
                }
                catch (Exception ex)
                {    
                    throw;
                }
               
            }
        }

        private void DeleteFromStorage(string userName, CloudTable table)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<User>("users", userName);

            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            User deleteEntity = (User)retrievedResult.Result;

            // Create the Delete TableOperation.
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                // Execute the operation.
                table.Execute(deleteOperation);
            }
        }
    }
}
