using DaXiong.Tools.Core.Helpers;
using Xunit;

namespace DaXiong.Tools.Xunit
{
    public class MemoryCacheHelper_Test
    {
        public MemoryCacheHelper_Test()
        {

        }

        [Fact]
        public void Set()
        {
            MemoryCacheHelper.Set("daxiong", "666");
            Assert.Null(MemoryCacheHelper.GetString("daxiong1"));
            Assert.Equal(MemoryCacheHelper.GetString("daxiong"),"666");
        }
    }
}
