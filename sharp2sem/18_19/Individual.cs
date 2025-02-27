using System;
using System.Text;
using static System.DateTime;

namespace sharp2sem._18_19
{
    [Serializable]
    public class Individual : Client, IDepositor, ILoaner
    {
        public string FirstName { get; }
        private const int DepositInterest = 22;
        private const int LoanInterest = 6;
        private bool _depositIsOpened;
        private bool _loanIsOpened;
        private DateTime _depositDate;

        public Individual(int clientId, string firstName) : base(clientId)
        {
            FirstName = firstName;
            RegistrationDate = Now;
        }

        public void OpenDeposit(decimal amount)
        {
            if (_depositIsOpened)
            {
                throw new Exception("Депозит уже открыт!");
            }

            Balance -= amount;
            DepositBalance += amount;
            _depositDate = Now;
            _depositIsOpened = true;
        }

        public void CloseDeposit()
        {
            if (!_depositIsOpened)
            {
                throw new Exception("Открытого депозита нет!");
            }

            Balance += DepositBalance * (decimal)(1 + DepositInterest / (IsLeapYear(Now.Year) ? 366.0 : 365.0) *
                _depositDate.Subtract(Now).TotalDays);
            DepositBalance = 0;
            _depositIsOpened = false;
        }

        public void TakeLoan(decimal amount)
        {
            if (_loanIsOpened)
            {
                throw new Exception("У вас уже есть кредит!");
            }

            decimal availableLoanAmount = Balance * 3;
            if (amount > availableLoanAmount)
            {
                throw new Exception($"Максимально доступный размер кредита: {availableLoanAmount}");
            }

            Balance += amount;
            LoanBalance += amount;
            _loanIsOpened = true;
        }

        public void PayLoan(decimal amount)
        {
            if (!_loanIsOpened)
            {
                throw new Exception("Вы не должник!");
            }

            LoanBalance +=
                (decimal)(_depositDate.Subtract(Now).TotalDays * (double)LoanBalance * (LoanInterest / 100.0));
            _depositDate = Now;
            Balance -= amount;
            LoanBalance -= amount;

            if (LoanBalance >= 0)
            {
                _loanIsOpened = false;
                Balance += LoanBalance;
                LoanBalance = 0;
            }
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Физ лицо");
            info.AppendLine($"Имя: {FirstName}");
            info.AppendLine($"Баланс: {Balance}");
            return info.ToString();
        }
    }
}