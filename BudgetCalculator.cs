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

            return 0;

            //var budgets = this._budgetRepo.GetAll();
            //var startBudget = budgets.FirstOrDefault(o => o.YearMonth == startDateTime.ToString("yyyyMM"));
            //var endBudget = budgets.LastOrDefault(o => o.YearMonth == endDateTime.ToString("yyyyMM"));

            //if (startBudget == null)
            //{
            //    return 0;
            //}

            //var startAmount = startBudget.Amount / DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month);
            //var endAmount = endBudget.Amount / DateTime.DaysInMonth(endDateTime.Year, endDateTime.Month);

            //var startBudgetAmount = startAmount * startDateTime.d
            //var startDate = (endDateTime.Date - startDateTime.Date).Days + 1;

            //return budgetAmount * date;
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