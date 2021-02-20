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
            if (IsQueryDateInvalid(startDateTime, endDateTime))
            {
                return 0;
            }

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

        private static bool IsQueryDateInvalid(DateTime startDateTime, DateTime endDateTime)
        {
            return endDateTime.Date < startDateTime.Date;
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
            decimal midAmount = 0;

            var end = endDateTime.ToString("yyyyMM");

            var currentMonth = startDateTime.AddMonths(1);

            while (currentMonth.ToString("yyyyMM").CompareTo(end) < 0)
            {
                var startPeriodString = currentMonth.ToString("yyyyMM");
                var budget = budgets.FirstOrDefault(o => o.YearMonth == startPeriodString);
                midAmount += budget?.Amount ?? 0;
                currentMonth = currentMonth.AddMonths(1);
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