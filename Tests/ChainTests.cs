using System;
using System.Diagnostics.CodeAnalysis;
using BlockChain;
using Newtonsoft.Json;
using Xunit;
using System.Linq;

namespace Tests
{

    public class ChainTests
    {
        [Fact]
        public void CanFindBlock()
        {
            var chain = new Chain<SampleData>(new ProofOfWork());
            chain.AddBlock(new SampleData(108, "Message - 108"));
            chain.AddBlock(new SampleData(109, "Message - 109"));
            chain.AddBlock(new SampleData(110, "Message - 110"));

            Assert.True(chain.IsValid());

            var block0 = chain.Blocks.First(x => x.Value.Data.Value.Message == "Message - 108");

            Assert.Equal("Message - 108", block0.Value.Data.Value.Message);
        }


        [Fact]
        public void ChainIsInvalidatedAfterChange()
        {

            var chain = new Chain<SampleData>(new ProofOfWork());
            chain.AddBlock(new SampleData(108, "Message - 108"));
            chain.AddBlock(new SampleData(109, "Message - 109"));

            var blockJson =
               chain.ToString().Replace("Message - 108", "CHANGED");

            chain = JsonConvert.DeserializeObject<Chain<SampleData>>(blockJson, new ProofOfWorkConverter());

            Assert.False(chain.IsValid());
        }

        [Fact]
        public void IsValidIsNullWhenChainHasNoBlocks()
        {
            var chain = new Chain<object>();
            Assert.True(chain.IsValid() == null);
        }

        [Fact]
        public void IsValidIsNotNullWhenChainHasBlock()
        {
            var chain = new Chain<object>();
            chain.AddBlock(new SampleData(108, "Message - 108"));

            Assert.True(chain.IsValid() != null);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ChainCanBeSerializedAndDeSerialized(bool proofOfWork)
        {
            var chain = new Chain<SampleData>(proofOfWork ? new ProofOfWork() : null);
            chain.AddBlock(new SampleData(108, "Amount is 108"));
            chain.AddBlock(new SampleData(109, "Amount is 109"));
            chain.AddBlock(new SampleData(110, "Amount is 110"));

            Assert.True(chain.IsValid());

            var blockJson = chain.ToString();

            chain = JsonConvert.DeserializeObject<Chain<SampleData>>(blockJson, new ProofOfWorkConverter());

            Assert.True(chain.IsValid());
            Assert.Equal(blockJson, chain.ToString());

            chain.AddBlock(new SampleData(111, "Amount is 111"));

            Assert.Equal(4, chain.Blocks.Count);
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
}