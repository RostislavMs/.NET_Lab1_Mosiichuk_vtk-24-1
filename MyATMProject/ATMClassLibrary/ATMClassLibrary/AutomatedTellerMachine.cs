using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMClassLibrary
{
    public class AutomatedTellerMachine
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public decimal Cash { get; private set; }



        public AutomatedTellerMachine(string id, string location, decimal initialCash)
        {
            Id = id;
            Location = location;
            Cash = initialCash;


        }

        public bool WithdrawCash(decimal amount)
        {
            if (Cash >= amount)
            {
                Cash -= amount;
                return true;
            }


            return false;
        }

        public void DepositCash(decimal amount)
        {
            Cash += amount;

        }
    }
}
