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
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        [TestInitialize]
        public void TestInitialize()
        {
            this._budgetRepo = Substitute.For<IBudgetRepo>();
            this._budget = new BudgetCalculator(this._budgetRepo);
        }

        [TestMethod]
        public void Query查一月一號_2Daily()
        {
            this.GivenOneMonthBudget("202101", 31);

            this.GivenStartDate(2021, 01, 1);
            this.GivenEndDate(2021, 01, 02);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Query查一月一號_Daily()
        {
            this.GivenOneMonthBudget("202101", 31);

            this.GivenStartDate(2021, 01, 1);
            this.GivenEndDate(2021, 01, 01);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Query查一月一號_資料只有二月_ResultIs_0()
        {
            this.GivenOneMonthBudget("202102", 28);

            this.GivenStartDate(2021, 01, 1);
            this.GivenEndDate(2021, 01, 02);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(0, result);
        }

        private void GivenEndDate(int year, int month, int day)
        {
            this._endDateTime = new DateTime(year, month, day);
        }

        private void GivenOneMonthBudget(string yearMonth, int amount)
        {
            this._budgetRepo.GetAll()
                .Returns(new List<Budget>() { new Budget() { YearMonth = yearMonth, Amount = amount } });
        }

        private void GivenStartDate(int year, int month, int day)
        {
            this._startDateTime = new DateTime(year, month, day);
        }
    }
}