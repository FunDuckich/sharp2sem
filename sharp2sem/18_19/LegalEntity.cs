using System;
using System.Collections.Generic;
using System.Text;
using static System.DateTime;

namespace sharp2sem._18_19
{
    [Serializable]
    public class LegalEntity : Client, IDepositor, ILoaner
    {
        private string NameOfOrganisation { get; }
        private const int DepositInterest = 29;
        private const int LoanInterest = 3;
        private bool _depositIsOpened;
        private bool _loanIsOpened;
        private DateTime _depositDate;
        private List<Individual> _workers = new List<Individual>();
        private decimal WorkerSalary => Balance * (decimal)0.4 / _workers.Count;

        public LegalEntity(int clientId, string nameOfOrganisation) : base(clientId)
        {
            NameOfOrganisation = nameOfOrganisation;
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

            decimal availableLoanAmount = Balance * 7;
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

        public void Hire(Individual newWorker)
        {
            _workers.Add(newWorker);
        }

        public void PaymentForWorkers()
        {
            if (_workers.Count < 0)
            {
                throw new Exception("Нет работников!");
            }

            foreach (Individual worker in _workers)
            {
                worker.TopUp(WorkerSalary);
                worker.Withdraw(WorkerSalary);
            }
        }

        private string GetWorkersNames()
            {
                StringBuilder names = new StringBuilder();
                foreach (Individual worker in _workers)
                {
                    if (names.Length > 0)
                    {
                        names.Append(", ");
                    }

                    names.Append(worker.FirstName);
                }

                return names.ToString();
            }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Юр лицо");
            info.AppendLine($"Наименование: {NameOfOrganisation}");
            info.AppendLine($"Баланс: {Balance}");
            info.AppendLine($"Работники: {GetWorkersNames()}");
            return info.ToString();
        }
    }
}