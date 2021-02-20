using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace TddWork
{
    [TestClass]
    public class UnitTest1
    {
        private BudgetCalculator _budget;
        private IBudgetRepo _budgetRepo;
        private DateTime _endDateTime;
        private DateTime _startDateTime;

        [TestInitialize]
        public void TestInitialize()
        {
            this._budgetRepo = Substitute.For<IBudgetRepo>();
            this._budget = new BudgetCalculator(this._budgetRepo);
        }

        [TestMethod]
        public void Query_()
        {
            this._budgetRepo.GetAll()
                .Returns(new List<Budget> { new Budget { YearMonth = "202102", Amount = 56 } });

            this.GivenStartDate(2021, 02, 1);
            this.GivenEndDate(2021, 02, 02);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void QueryDateRangeInvalid()
        {
            var budgetList = new List<Budget>();
            budgetList.Add(new Budget { YearMonth = "202012", Amount = 31 });
            budgetList.Add(new Budget { YearMonth = "202111", Amount = 60 });
            budgetList.Add(new Budget { YearMonth = "202211", Amount = 90 });

            this._budgetRepo.GetAll()
                .Returns(budgetList);

            this.GivenEndDate(2020, 12, 31);
            this.GivenStartDate(2022, 11, 1);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(0, result);
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

        [TestMethod]
        public void Query查跨2年()
        {
            var budgetList = new List<Budget>();
            budgetList.Add(new Budget { YearMonth = "202012", Amount = 31 });
            budgetList.Add(new Budget { YearMonth = "202111", Amount = 60 });
            budgetList.Add(new Budget { YearMonth = "202211", Amount = 90 });

            this._budgetRepo.GetAll()
                .Returns(budgetList);

            this.GivenStartDate(2020, 12, 31);
            this.GivenEndDate(2022, 11, 1);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(1 + 60 + 3, result);
        }

        [TestMethod]
        public void Query查跨3個月()
        {
            var budgetList = new List<Budget>();
            budgetList.Add(new Budget { YearMonth = "202102", Amount = 56 });
            budgetList.Add(new Budget { YearMonth = "202103", Amount = 31 });
            budgetList.Add(new Budget { YearMonth = "202104", Amount = 90 });

            this._budgetRepo.GetAll()
                .Returns(budgetList);

            this.GivenStartDate(2021, 02, 27);
            this.GivenEndDate(2021, 04, 3);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(4 + 31 + 9, result);
        }

        [TestMethod]
        public void Query查跨4個月()
        {
            var budgetList = new List<Budget>();
            budgetList.Add(new Budget { YearMonth = "202102", Amount = 56 });
            budgetList.Add(new Budget { YearMonth = "202103", Amount = 31 });
            budgetList.Add(new Budget { YearMonth = "202104", Amount = 90 });
            budgetList.Add(new Budget { YearMonth = "202105", Amount = 124 });

            this._budgetRepo.GetAll()
                .Returns(budgetList);

            this.GivenStartDate(2021, 02, 27);
            this.GivenEndDate(2021, 05, 3);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(4 + 31 + 90 + 12, result);
        }

        [TestMethod]
        public void Query查跨月()
        {
            var budgetList = new List<Budget>();
            budgetList.Add(new Budget { YearMonth = "202102", Amount = 56 });
            budgetList.Add(new Budget { YearMonth = "202103", Amount = 31 });

            this._budgetRepo.GetAll()
                .Returns(budgetList);

            this.GivenStartDate(2021, 02, 27);
            this.GivenEndDate(2021, 03, 3);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Query查跨年()
        {
            var budgetList = new List<Budget>();
            budgetList.Add(new Budget { YearMonth = "202012", Amount = 31 });
            budgetList.Add(new Budget { YearMonth = "202112", Amount = 62 });

            this._budgetRepo.GetAll()
                .Returns(budgetList);

            this.GivenStartDate(2020, 12, 31);
            this.GivenEndDate(2021, 12, 31);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(63, result);
        }

        [TestMethod]
        public void Query查整個月份()
        {
            this.GivenOneMonthBudget("202104", 30);

            this.GivenStartDate(2021, 04, 1);
            this.GivenEndDate(2021, 04, 30);
            var result = this._budget.Query(this._startDateTime, this._endDateTime);

            Assert.AreEqual(30, result);
        }

        private void GivenEndDate(int year, int month, int day)
        {
            this._endDateTime = new DateTime(year, month, day);
        }

        private void GivenOneMonthBudget(string yearMonth, int amount)
        {
            this._budgetRepo.GetAll()
                .Returns(new List<Budget> { new Budget { YearMonth = yearMonth, Amount = amount } });
        }

        private void GivenStartDate(int year, int month, int day)
        {
            this._startDateTime = new DateTime(year, month, day);
        }
    }
}