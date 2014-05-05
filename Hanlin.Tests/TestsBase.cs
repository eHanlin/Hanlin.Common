using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac.Extras.Moq;
using NUnit.Framework;
using log4net;

namespace Hanlin.Tests
{
    public class TestsBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected string BasePath { get; set; }
        public string AssetsFolder { get; set; }
        
        protected Stack<string> PathSegments { get; set; }
        protected AutoMock AutoMock { get; set; }

        public TestsBase()
        {
            AssetsFolder = "TestCases";
            PathSegments = new Stack<string>(new [] { Directory.GetCurrentDirectory(), AssetsFolder });
        }

        protected string PathTo(string filename)
        {
            return Path.Combine(BasePath, filename); 
        }

        protected string PathToOutput(string filename)
        {
            return Path.Combine(BasePath, PathEx.AppendToPath(filename, "_output"));
        }

        [TestFixtureSetUp]
        protected virtual void FixtureSetup()
        {
            ConfigureAssetsPath();
        }

        [SetUp]
        protected virtual void Setup()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

            AutoMock = AutoMock.GetLoose();

            BuildBasePath();
        }

        protected virtual void ConfigureAssetsPath()
        {
        }

        protected void PushPath(string path)
        {
            PathSegments.Push(path);
            BuildBasePath();
        }

        private void BuildBasePath()
        {
            BasePath = Path.Combine(PathSegments.Reverse().ToArray());

            Log.Info("Asset base path: " + BasePath);
        }
    }
}