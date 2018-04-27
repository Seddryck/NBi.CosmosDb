using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration.Extension;
using NBi.Core.CosmosDb.Query.Execution;
using NBi.Core.CosmosDb.Query.Command;
using NBi.Core.CosmosDb.Query.Client;

namespace NBi.Testing.Core.CosmosDb.Unit.Configuration.Extension
{
    public class ExtensionAnalyzerTest
    {
        [Test]
        public void Execute_CosmosDb_Six()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types.Count(), Is.EqualTo(6));
        }

        [Test]
        public void Execute_GraphApi_IClientFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(GraphClientFactory)));
        }

        [Test]
        public void Execute_GraphApi_ICommandFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(GraphCommandFactory)));
        }

        [Test]
        public void Execute_GraphApi_IExecutionEngine()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(GraphExecutionEngine)));
        }


        [Test]
        public void Execute_SqlApi_IClientFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(SqlClientFactory)));
        }

        [Test]
        public void Execute_SqlApi_ICommandFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(SqlCommandFactory)));
        }

        [Test]
        public void Execute_SqlApi_IExecutionEngine()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(SqlExecutionEngine)));
        }
    }
}
