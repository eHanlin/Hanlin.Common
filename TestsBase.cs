using System.IO;
using Autofac.Extras.Moq;
using NUnit.Framework;

namespace Hanlin.Tests
{
    public class TestsBase
    {
        public string AssetsFolder { get; set; }

        protected string BasePath { get; set; }
        protected AutoMock AutoMock { get; set; }

        public TestsBase()
        {
            AssetsFolder = "TestCases";
        }

        protected string PathTo(string filename)
        {
            return Path.Combine(BasePath, filename); 
        }

        [SetUp]
        protected virtual void Setup()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

            AutoMock = AutoMock.GetLoose();

            BasePath = Path.Combine(Directory.GetCurrentDirectory(), AssetsFolder);
        }
    }
}