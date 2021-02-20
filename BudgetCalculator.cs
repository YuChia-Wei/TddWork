using System;

namespace TddWork
{
    public class BudgetCalculator
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetCalculator(IBudgetRepo budgetRepo)
        {
            this._budgetRepo = budgetRepo;
            throw new NotImplementedException();
        }

        public decimal Query(DateTime startDateTime, DateTime endDateTime)
        {
            throw new NotImplementedException();
        }
    }
}