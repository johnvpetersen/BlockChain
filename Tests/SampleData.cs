using Newtonsoft.Json;

namespace Tests
{
    public class SampleData
    {
        private readonly int _amount;
        private readonly string _message;

        [JsonConstructor]
        public SampleData(int amount, string message)
        {
            _amount = amount;
            _message = message;
        }

        public int Amount => _amount;

        public string Message => _message;
    }
}