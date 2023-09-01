using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace RestaurantTableBookingApp.API
{
    public class KeyVaultSecretReader
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var kvUri = configuration.GetSection("KeyVaultUrl").Value;
            SecretClient client = new SecretClient(new Uri(kvUri!), new DefaultAzureCredential());
            var secret = client.GetSecretAsync(configuration.GetSection("SecretName").Value, configuration.GetSection("SecretVersion").Value).Result;
            return secret.Value.Value;
        }
    }
}
