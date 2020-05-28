using ByteRush.Utilities;
using ByteRush.Utilities.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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

        [Test]
        public void ExtendTest()
        {
            var one = Util.NewArray(1);
            var two = Util.NewArray(1, 2);

            var extend = one.Extend(two);

            var enumerator = extend.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ForEachTest()
        {
            var expected = Util.NewArray(1, 2);
            var actual = Util.NewArray(1, 2);

            var expectedSum = 0;
            foreach (var i in expected) expectedSum += i;

            var actualSum = 0;
            actual.ForEach(i => actualSum += i);

            Assert.AreEqual(expectedSum, actualSum);
        }

        [Test]
        public void SequenceGetHashCodeTest()
        {
            var expected = 1.GetHashCode() ^ 50.GetHashCode() ^ -573.GetHashCode();
            var actual = Util.NewArray(1, 50, -573).SequenceGetHashCode();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToDictionaryTest()
        {
            var expected = new Dictionary<string, int>() { { "0", 0 }, { "1", 1 } };
            var actual = Util.NewArray(("0", 0), ("1", 1)).ToDictionary();

            foreach (var (key, value) in actual.ToTuples())
            {
                Assert.IsTrue(expected[key] == value);
            }

            var empty = Util.NewArray<(object, object)>().ToDictionary();
            Assert.IsEmpty(empty);
        }

        [Test]
        public void ToHashSetTest()
        {
            var expected = Util.NewArray("0", "1");
            var actual = IEnumerableExt.ToHashSet(Util.NewArray("0", "1", "1"));

            foreach (var value in expected)
            {
                Assert.IsTrue(actual.Contains(value));
            }

            Assert.AreEqual(2, actual.Count);

            var empty = Util.NewArray<(object, object)>().ToDictionary();
            Assert.IsEmpty(empty);
        }

        [Test]
        public void ToTuplesTest()
        {
            var expected = Util.NewArray(Util.NewKVP("0", 0), Util.NewKVP("1", 1));
            var actual = expected.ToTuples();

            foreach (var (expectedItem, actualItem) in IEnumerableExt.ValueZip(expected, actual))
            {
                Assert.AreEqual(expectedItem.Key, actualItem.Key);
                Assert.AreEqual(expectedItem.Value, actualItem.Value);
            }

            var empty = Util.NewArray<KeyValuePair<object, object>>().ToTuples();
            Assert.IsEmpty(empty);
        }

        [Test]
        public void IntoRefTest()
        {
            var items = Util.NewArray(0, 1);
            var actual = items.IntoRef();
            var enumerator = actual.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
            enumerator.Current = 2;
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());

            var empty = Util.NewArray<KeyValuePair<object, object>>().ToTuples();
            Assert.IsFalse(empty.GetEnumerator().MoveNext());
        }

        [Test]
        public void ValueZipTest()
        {
            var left = Util.NewArray(0, 1);
            var right = Util.NewArray(2, 3, 4);
            var zipped = left.Zip(right);
            var enumerator = zipped.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current.First);
            Assert.AreEqual(2, enumerator.Current.Second);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current.First);
            Assert.AreEqual(3, enumerator.Current.Second);
            Assert.IsFalse(enumerator.MoveNext());

            var empty = Util.NewArray<KeyValuePair<object, object>>().ToTuples();
            Assert.IsEmpty(left.ValueZip(empty));
            Assert.IsEmpty(empty.ValueZip(left));
        }
    }
}
