using System;
using Xunit;

namespace CustomComparer.Tests
{
    public class CustomComparerTests
    {
        [Fact]
        public void ShouldReturnTrueWhenObjectsAreEqual()
        {
            var a = new ClassForTesting<string>("my value");
            var b = new ClassForTesting<string>("my value");

            Assert.True(a.IsEqualTo(b));
        }

        [Fact]
        public void ShouldReturnFalseWhenObjectsDiffer()
        {
            var a = new ClassForTesting<int>(1);
            var b = new ClassForTesting<int>(2);

            Assert.False(a.IsEqualTo(b));
        }

        [Fact]
        public void ShouldReturnTrueWhenObjectsInIEnumerableAreEqual()
        {
            var first = new object[] { new ClassForTesting<int>(12), new ClassForTesting<string>("abc"), 1.2m, new DateTime(2020, 01, 01), };
            var second = new object[] { new ClassForTesting<int>(12), new ClassForTesting<string>("abc"), 1.2m, new DateTime(2020, 01, 01), };

            Assert.True(first.IsEqualTo(second));
        }

        [Fact]
        public void ShouldReturnFalseWhenObjectsInIEnumerableAreDifferent()
        {
            var first = new object[] { new ClassForTesting<int>(12), new ClassForTesting<string>("abc"), new ClassForTesting<decimal>(1.2m), };
            var second = new object[] { new ClassForTesting<int>(12), "abc", };

            Assert.False(first.IsEqualTo(second));
        }

    }
}
