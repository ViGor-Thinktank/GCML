using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using NUnit.Framework;

namespace GenericCampaignMasterLibTests.Tests
{
    [TestFixture]
    public class SektorTest
    {
        Sektor sektorTest;

        [SetUp]
        public void init()
        {
            sektorTest = new Sektor(666);
        }

        [Test]
        public void addUnitTest ()
		{
            int unitCount = sektorTest.ListUnits.Count;

            DummyUnit testUnit = new DummyUnit(667);
            sektorTest.addUnit(testUnit);

            Assert.AreEqual(unitCount + 1, sektorTest.ListUnits.Count);
		}
		
        [Test]
		public void removeUnitTest ()
		{
            DummyUnit testUnit = new DummyUnit(668);
            sektorTest.addUnit(testUnit);

            int unitCount = sektorTest.ListUnits.Count;

            sektorTest.removeUnit(testUnit);

            Assert.AreEqual(unitCount - 1, sektorTest.ListUnits.Count);
		}

        [Test]
        public void EqualsTest()
        {


        }

    }
}
