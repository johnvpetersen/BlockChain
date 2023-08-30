using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlockChain
{
    public class Block<T>
    {
        private readonly string _hash = string.Empty;
        private readonly Data<T> _data;
        private readonly string _prefix;

        [JsonConstructor]
        public Block(string hash, Data<T> data, string prefix)
        {
            _hash = hash;
            _data = data;
            _prefix = prefix;
        }

        public Block(T dataValue, string previousHash = "", IProofOfWork proofOfWork = null)
        {
            _prefix = proofOfWork == null ? string.Empty : proofOfWork.GetPrefix();
            var rnd = new Random();
            while (true)
            {
                Console.WriteLine("Creating hash candidate...");
                var data = new Data<T>(rnd.Next(), dataValue, previousHash);
                var hashCandidate = ComputeHash(data);

                if (string.IsNullOrEmpty(_prefix) ||
                    hashCandidate.Substring(0, _prefix.Length) == _prefix)
                {
                    _hash = hashCandidate;
                    _data = data;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Hash => _hash;

        public Data<T> Data => _data;

        public bool IsValid()
        {
            bool isHashPrefixValid =
                string.IsNullOrEmpty(_prefix) ||
                Hash.Substring(0, _prefix.Length) == _prefix;

            return isHashPrefixValid && ComputeHash(_data) == Hash;
        }

        private string ComputeHash(Data<T> data)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(
                    sha256
                    .ComputeHash(
                        Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(
                                data
                            )
                        )
                    )
                );
            }
        }
    }
}