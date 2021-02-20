using System;
using System.Collections.Generic;
using System.Linq;

namespace TddWork
{
    public class BudgetCalculator
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetCalculator(IBudgetRepo budgetRepo)
        {
            this._budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime.ToString("yyyyMM") == endDateTime.ToString("yyyyMM"))
            {
                return this.GetSameMonthAmount(startDateTime, endDateTime);
            }

            var budgets = this._budgetRepo.GetAll();

            var startBudgetAmount = GetStartBudgetAmount(startDateTime, budgets);

            var endBudgetAmount = GetEndBudgetAmount(endDateTime, budgets);

            var midAmount = GetMidAmount(startDateTime, endDateTime, budgets);

            return startBudgetAmount + endBudgetAmount + midAmount;
        }

        private static int GetDaysInMonth(DateTime dateTime)
        {
            return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        }

        private static int GetEndBudgetAmount(DateTime endDateTime, IEnumerable<Budget> budgets)
        {
            var endBudget = budgets.LastOrDefault(o => o.YearMonth == endDateTime.ToString("yyyyMM"))?.Amount ?? 0;
            var daysInEndMonth = GetDaysInMonth(endDateTime);
            return endBudget / daysInEndMonth * endDateTime.Day;
        }

        private static decimal GetMidAmount(
            DateTime startDateTime,
            DateTime endDateTime,
            IReadOnlyCollection<Budget> budgets)
        {
            var start = startDateTime.AddMonths(1);
            //var end = queryDateTime.AddMonths(-1);

            decimal midAmount = 0;
            for (var i = start; i.Month < endDateTime.Month; i = i.AddMonths(1))
            {
                var budget = budgets.FirstOrDefault(o => o.YearMonth == i.ToString("yyyyMM"));
                midAmount += budget?.Amount ?? 0;
            }

            return midAmount;
        }

        private decimal GetSameMonthAmount(DateTime startDateTime, DateTime endDateTime)
        {
            var budgets = this._budgetRepo.GetAll();
            var budget = budgets.FirstOrDefault(o => o.YearMonth == startDateTime.ToString("yyyyMM"));
            if (budget == null)

            {
                return 0;
            }

            var budgetAmount = budget.Amount / GetDaysInMonth(startDateTime);
            var date = (endDateTime.Date - startDateTime.Date).Days + 1;

            return budgetAmount * date;
        }

        private static int GetStartBudgetAmount(DateTime startDateTime, IEnumerable<Budget> budgets)
        {
            var startBudget = budgets.FirstOrDefault(o => o.YearMonth == startDateTime.ToString("yyyyMM"))?.Amount ?? 0;
            var daysInFirstMonth = GetDaysInMonth(startDateTime);
            var startAmount = startBudget / daysInFirstMonth;
            return startAmount * (daysInFirstMonth - startDateTime.Day + 1);
        }
    }
}