using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat
{
    public class UI
    {        
        private LogicAndData _atm;
        private DataRepository _dataRepository;
        public UI(LogicAndData atm, DataRepository dataRepository) 
        {
            _atm = atm;
            _dataRepository = dataRepository;
        }
        /// <summary>
        /// Login menu. First thing users sees when opening the program.
        /// </summary>
        public void MainMenu()
        {
            while (true)
            {
                #region MainMenu text
                Console.Clear();
                Console.WriteLine("Hi and welcome to JETT and HAJ's ATM!");
                Console.WriteLine("Please chose an option in the menu below.\n");
                Console.WriteLine("[1]\tLog in");
                Console.WriteLine("[2]\tCreate a new account.");
                Console.WriteLine("[3]\tExit the program.");
                #endregion
                if (int.TryParse(Console.ReadLine(), out int userChoiceMainMenu))
                {
                    switch (userChoiceMainMenu)
                    {
                        case 1:
                            _atm.Login(this); //Lets user atempt to log in
                            break;
                        case 2:
                            _atm.CreateAccount(); //Lets user create an account.
                            break;

                        case 3:
                            _dataRepository.SaveBankAccountsToFile(_atm.allBankAccounts); //Saves current data 
                            Environment.Exit(0); //Closes the program.
                            break;

                        default: //error response
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Choose option 1, 2 or 3 from the list.");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                    }
                }
                else//error response
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Please insert a valid number.");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }
        /// <summary>
        /// Menu for when the user successfully logs in
        /// </summary>
        /// <param name="currentUserAccount"></param>
        public void AccountMenu(User currentUserAccount)
        {

            while (true)
            {
                #region AccountMenu text
                
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"Welcome {currentUserAccount.firstName} {currentUserAccount.lastName}. Please make choice:");
                Console.WriteLine();
                Console.WriteLine("[1]\tWithdraw money");
                Console.WriteLine("[2]\tDeposit money");
                Console.WriteLine("[3]\tTransfer money");
                Console.WriteLine("[4]\tCheck balance");
                Console.WriteLine("[5]\tTransfer history");
                Console.WriteLine("[6]\tLogout");
                #endregion
                if (int.TryParse(Console.ReadLine(), out int userChoiceAccountMenu))
                {
                    switch (userChoiceAccountMenu)
                    {
                        case 1:
                            _atm.WithdrawMoney(currentUserAccount); //Lets the user withdrawl money from account                           
                            break;

                        case 2:
                            _atm.DepositMoney(currentUserAccount); //Lets the user deposit money to account                          
                            break;

                        case 3:
                            _atm.TransferMoney(currentUserAccount); //Lets the user transfer money to a different account                            
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine($"\nYour current balance is {currentUserAccount.Balance:C}"); //Checks users current balance
                            Console.ReadKey();
                            break;
                        case 5:
                            _atm.SeeTransferHistory(currentUserAccount); //Lists up prrevious transactions to the user.                            
                            break;
                        case 6://logout message
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Logging you out. Have a pleasant rest of the day.");                         
                            Console.ReadKey();
                            return;
                        default://error response
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine("Choose option from the list.");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                    }
                }
                else//error response
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please input a valid number from the menu");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }
    }
}
