using System;

namespace sharp2sem._18_19
{
    [Serializable]
    public abstract class Client
    {
        public int ClientId { get; protected set; }
        private decimal _balance;
        private decimal _depositBalance;

        protected decimal Balance
        {
            get => _balance;
            
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Баланс не может быть меньше нуля!");
                }
                _balance = value;
            }
        }

        protected decimal DepositBalance
        {
            get => _depositBalance;
            
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Депозит не может быть меньше нуля!");
                }
                _depositBalance = value;
            }
        }
        
        protected decimal LoanBalance { get; set; }
        protected DateTime RegistrationDate;

        protected Client(int clientId) 
        {
            ClientId = clientId;
        }

        public void TopUp(decimal amount)
        {
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            Balance -= amount;
        }
    }
}