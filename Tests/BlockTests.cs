using System;
using System.Collections.Generic;
using BlockChain;
using Xunit;
using static Newtonsoft.Json.JsonConvert;

namespace Tests
{
    public class BlockTests {

        [Fact]
        public void BlockIsInvalidatedAfterChange () {
            var sut = new BlockChain.Block<MyData> (
                new MyData (108, "Hello"), null, new ProofOfWork ());
            Assert.True (sut.IsValid ());
            var blockJSON = sut.ToString ().Replace ("Hello", "Changed");
            sut = DeserializeObject<BlockChain.Block<MyData>> (blockJSON);
            Assert.False (sut.IsValid ());
        }

        [Fact]
        public void BlockCanBeSerializedAndDeSerialized () {
            var sut =
                new BlockChain.Block<MyData> (
                    new MyData (
                        108, "Hello"),
                    null,
                    new ProofOfWork ()
                );
            var blockJSON = sut.ToString ();
            sut = null;
            sut = DeserializeObject<BlockChain.Block<MyData>> (blockJSON);

            Assert.True (sut.IsValid ());
        }

        [Fact]
        public void CanCreateBlockWithModerateDifficulty () {

            var proofOfWork =
                new ProofOfWork (
                    Guid.NewGuid (),
                    new int[] { 1, 3, 5 }
                );

            var sut =
                new BlockChain.Block<object> (
                    new {
                        Amount = 108,
                            Message = "Hello"
                    },
                    null,
                    proofOfWork
                );

            Assert.True (sut.IsValid ());
            Assert.Equal (
                proofOfWork.GetPrefix (),
                sut.Hash.Substring (
                    0,
                    proofOfWork.GetPrefix ().Length
                )
            );
        }

        [Fact]
        public void CanCreateBlockWithLittleDifficulty () {
            var sut =
                new BlockChain.Block<object> (
                    new {
                        Amount = 108,
                            Message = "Hello"
                    },
                    null,
                    new ProofOfWork ()
                );
            Assert.True (sut.IsValid ());
        }

        [Fact]
        public void CanCreateBlockWithoutDifficulty () {
            var sut =
                new BlockChain.Block<object> (
                    new {
                        Amount = 108,
                            Message = "Hello"
                    }
                );
            Assert.True (sut.IsValid ());
        }

        [Fact]
        public void ChangedDataInvalidatesBlock () {
            var data = new List<string> ();
            var sut = new BlockChain.Block<object> (data);
            data.Add ("It Changed");
            Assert.False (sut.IsValid ());
        }
    }
}