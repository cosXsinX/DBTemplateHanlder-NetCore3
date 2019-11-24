using DBTemplateHander.DatabaseModel.Import.Importer;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.DatabaseModel.Import.IntegrationTests
{
    [TestFixture]
    public class SQLServerImporterIntegrationTest
    {
        public SQLServerImporter _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new SQLServerImporter();
        }

        [Test]
        public void ImportShouldNotFail()
        {
            string connectionString = "Data Source=DESKTOP-JNFJSV9\\SQLEXPRESS01;Initial Catalog=AdventureWorks2017";
            _tested.Import(connectionString);
        }
    }
}
