using System;
using Newtonsoft.Json;
using BlockChain;

namespace Tests
{
  
    
    public class ProofOfWork : IProofOfWork
    {

        private string _prefix;

        [JsonConstructor]
        public ProofOfWork(string prefix) {
            _prefix = prefix;
        }

        public ProofOfWork() : this(Guid.NewGuid(),new int[] {0}) {}

        public ProofOfWork(Guid guid, int[] charsToTake) {

            _prefix = string.Empty;
            if (charsToTake == null || charsToTake.Length == 0)
               return;    

             Array.ForEach(charsToTake, element => {
                 _prefix += guid.ToString().Substring(element,1);
             });
        }

        public string GetPrefix() => _prefix;
    }

}