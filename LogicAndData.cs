using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat
{
    public class LogicAndData
    {        
        //When the program starts and an instance of LogicAndData is created. It will retreive a list of bankaccounts thats saved externally
        public List<User> allUserAccounts = new DataRepository().LoadUserAccountsFromFile();
        private Random random = new Random();

        /// <summary>
        /// Will start a login procedure. Will check if account number exists and password matches.
        /// </summary>
        /// <param name="atmUI"></param>
        public void Login(UI atmUI)
        {
            Console.Clear();
            Console.WriteLine("You are logging in.");
            Console.WriteLine();            
            User currentUser = null;
            while (true)
            {
                Console.Write("Type the name of the recipient: ");
                string nameOfRecipientAccount = Console.ReadLine().Trim();
                string[] temp = nameOfRecipientAccount.Split(' '); //splits "nameOfRecipientAccount" from 1 string into 2 elements in an array.
                                                                   //Checks to see if the recipients first and last name matches account number
                if (temp.Length != 2 || temp[0] != recepientAccount.firstName || temp[1] != recepientAccount.lastName) //If the match is false, you get the error message inside the statement.
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Account number and name don't match. Please try again.");
                    Console.ResetColor();
                    continue;
                }


                Console.Write("Type in your full name:");


                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    currentUser = GetBankAccountByAccountNumber(result); //matches user input against saved account numbers.
                    if (currentUser != null)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Account found.");
                        Console.ResetColor();
                        string loginPassword = PasswordGenerator(); //Gets the password from the user, using the same method when creating a new password.
                        if (PasswordCheck(result, loginPassword))//matches account number to password
                        {
                            atmUI.AccountMenu(currentUser); //Takes user to next menu if successfull
                        }
                        else //error response
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid password");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                        break;
                    }
                    else//error response
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Account not found.");
                        Console.ResetColor();
                        Console.WriteLine("Press \"Enter\" to return to main menu");
                        Console.WriteLine();
                        Console.ReadKey();
                        break;
                    }
                }
                else//error response
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Numbers only.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ReadKey();
                }
            }
        }
        /// <summary>
        /// Lets the user create a new password. It will show as stars "*" for the user, but the program will understand whatever the user inputs and save it as a string.
        /// </summary>
        /// <returns>Returns the users password</returns>
        public string PasswordGenerator()
        {
            Console.Write("\nPassword: ");
            string password = ""; //Declaring an empty string.

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) //Will break the loop and return the password when pressing "Enter" key
                {
                    Console.WriteLine();
                    return password;
                }

                if (key.Key == ConsoleKey.Backspace) //Works like a regular Backspace keypress. Removes the latest char
                {
                    if (password.Length > 0) //Double checks if there actually is a char to remove.
                    {
                        password = password.Remove(password.Length - 1); //Removes the latest char.
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    //This replaces the visual char as a star "*" for the user. Doesnt affect the password. 
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
        }

        #region Methods used to check and get existing data
        /// <summary>
        /// Loops through saved accounts in "allBankAccouts" list. Trying to match user input to saved account numbers.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>Recipient account number</returns>
        public User GetBankAccountByAccountNumber(int accountNumber)
        {
            User bankAccountToReturn = null;
            foreach (User bankAccount in allBankAccounts) //Loops through all saved accounts
            {
                if (bankAccount.accountNumber == accountNumber) //Checks user input for exact matches 
                {
                    bankAccountToReturn = bankAccount; //sets and returns if match is found
                    return bankAccountToReturn;
                }
            }
            return bankAccountToReturn; // Account not found
        }
        /// <summary>
        /// Lets user input an account number thats matching currently saved accounts for a transfer.
        /// </summary>
        /// <returns>recipients account number as a class</returns>
        private User GetRecipientAccount()
        {
            User recepientAccount = null; //set recipient account as null, local variable

            while (true)
            {
                
                Console.Write("Please enter the account number of the recipient: ");
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    recepientAccount = GetBankAccountByAccountNumber(result); //Checks users input to saved account numbers.
                    if (recepientAccount != null) //If match is found.
                    {
                        Console.Write("Type the name of the recipient: ");
                        string nameOfRecipientAccount = Console.ReadLine().Trim();
                        string[] temp = nameOfRecipientAccount.Split(' '); //splits "nameOfRecipientAccount" from 1 string into 2 elements in an array.
                        //Checks to see if the recipients first and last name matches account number
                        if (temp.Length != 2 || temp[0] != recepientAccount.firstName || temp[1] != recepientAccount.lastName) //If the match is false, you get the error message inside the statement.
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Account number and name don't match. Please try again.");
                            Console.ResetColor();
                            continue;
                        }
                        return recepientAccount; //if the match is exact, it will return name and account number.
                    }
                    else//error response if account wasnt found or user input wrong numbers.
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Recipient account not found. Please try again.\n"); 
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
                else//error response
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Invalid input.");
                    Console.ResetColor();
                    Console.WriteLine("Press \"Enter\" to return to main menu");
                    Console.WriteLine();
                    Console.ReadKey();
                    return null;
                }               
            }
        }
        /// <summary>
        /// Compares users account number to his/hers password.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>  
        public bool PasswordCheck(int result, string loginPassword)
        {
            foreach (User bankAccount in allBankAccounts)//Loops through all saved accounts
            {
                if (bankAccount.accountNumber == result && bankAccount.Password == loginPassword) //Matches users input of account number and password against saved accouts
                {
                    return true;//if all matches, returns true = successful login
                }
            }
            return false;//if either one doesnt match, returns false = unsuccessful login.
        }
        /// <summary>
        /// Checks to see if there's an account with the exact name.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>User already exists = true</returns>
        public bool UserExistCheck(string firstName, string lastName)
        {
            foreach (User bankAccount in allBankAccounts) //Loops through all saved accounts
            {
                if (bankAccount.firstName.ToLower() == firstName.ToLower() && bankAccount.lastName.ToLower() == lastName.ToLower()) //Compares user input to saved accounts.
                {
                    Console.WriteLine($"User {firstName} {lastName} already exist. Account number is {bankAccount.accountNumber}.");
                    return true; //if person exists, returns true
                }
            }
            return false; //If person doesnt exist, return false. Lets user create new account.
        }
        #endregion

        #region Create account methods
        /// <summary>
        /// Generates a random number between 10,000-99,999. Doublechecks current account numbers saved in "allBankAccounts"-list to create a new unique number.
        /// </summary>
        /// <returns>BankAccount.accountNumber</returns>
        public int GenerateAccountNumber()
        {
            int newAccountNumber;
            do
            {
                newAccountNumber = random.Next(10000, 99999); //rolls a number and saves it in a variable.
                //Compares current value of "newAccountNumber" to other account number saved in the list. Loops again if a match is found. Continues until a unique number is found.
            } while (allBankAccounts.Any(BankAccount => BankAccount.accountNumber == newAccountNumber));
            return newAccountNumber;
        }
        /// <summary>
        /// Lets the user create a new account. First name, last name, password + randomly generated account number.
        /// Saves the account in a list containing all the info using a constructor in BankAccount class.
        /// </summary>
        public void CreateAccount()
        {           
            Console.Clear();
            Console.WriteLine();           
            Console.WriteLine("You are creating a new account.");
            Console.WriteLine();
            Console.Write("Please type you first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Please type you last name: ");
            string lastName = Console.ReadLine();
            if (!UserExistCheck(firstName, lastName)) //Checks to see if user already exists.
            {
                string password = PasswordGenerator(); //Lets user create a new password
                int accountNumber = GenerateAccountNumber(); //Gives the user a randomly generated 5-digit account number. 
                User newBankAccount = new User(firstName, lastName, password, accountNumber);
                allBankAccounts.Add(newBankAccount); //Saves the newly constructed account to the list.
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Bank account created!" +
                    $"\nAccount owner: {firstName} {lastName}" +
                    $"\nAccount number: {accountNumber}" +
                    $"\nAccount password: {password}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\nPlease save your account info for later use!");
                Console.WriteLine("Press \"Enter\" to proceed");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
        #endregion

        #region Transfer methods
        /// <summary>
        /// Asks the user to deposit money into his/her account. Adds new value onto the previous value and saves it.
        /// </summary>
        /// <param name="currentUserAccount"></param>
        public void DepositMoney(User currentUserAccount)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("\nPlease enter the amount you would like to deposit: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount)) //Checks to see if user inputs a number
                {
                    if (amount >= 1) //Checks to see if user tries to deposit a value greater than 0
                    {
                        currentUserAccount.Balance += amount; //Old balance + deposit = new balance
                        Console.WriteLine();
                        Console.WriteLine($"{amount:C} have successfully been added to your account.");
                        currentUserAccount.SaveEventToTransferHistory("Deposit", amount); //saves new balance to users account.
                        Console.ReadKey();
                        return;
                    }
                    else if (amount == 0) //error response
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Cant deposit 0 kr.");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                     
                    else //error response
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You can't deposit negative numbers.");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
                else //error response
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid input. Please enter a valid amount.\n");
                    Console.ResetColor();
                    Console.WriteLine("Press \"Enter\" to proceed");
                    Console.ReadKey();
                    return;
                }
            }
        }
        /// <summary>
        /// Shows all previous transfer history to the user
        /// </summary>
        /// <param name="currentUserAccount"></param>
        public void SeeTransferHistory(User currentUserAccount)

        {       
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Nr\tType".PadRight(15) + "\tAmount".PadRight(10) + "\tBalance".PadRight(10) + "\tDate and time");
            Console.WriteLine("----------------------------------------------------------------------------");

            for (int i = 0; i < currentUserAccount.transferHistorik.Count; i++) //Loops through saved transferhistory, 10 transfers = 10 loops.
            {
                // Hämtar ut varje specifik händelse som en separat sträng i listan transferHistorik.
                string historikAttSkrivaUt = currentUserAccount.transferHistorik[i]; //copies everything saved in the list onto a single string.
                string[] historikArray = historikAttSkrivaUt.Split(new char[] { ',' }); //Splits and seperates the single string into an array with (in this case) 4 elements.
                //Prints out the array in a stylish fashion.
                Console.WriteLine($"{i + 1}\t{historikArray[0].PadRight(10)}\t{historikArray[1].PadRight(15)}\t{historikArray[2].PadRight(15)}\t{historikArray[3]}");
              
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press \"Enter\" to return");
            Console.ResetColor();
            Console.ReadKey();
        }
        /// <summary>
        /// Lets current user transfer money to a different account. Using first name, last name and account number of the recipient.
        /// </summary>
        /// <param name="currentUserAccount"></param>
        public void TransferMoney(User currentUserAccount)
        {
            Console.Clear();
            Console.WriteLine();
            User recepientAccount = GetRecipientAccount(); //gets all info from recipient.        

            Console.Write("Enter the amount you would like to  transfer: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount == 0)
                {
                    Console.WriteLine("Cant transfer nothing.");
                    return;
                }
                if (amount <= 0) //Makes sure you cant steal from recipient.
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("You can't transfer a non-positive amount.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
                if (amount > currentUserAccount.Balance) //Makes sure you can afford the transfer
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine($"Your balance ({currentUserAccount.Balance:C}) is too low for the requested transfer.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
                currentUserAccount.Balance -= amount; //current user loses x-amount of money
                recepientAccount.Balance += amount; //recipient gains x-amount of money
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine($"You have transferred {amount:C} to {recepientAccount.firstName} {recepientAccount.lastName}.");
                Console.ResetColor();
                Console.ReadKey();
                //saves the new balance for each account. Current user and Recipient.               
                currentUserAccount.SaveEventToTransferHistory($"Trans > {recepientAccount.accountNumber}", amount);              
                recepientAccount.SaveEventToTransferHistory($"Trans < {currentUserAccount.accountNumber}", amount);                
            }
            else //Error response
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid input. Please enter a valid amount.\n");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
        }
        /// <summary>
        /// Lets the user input an amount to withdraw from the account.
        /// </summary>
        /// <param name="currentUserAccount"></param>
        public void WithdrawMoney(User currentUserAccount)
        {
            Console.Clear();
            Console.Write("\nPlease input the amount you would like to withdraw: ");
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out decimal amount)) //Checks to see if the user inputs a number
                {
                    if (amount == 0) //respons if user tries to withdraw nothing
                    {
                        Console.WriteLine("Cant withdraw nothing.");
                        Console.ReadKey();
                        return;
                    }
                    if (amount < 0) //response if user tries to add funds through negative means.
                    {
                        Console.WriteLine("Cant withdraw a negative amount.");
                        Console.ReadKey();
                        return;
                    }
                    if (amount <= currentUserAccount.Balance) //makes sure the amount withdrawl doesnt not exceed current balance.
                    {
                        currentUserAccount.Balance -= amount;
                        Console.WriteLine();
                        Console.WriteLine($"You withdrew {amount:C}");
                        currentUserAccount.SaveEventToTransferHistory("Withdrawal", amount); //saves new balance
                        Console.ReadKey();
                        return;
                    }
                    else //error response
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Your withdrawl request ({amount}) exceeds your current balance: ({currentUserAccount.Balance:C}).");
                        Console.ResetColor();
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid input. Please use numericals\n");
                    Console.ResetColor();
                    Console.WriteLine("Press \"Enter\" to proceed");
                    Console.ReadKey();
                    return;
                }
            }
        }
        #endregion
    }
}


/* SPARAR DENNA IFALL JAG FUCKAR UR HELT MED MINA ÄNDRINGAR. SLÄNG DETTA OM JAG FÅR DET ATT FUNKA.
public void Login(UI atmUI)
        {
            Console.Clear();
            Console.WriteLine("You are logging in.");
            Console.WriteLine();            
            BankAccount currentUser = null;
            while (true)
            {
                Console.Write("Type in your account number:");
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    currentUser = GetBankAccountByAccountNumber(result); //matches user input against saved account numbers.
                    if (currentUser != null)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Account found.");
                        Console.ResetColor();
                        string loginPassword = PasswordGenerator(); //Gets the password from the user, using the same method when creating a new password.
                        if (PasswordCheck(result, loginPassword))//matches account number to password
                        {
                            atmUI.AccountMenu(currentUser); //Takes user to next menu if successfull
                        }
                        else //error response
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid password");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                        break;
                    }
                    else//error response
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Account not found.");
                        Console.ResetColor();
                        Console.WriteLine("Press \"Enter\" to return to main menu");
                        Console.WriteLine();
                        Console.ReadKey();
                        break;
                    }
                }
                else//error response
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Numbers only.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ReadKey();
                }
            }
        }
*/