using System;
using System.Diagnostics.CodeAnalysis;
using BlockChain;
using Newtonsoft.Json;
using Xunit;
using static Newtonsoft.Json.JsonConvert;
namespace Tests
{

    public class ChainTests {


        public ChainTests() {

var settings = new JsonSerializerSettings
            {
                Converters = null
            };


            
        }

        [Fact]
        public void CanUpdateBlock() {
            var blocks = new Chain<MyData> (new ProofOfWork());
            blocks.AddBlock (new MyData (108, "Message - 108"));
            blocks.AddBlock (new MyData (109, "Message - 109"));
            blocks.AddBlock (new MyData (110, "Message - 110"));

        Assert.True(blocks.IsValid());
        Assert.Equal(3,blocks.Blocks.Count);

        blocks.UpdateBlock(1,new MyData (1099, "Message - 1099"));

        Assert.True(blocks.IsValid());
        Assert.Equal(3,blocks.Blocks.Count);
        Assert.Equal("Message - 1099",blocks[1].Data.Value.Message); 

        }


        [Fact]
        public void CanRemoveBlock() {
            var blocks = new Chain<MyData> (new ProofOfWork());
            blocks.AddBlock (new MyData (108, "Message - 108"));
            blocks.AddBlock (new MyData (109, "Message - 109"));
            blocks.AddBlock (new MyData (110, "Message - 110"));

        Assert.True(blocks.IsValid());
        Assert.Equal(3,blocks.Blocks.Count);

        blocks.RemoveBlock(1);

        Assert.True(blocks.IsValid());
        Assert.Equal(2,blocks.Blocks.Count);


        }


        [Fact]
        public void ChainIsInvalidatedAfterChange () {

            var blocks = new Chain<MyData> (new ProofOfWork());
            blocks.AddBlock (new MyData (108, "Message - 108"));
            blocks.AddBlock (new MyData (109, "Message - 109"));

            var blockJSON = 
               blocks.ToString ().Replace ("Message - 108", "CHANGED");

            blocks = DeserializeObject<Chain<MyData>> (blockJSON, new ProofOfWorkConverter());

            Assert.False (blocks.IsValid ());
        }

        [Fact]
        public void IsValidIsNullWhenChainDoesNotExist () {
            var sut = new Chain<object> ();
            Assert.True (sut.IsValid () == null);
        }

        [Fact]
        public void IsValidIsNotNullWhenChainExists () {
            var sut = new Chain<object> ();
            sut.AddBlock (new MyData (108, "Message - 108"));

            Assert.True (sut.IsValid () != null);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]

        public void ChainCanBeSerializedAndDeSerialized (bool proofOfWork) {

        

            var blocks = new Chain<MyData> (proofOfWork ? new ProofOfWork() : null);
            blocks.AddBlock (new MyData (108, "Message - 108"));
            blocks.AddBlock (new MyData (109, "Message - 109"));
            blocks.AddBlock (new MyData (110, "Message - 110"));

            var blockJSON = blocks.ToString ();

            blocks = DeserializeObject<Chain<MyData>> (blockJSON, new ProofOfWorkConverter());

            Assert.True (blocks.IsValid ());


        }

    }


    public class ProofOfWorkConverter : JsonConverter<IProofOfWork>
    {
        public override IProofOfWork ReadJson(JsonReader reader, Type objectType, [AllowNull] IProofOfWork existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new ProofOfWork((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] IProofOfWork value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    public class MyData {

        private int _amount;
        private string _message;

        [JsonConstructor]
        public MyData (int amount, string message) {
            _amount = amount;
            _message = message;
        }

        public int Amount => _amount;
        public string Message => _message;
    }

}