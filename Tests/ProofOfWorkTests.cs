using System;
using Xunit;

namespace Tests
{
    public class ProofOfWorkTests
    {
        [Fact]
        public void CanVerifyPrefixWithOutAnyParameter()
        {
            var proofOfWork = new ProofOfWork();
            Console.WriteLine($"Prefix: {proofOfWork.GetPrefix()}");
            Assert.Equal(1, proofOfWork.GetPrefix().Length);
        }

        [Fact]
        public void CanVerifyPrefixWithOutIntArrayParameter()
        {
            var proofOfWork = new ProofOfWork(Guid.NewGuid(), null);
            Console.WriteLine($"Prefix: {proofOfWork.GetPrefix()}");
            Assert.True(string.IsNullOrEmpty(proofOfWork.GetPrefix()));
        }

        [Fact]
        public void CanVerifyPrefixWithParameters()
        {
            var guid = Guid.NewGuid();
            int[] charsToTake = { 0, 5, 10 };
            var proofOfWork = new ProofOfWork(guid, charsToTake);
            var expectedPrefix = string.Empty;

            Array.ForEach(charsToTake, element =>
            {
                expectedPrefix += guid.ToString().Substring(element, 1);
            });

            Assert.Equal(expectedPrefix, proofOfWork.GetPrefix());

            Console.WriteLine($"Prefix: {proofOfWork.GetPrefix()}");
        }
    }
}