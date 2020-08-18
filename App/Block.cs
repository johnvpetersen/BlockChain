using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

namespace BlockChain
{
    public class Block<T> {

        private string _hash = string.Empty;
        private Data<T> _data;
        private string _prefix;

//        public string Prefix => _prefix;


        [JsonConstructor]
        public Block (string hash, Data<T> data,string prefix) {
            _hash = hash;
            _data = data;
            _prefix = prefix;
        }

        public Block (T data, string previousHash = "", IProofOfWork proofOfWork = null) {
            _prefix = proofOfWork == null ? string.Empty : proofOfWork.GetPrefix ();
            var rnd = new Random ();
            while (true) {
                var blockData = new Data<T> (rnd.Next (), data, previousHash);
                var result = computeHash(blockData);
 
                if (string.IsNullOrEmpty (_prefix) ||
                    result.Substring (0, _prefix.Length) == _prefix) {
                    _hash = result;
                    _data = blockData;
                    break;
                }
            }
        }
        string computeHash () => computeHash (_data);

        string computeHash (Data<T> data) {
            return Convert.ToBase64String (
                SHA256.Create ()
                .ComputeHash (
                    Encoding.UTF8.GetBytes (
                        SerializeObject (
                            data
                        )
                    )
                )
            );
        }
        public override string ToString () {
            return SerializeObject (this);
        }
        public string Hash => _hash;
        public Data<T> Data => _data;
        public bool IsValid () {
            var retVal = true;
            retVal = string.IsNullOrEmpty (_prefix) ? retVal :
                this.Hash.Substring (0, _prefix.Length) == _prefix;

            return retVal && computeHash (_data) == this.Hash;
        }
    }

}