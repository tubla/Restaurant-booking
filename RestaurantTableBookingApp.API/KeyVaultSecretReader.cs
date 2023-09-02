using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace RestaurantTableBookingApp.API
{
    public class KeyVaultSecretReader
    {
        public static string GetConnectionString(IConfiguration configuration, string keyvaultTypeName)
        {

            var kvUri = configuration.GetSection($"{keyvaultTypeName}:KeyVaultUrl").Value;
            SecretClient client = new SecretClient(new Uri(kvUri!), new DefaultAzureCredential());
            var secret = client.GetSecretAsync(configuration.GetSection($"{keyvaultTypeName}:SecretName").Value, configuration.GetSection($"{keyvaultTypeName}:SecretVersion").Value).Result;
            return secret.Value.Value;
        }
    }
}
