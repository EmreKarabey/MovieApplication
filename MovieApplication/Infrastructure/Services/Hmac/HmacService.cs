using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Hmac
{
    public class HmacService
    {
        private readonly string _secretkey;

        public HmacService(IConfiguration configuration)
        {
            _secretkey = configuration["HmacSettings:SecretKey"];
        }

        public string ComputeSignature(string method, string path, string body, string timestamp)
        {
            var rawData = $"{method}{path}{body}{timestamp}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretkey));

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            return Convert.ToBase64String(hash);
        }

        public bool ValidateSignature(string method, string path, string body, string timestamp, string clientSignature)
        {
            if (!long.TryParse(timestamp, out var ts)) return false;

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(ts);

            if (Math.Abs((DateTimeOffset.UtcNow - requestTime).TotalMinutes) > 5)
                return false;

            var expected = ComputeSignature(method, path, body, timestamp);

            return CryptographicOperations.FixedTimeEquals(
    Convert.FromBase64String(expected),
    Convert.FromBase64String(clientSignature));

        }
    }
}
