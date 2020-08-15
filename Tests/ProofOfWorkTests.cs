using System;
using BlockChain;
using Xunit;

namespace Tests {

    public class ProofOfWorkTests {

        [Fact]
        public void CanVerifyPrefixWithOutAnyParameter () {
            var sut = new ProofOfWork ();
            System.Console.WriteLine ($"Prefix: {sut.GetPrefix()}");
            Assert.Equal (1, sut.GetPrefix ().Length);

        }

        [Fact]
        public void CanVerifyPrefixWithOutIntArrayParameter () {
            var sut = new ProofOfWork (Guid.NewGuid (), null);
            System.Console.WriteLine ($"Prefix: {sut.GetPrefix()}");
            Assert.True (string.IsNullOrEmpty (sut.GetPrefix ()));

        }

        [Fact]
        public void CanVerifyPrefixWithParameters () {
            var guid = Guid.NewGuid ();
            int[] charsToTake = { 0, 5, 10 };
            var expected = string.Empty;

            var sut = new ProofOfWork (guid, charsToTake);

            Array.ForEach (charsToTake, element => {
                expected += guid.ToString ().Substring (element, 1);
            });

            Assert.Equal (expected, sut.GetPrefix ());

            System.Console.WriteLine ($"Prefix: {sut.GetPrefix()}");

        }
    }

}