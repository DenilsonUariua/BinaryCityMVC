using BinaryCityMVC.Data;

namespace BinaryCityMVC.Helpers
{
    public class ClientCodeHelpers
    {
        public static class ClientCodeGenerator
        {
            public static string GenerateClientCode(string name, ApplicationDbContext context)
            {
                string alphaPart = name.Length >= 3 ? name.Substring(0, 3).ToUpper() : name.PadRight(3, 'A').ToUpper();
                int numericPart = 1;

                string clientCode;
                do
                {
                    clientCode = $"{alphaPart}{numericPart.ToString("D3")}";
                    numericPart++;
                } while (context.Clients.Any(c => c.ClientCode == clientCode));

                return clientCode;
            }
        }
    }
}
