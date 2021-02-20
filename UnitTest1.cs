using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace TddWork
{
    [TestClass]
    public class UnitTest1
    {
        private IBudgetRepo _budgetRepo;
        private BudgetCalculator _budget;

        [TestInitialize]
        public void TestInitialize()
        {
            this._budgetRepo = Substitute.For<IBudgetRepo>();
            this._budget = new BudgetCalculator(this._budgetRepo);
        }

        [TestMethod]
        public void QueryDaily()
        {
            this._budgetRepo.GetAll()
                .Returns(new List<Budget>() { new Budget() { YearMonth = "202101", Amount = 31 } });

            var startDateTime = new DateTime(2021, 01, 01);
            var endDateTime = new DateTime(2021, 01, 01);
            var result = this._budget.Query(startDateTime, endDateTime);

            Assert.AreEqual(1, result);
        }
    }
}