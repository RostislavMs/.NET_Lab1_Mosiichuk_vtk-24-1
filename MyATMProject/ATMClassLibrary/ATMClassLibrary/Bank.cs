using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMClassLibrary
{
    public class Bank
    {
        public string Name { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AutomatedTellerMachine> Atms { get; set; }

        public Bank(string name)
        {
            Name = name;
            Accounts = new List<Account>();
            Atms = new List<AutomatedTellerMachine>();
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

        public Account FindAccountByCardNumber(string cardNumber)
        {
            return Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
        }

        public Account GetAccountByCardNumber(string cardNumber)
        {
            return Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
        }
    }
}
