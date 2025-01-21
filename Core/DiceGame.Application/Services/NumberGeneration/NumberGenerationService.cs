using System.Security.Cryptography;

namespace DiceGame.Application.Services.NumberGeneration
{
    public class NumberGenerationService : INumberGenerationService
    {
        public (string HMAC, byte[] Key, int Number) GenerateRandom(int range)
        {
            if (range <= 0)
            {
                Console.WriteLine("Error: Range must be greater than 0.");
                return (string.Empty, Array.Empty<byte>(), -1);
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                int number = Roll(range);
                if (number == -1)                
                    return (string.Empty, Array.Empty<byte>(), -1);
                
                byte[] key = new byte[32];
                rng.GetBytes(key);
                    
                using (HMACSHA3_256 hmacSha3 = new HMACSHA3_256(key))
                {
                    byte[] hmacBytes = hmacSha3.ComputeHash(BitConverter.GetBytes(number));
                    string hmac = BitConverter.ToString(hmacBytes).Replace("-", "");
                    return (hmac, key, number);
                }
            }
        }
        public int Roll(int range)
        {
            if (range <= 0)
            {
                Console.WriteLine("Error: Range must be greater than 0.");
                return -1;
            }

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[4];
                rng.GetBytes(buffer);
                uint number = BitConverter.ToUInt32(buffer, 0);
                return (int)(number % (uint)range);
            }
        }
    }
}
