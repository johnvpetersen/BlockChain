using Newtonsoft.Json;

namespace BlockChain
{
    public class Data<T>
    {
        public Data(int nonce, T value, string previousHash = "")
        {
            Nonce = nonce;
            Value = value;
            PreviousHash = previousHash;
        }

        public int Nonce { get; private set; }

        public T Value { get; private set; }

        public string PreviousHash { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}