using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat
{
    public class BankAccount
    {
        public decimal Balance { get; set; }
        public int accountNumber;
        /// <summary>
        /// Basic constructor for the creation of a new account.
        /// </summary>
        /// <param name="number"></param>
        public BankAccount(int number)
        {
            this.accountNumber = number;
            this.Balance = 0;
        }

        public List<string> transferHistorik = new List<string>(); //Holding all the transfer history.

        /// <summary>
        /// Saves all transactions in a List of strings. TypeOfTransfer, TransferAmount, NewBalance, DateTime
        /// </summary>
        /// <param name="typeOfTransfer"></param>
        /// <param name="transferAmount"></param>   
        public void SaveEventToTransferHistory(string typeOfTransfer, decimal transferAmount)
        {
            //TODO: SKAPA If-statements med Enums istället.


            if (typeOfTransfer == "Withdrawal" || typeOfTransfer == "Trans > ") //If money is removed from the account, it specifies it with a "-" sign prior to transferAmount
            {
                string stringToSave = $"{typeOfTransfer}, -{transferAmount}, {Balance},{DateTime.Now}";                
                transferHistorik.Insert(0, stringToSave);
            }
            else //If money is added to the account, it specifies it with a "+" sign prior to the transferAmount
            {
                string stringToSave = $"{typeOfTransfer}, +{transferAmount}, {Balance},{DateTime.Now}";                
                transferHistorik.Insert(0, stringToSave);
            }


            // 1.   Deposit     +500    500     2023.01.23
            // 2.   Withdraw    -200    300     2023.02.01
        }
    }
}
