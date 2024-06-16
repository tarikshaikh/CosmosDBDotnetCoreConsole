using Microsoft.Azure.Cosmos;

namespace CosmosDBDotnetCoreConsole1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Initialize the Cosmos DB client
                using CosmosClient client = new(
                    accountEndpoint: "https://localhost:8081/",
                    authKeyOrResourceToken: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
                );

                // Create a Database if it doesn't exists using the client
                Database database = await client.CreateDatabaseIfNotExistsAsync(
                    id: "ecommerceDb",
                    throughput: 400
                );

                // Create a Container if it doesn't exists using the client
                Container productContainer = await database.CreateContainerIfNotExistsAsync(
                    id: "products",
                    partitionKeyPath: "/category"
                );

                // Preparing an Item to be inserted(if it doesn't exists) or updated(if it exists)
                var productItem = new
                {
                    id = Guid.NewGuid().ToString(),
                    name = "Kiama classic surfboard",
                    category = "Fitness"
                };

                await productContainer.UpsertItemAsync(productItem);
            }
            catch (Exception ex)
            {
                // This will get the current WORKING directory (i.e. \bin\Debug)
                string workingDirectory = Environment.CurrentDirectory;
                // or: Directory.GetCurrentDirectory() gives the same result

                // This will get the current PROJECT directory
                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

                using FileStream fileStream = File.Open(projectDirectory + "/Logger.txt", FileMode.Append);
                using StreamWriter file = new StreamWriter(fileStream);
                file.WriteLine("Exception time: " + DateTime.Now.ToString());
                file.WriteLine("Exception Message: " + ex.Message);
                file.WriteLine("StackTrace: " + ex.StackTrace);
                file.WriteLine("=====================================================");
            }
        }
    }
}
