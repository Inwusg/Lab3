using System;
using System.Collections.Generic;
using HashTable;
using Xunit;

namespace xUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestResize()
        {
            OpenAddressHashTable<int, int> hashTable = new(0);
            for (int i = 0; i < 5; i++)
            {
                hashTable.Add(i, i);
            }

            int k = 0;
            bool flag = true;
            foreach (var pair in hashTable)
            {
                if (pair.Key != pair.Value || pair.Value != k)
                {
                    flag = false;
                    break;
                }
                k++;
            } 
            Assert.True(flag);      
        }

        [Fact]
        public void TestResizeAndRehash()
        {
            int[] mas = {5, 55, 605, 6655, 73205, 805255, 8857508, 4, 44, 484, 5324, 58564};
            OpenAddressHashTable<int, int> hashTable = new(1);

            foreach (int item in mas) 
                hashTable.Add(item, item);

            bool flag = false;
            foreach (int item in mas)
                flag = hashTable.Remove(item);

            Assert.Equal(0, hashTable.Count);
        }

        [Fact]
        public void DeleteNoExistsAItem()
        {
            OpenAddressHashTable<int, int> hashTable = new();
            Assert.False(hashTable.Remove(1));
        }

        [Fact]
        public void DeleteExistsAItem()
        {
            OpenAddressHashTable<int, int> hashTable = new();
            hashTable.Add(1, 1);
            Assert.True(hashTable.Remove(1));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(999999)]
        public void CheckCountAfterAddElement(int value)
        {
            OpenAddressHashTable<int, int> hashTable = new();
            for (int i = 0; i < value; i++)
            {
                hashTable.Add(i, i);
            }
            Assert.Equal(value, hashTable.Count);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(999999)]
        public void CheckCountAfterRemoveElement(int value)
        {
            OpenAddressHashTable<int, int> hashTable = new();
            for (int i = 0; i < value; i++)
            {
                hashTable.Add(i, i);
            }

            for (int i = 0; i < value/2; i++)
            {
                hashTable.Remove(i);
            }

            Assert.Equal(value - value / 2, hashTable.Count);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(999999)]
        public void CheckElementAfterRemove(int value)
        {
            OpenAddressHashTable<int, int> hashTable = new();
            for (int i = 0; i < value; i++)
            {
                hashTable.Add(i, i);
            }

            for (int i = 0; i < value / 2; i++)
            {
                hashTable.Remove(i);
            }

            bool flag = true;
            for (int i = value / 2; i < value; i++)
            {
                if (hashTable[i] != i)
                {
                    flag=false;
                    break;
                }
            }
            Assert.True(flag);
        }

        [Fact]
        public void TestForeach()
        {
            OpenAddressHashTable<int, int> hashTable = new(0);
            for (int i = 0; i < 10000; i++)
            {
                hashTable.Add(i, i);
            }

            int k = 0;
            bool flag = true;
            foreach (var pair in hashTable)
            {
                if (pair.Key != pair.Value || pair.Value != k)
                {
                    flag = false;
                    break;
                }
                k++;
            }
            Assert.True(flag);
        }

        [Fact]
        public void TryAddExistsKey()
        {
            OpenAddressHashTable<int, int> hashTable = new();
            hashTable.Add(1, 1);
            Assert.Throws<ArgumentException>(() => hashTable.Add(1, 1));
        }

        [Fact]
        public void TryGetNoExistsKey()
        {
            OpenAddressHashTable<int, int> hashTable = new();
            hashTable.Add(1,1);
            hashTable[1]++;
            Assert.Throws<KeyNotFoundException>(() => hashTable[2]);
        }

    }
}