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
            var budgets = this._budgetRepo.GetAll();
            var budget = budgets.First(o => o.YearMonth == startDateTime.ToString("yyyyMM"));
            var budgetAmount = budget.Amount / 31;
            var date = (endDateTime.Date - startDateTime.Date).Days + 1;

            return budgetAmount * date;
        }
    }
}