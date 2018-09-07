using FluentAssertions;
using togglhelper.Arguments;
using Xunit;

namespace togglhelper.Unit.Tests
{
    public class FillArgumentsTests
    {
        [Fact]
        public void IsSpecificSyncLiefertTrueWennItemsGefuellt()
        {
            var arguments = new FillArguments
            {
                ConfigFilePath = "T:\\test\\",
                ItemIds = new[] { 12345, 67891 }
            };

            arguments.IsSpecificSync.Should().BeTrue();
        }

        [Fact]
        public void IsSpecificSyncLiefertFalseWennItemsNichtGefuellt()
        {
            var arguments = new FillArguments
            {
                ConfigFilePath = "T:\\test\\"
            };

            arguments.IsSpecificSync.Should().BeFalse();
        }
    }
}
