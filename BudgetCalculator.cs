using System;
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
            if (startDateTime.Month == endDateTime.Month)
            {
                return this.GetSameMonthAmount(startDateTime, endDateTime);
            }

            var budgets = this._budgetRepo.GetAll();

            var startBudget = budgets.FirstOrDefault(o => o.YearMonth == startDateTime.ToString("yyyyMM"));
            var endBudget = budgets.LastOrDefault(o => o.YearMonth == endDateTime.ToString("yyyyMM"));

            if (startBudget == null)
            {
                return 0;
            }

            var startAmount = startBudget.Amount / DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month);
            var endAmount = endBudget.Amount / DateTime.DaysInMonth(endDateTime.Year, endDateTime.Month);

            var startBudgetAmount = startAmount *
                                    (DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month) - startDateTime.Day +
                                     1);

            var endBudgetAmount = endAmount * endDateTime.Day;

            var start = startDateTime.AddMonths(1);
            //var end = endDateTime.AddMonths(-1);

            var orderedEnumerable = budgets.OrderBy(o => o.YearMonth);

            decimal midAmount = 0;
            for (var i = start; i.Date < endDateTime.Date; i = i.AddMonths(1))
            {
                var budget = budgets.FirstOrDefault(o => o.YearMonth == i.ToString("yyyyMM"));
                midAmount += budget.Amount;
            }

            return startBudgetAmount + endBudgetAmount + midAmount;
        }

        private decimal GetSameMonthAmount(DateTime startDateTime, DateTime endDateTime)
        {
            var budgets = this._budgetRepo.GetAll();
            var budget = budgets.FirstOrDefault(o => o.YearMonth == startDateTime.ToString("yyyyMM"));
            if (budget == null)

            {
                return 0;
            }

            var budgetAmount = budget.Amount / DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month);
            var date = (endDateTime.Date - startDateTime.Date).Days + 1;

            return budgetAmount * date;
        }
    }
}