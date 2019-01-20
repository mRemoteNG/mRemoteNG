using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public static class NunitExtensions
    {
        /// <summary>
        /// Set the name of the fixture created by this <see cref="TestFixtureData"/>
        /// </summary>
        /// <param name="fixtureData"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TestFixtureData SetName(this TestFixtureData fixtureData, string name)
        {
            fixtureData.TestName = name;
            return fixtureData;
        }
    }
}
