using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;
using System.Linq;

namespace BlockChain
{
    public class Chain<T>
    {
        private readonly Dictionary<int, Block<T>> _blockChain = new Dictionary<int, Block<T>>();
        private readonly IProofOfWork _proofOfWork = null;

        public ImmutableDictionary<int, Block<T>> Blocks => _blockChain.ToImmutableDictionary();

        [JsonConstructor]
        public Chain(Dictionary<int, Block<T>> blockChain, IProofOfWork proofOfWork)
        {
            _proofOfWork = proofOfWork;
            _blockChain = blockChain;
        }

        public Chain(IProofOfWork proofOfWork = null)
        {
            _proofOfWork = proofOfWork;
        }

        public bool? IsValid()
        {
            if (Blocks == null || Blocks.Count == 0)
                return null;

            if (Blocks.Count(x => !x.Value.IsValid()) > 0)
                return false;

            for (int i = 1; i < Blocks.Count; i++)
            {
                if (Blocks[i].Data.PreviousHash != Blocks[i - 1].Hash)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            var prefix = _proofOfWork == null ? string.Empty : _proofOfWork.GetPrefix();

            return JsonConvert.SerializeObject(
                new
                {
                    BlockChain = _blockChain,
                    ProofOfWork = prefix
                }
            );
        }

        public void AddBlock(T dataValue)
        {
            var previousHash =
                _blockChain.Count == 0 ?
                null :
                _blockChain[_blockChain.Count - 1].Hash;

            _blockChain.Add(
                _blockChain.Count,
                new Block<T>(
                    dataValue,
                    previousHash,
                    _proofOfWork)
            );
        }

        public Block<T> this[int i] => _blockChain[i];
    }
}