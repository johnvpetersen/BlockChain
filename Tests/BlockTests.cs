using System;
using System.Collections.Generic;
using BlockChain;
using Xunit;
using Newtonsoft.Json;

namespace Tests
{
    public class BlockTests
    {
        [Fact]
        public void CanInstantiateData()
        {
            var data = new Data<SampleData>(0, new SampleData(10, "Amount is $10.00"));            

            Assert.Equal(10, data.Value.Amount);
            Assert.Equal("Amount is $10.00", data.Value.Message);
            Assert.Equal(0, data.Nonce);
            Assert.True(string.IsNullOrEmpty(data.PreviousHash));
        }

        [Fact]
        public void BlockIsInvalidatedAfterChange()
        {
            // Create valid block
            var block = new Block<SampleData>(new SampleData(108, "Hello"), null, new ProofOfWork());
            Assert.True(block.IsValid());

            // Change the block by replacing the sample data
            var blockJson = block.ToString().Replace("Hello", "Changed");
            block = JsonConvert.DeserializeObject<Block<SampleData>>(blockJson);

            // Make sure it is now invalid
            Assert.False(block.IsValid());
        }

        [Fact]
        public void BlockCanBeSerializedAndDeSerialized()
        {
            var block = new Block<SampleData>(new SampleData(108, "Hello"), null, new ProofOfWork());
            var blockJson = block.ToString();
            block = JsonConvert.DeserializeObject<Block<SampleData>>(blockJson);
            Assert.True(block.IsValid());            
        }

        [Fact]
        public void CanCreateBlockWithModerateDifficulty()
        {
            var proofOfWork =
                new ProofOfWork(
                    Guid.NewGuid(),
                    new int[] { 1, 3, 5 }
                );

            var block =
                new Block<object>(
                    new
                    {
                        Amount = 108,
                        Message = "Hello"
                    },
                    null,
                    proofOfWork
                );

            Assert.True(block.IsValid());
            Assert.Equal(
                proofOfWork.GetPrefix(),
                block.Hash.Substring(
                    0,
                    proofOfWork.GetPrefix().Length
                )
            );
        }

        [Fact]
        public void CanCreateBlockWithLittleDifficulty()
        {
            var block =
                new Block<object>(
                    new
                    {
                        Amount = 108,
                        Message = "Hello"
                    },
                    null,
                    new ProofOfWork()
                );
            Assert.True(block.IsValid());
        }

        [Fact]
        public void CanCreateBlockWithoutDifficulty()
        {
            var sut =
                new BlockChain.Block<object>(
                    new
                    {
                        Amount = 108,
                        Message = "Hello"
                    }
                );
            Assert.True(sut.IsValid());
        }

        [Fact]
        public void ChangedDataInvalidatesBlock()
        {
            var dataValue = new List<string>();
            var block = new Block<object>(dataValue);
            dataValue.Add("It Changed");
            Assert.False(block.IsValid());
        }
    }
}