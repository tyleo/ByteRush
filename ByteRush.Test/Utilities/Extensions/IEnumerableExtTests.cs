using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ByteRush.Test.Utilities.Extensions
{
    public sealed class IEnumerableExtTests
    {
        [Test]
        public void ConcatTest()
        {
            var one = 1.AsOneItemEnumerable();
            var two = 2.AsOneItemEnumerable();
            var three = 3.AsOneItemEnumerable();

            var concat = IEnumerableExt.Concat(
                one,
                two,
                three
            );

            var concatEnumerator = concat.GetEnumerator();

            Assert.IsTrue(concatEnumerator.MoveNext());
            Assert.AreEqual(1, concatEnumerator.Current);

            Assert.IsTrue(concatEnumerator.MoveNext());
            Assert.AreEqual(2, concatEnumerator.Current);

            Assert.IsTrue(concatEnumerator.MoveNext());
            Assert.AreEqual(3, concatEnumerator.Current);

            Assert.IsFalse(concatEnumerator.MoveNext());
        }

        [Test]
        public void DefaultTest()
        {
            var zeroEnumerable = IEnumerableExt.Default<int>(0);
            var fiveEnumerable = IEnumerableExt.Default<int>(5);

            Assert.IsFalse(zeroEnumerable.GetEnumerator().MoveNext());
            fiveEnumerable.ForEach(i => Assert.AreEqual((int)default, i));
            Assert.IsFalse(zeroEnumerable.GetEnumerator().MoveNext());
        }

        [Test]
        public void DistinctCustomTest()
        {
            var items = Util.NewArray(0, 1, 1, 2);
            var expected = Util.NewHashSet(0, 1, 2);
            void ExpectedContains(IEnumerable<int> items)
            {
                foreach (var item in items)
                {
                    Assert.IsTrue(expected.Contains(item));
                }
            }

            var noneEqual = items.Distinct((_0, _1) => false, i => i.GetHashCode());
            Assert.AreEqual(4, noneEqual.Count());
            ExpectedContains(noneEqual);
            Assert.AreEqual(2, noneEqual.Count(i => i == 1));

            var allEqual = items.Distinct((_0, _1) => true, i => 0);
            Assert.AreEqual(1, allEqual.Count());
            ExpectedContains(allEqual);

            var standard = items.Distinct((_0, _1) => _0 == _1, i => 0);
            Assert.AreEqual(3, standard.Count());
            ExpectedContains(standard);
            Assert.AreEqual(1, standard.Count(i => i == 1));
        }

        [Test]
        public void DistinctMapTest()
        {
            var items = Util.NewArray(0, 1, 1, 2);
            var map = Util.NewDictionary((0, "0"), (1, "1"), (2, "1"));
            var expected = Util.NewHashSet(0, 1);

            var result = items.Distinct(i => map[i]);
            Assert.AreEqual(2, result.Count());
            foreach (var item in result)
            {
                Assert.IsTrue(expected.Contains(item));
            }
        }

        [Test]
        public void EnumerateTest()
        {
            var zero = 0.Enumerate();
            var one = 1.Enumerate();
            var two = 2.Enumerate();

            Assert.IsEmpty(zero);

            var oneEnumerator = one.GetEnumerator();
            Assert.IsTrue(oneEnumerator.MoveNext());
            Assert.AreEqual(0, oneEnumerator.Current);
            Assert.IsFalse(oneEnumerator.MoveNext());

            var twoEnumerator = two.GetEnumerator();
            Assert.IsTrue(twoEnumerator.MoveNext());
            Assert.AreEqual(0, twoEnumerator.Current);
            Assert.IsTrue(twoEnumerator.MoveNext());
            Assert.AreEqual(1, twoEnumerator.Current);
            Assert.IsFalse(twoEnumerator.MoveNext());
        }
    }
}
