using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMClassLibrary
{
    public class Account
    {
        public string CardNumber { get; set; }
        public string OwnerName { get; set; }
        public decimal Balance { get; private set; }
        private string PinCode;

        public Account(string cardNumber, string ownerName, decimal initialBalance, string pinCode)
        {
            CardNumber = cardNumber;
            OwnerName = ownerName;
            Balance = initialBalance;
            PinCode = pinCode;
        }

        public bool Authenticate(string cardNumber, string pinCode)
        {
            return CardNumber == cardNumber && PinCode == pinCode;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сума поповнення повинна бути позитивною.");
            }

            Balance += amount;
            LogTransaction($"Поповнено: {amount} грн.");
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сума зняття повинна бути позитивною.");
            }
            if (Balance >= amount)
            {
                Balance -= amount;
                LogTransaction($"Знято: {amount} грн.");
                return true;
            }
            else
            {
                throw new InvalidOperationException("Недостатньо коштів на рахунку.");
            }
        }

        private void LogTransaction(string message)
        {
            // Тут може бути логіка для запису в файл або базу даних
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
