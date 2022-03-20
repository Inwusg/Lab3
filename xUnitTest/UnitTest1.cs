using HashTable;
using Xunit;

namespace xUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestResize()
        {
            OpenAddressHashTable<int, int> hashTable = new();
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
    }
}