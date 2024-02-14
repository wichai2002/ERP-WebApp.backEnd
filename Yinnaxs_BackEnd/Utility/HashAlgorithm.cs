using System;
using System.Security.Cryptography;
using System.Text;

namespace Yinnaxs_BackEnd.Utility
{
	public class HashAlgorithm
	{
		public HashAlgorithm()
		{
		}

		public string genarateHash(string plainText)
		{
			string result = string.Empty;

			using (var hash = SHA256.Create())
			{
                var byteArrayResultOfRawData = Encoding.UTF32.GetBytes(plainText);
				var byteArrayResult = hash.ComputeHash(byteArrayResultOfRawData);
				result = string.Concat(Array.ConvertAll(byteArrayResult, h => h.ToString("X2")));
            }
			Console.WriteLine(result);

			return result;
		}

		public bool VerifyHash(string InputText, string hashText)
			
		{
			var verify = false;
            using (var hash = SHA256.Create()){
				var hashOfImputText = genarateHash(InputText);

				StringComparer comparer = StringComparer.OrdinalIgnoreCase;
				verify = comparer.Compare(hashOfImputText, hashText) == 0;
			}

			return verify;
		}

    }
}

