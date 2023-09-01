using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace RestaurantTableBookingApp.API
{
    public class KeyVaultSecretReader
    {
        public static async Task<string> GetConnectionString(IConfiguration configuration)
        {
            var kvUri = configuration.GetSection("KeyVaultUrl").Value;
            SecretClient client = new SecretClient(new Uri(kvUri!), new DefaultAzureCredential());
            var secret = await client.GetSecretAsync(configuration.GetSection("SecretName").Value, configuration.GetSection("SecretVersion").Value);
            return secret.Value.Value;
        }
    }
}
