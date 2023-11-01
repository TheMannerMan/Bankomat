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
        public User GetExistingUserAtLogin()
        {
            Console.Write("Type your full name: ");
            string nameOfUser = Console.ReadLine().Trim();
            string[] temp = nameOfUser.Split(' '); //splits "nameOfRecipientAccount" from 1 string into 2 elements in an array.

            //TODO: Hur hanterar vi namn med fler än 2 namn? T.ex. Otto Von Snorre?
            foreach (User user in allUserAccounts)
            {
                if (temp[0] == user.firstName && temp[1] == user.lastName)
                {
                    return user;
                }

            }
            return null;
        }
        public User Login(UI atmUI)
        {
            Console.Clear();
            Console.WriteLine("You are logging in.");
            Console.WriteLine();
            User ActiveUserLoggingIn = null;
            //User currentUser = null; BEHÖVER DEN HÄR VARA MED LÄNGRE EFTER ÄNDRINGAR?
            while (true)
            {
                
                ActiveUserLoggingIn = GetExistingUserAtLogin();

                if (ActiveUserLoggingIn != null)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Account found.");
                    Console.ResetColor();
                    if (PasswordCheck(ActiveUserLoggingIn, PasswordGenerator()))//Gets the password from the user, using the same method when creating a new password and the control if its a match with the password saved in database.
                    {
                        //TODO: här behöver vi ett steg där vi kontrollerar ifall användaren har flera än ett konto. 
                        return ActiveUserLoggingIn; //Takes user to next menu if successfull TODO: ÄR DENNA KOMMENTAR FORTFARANDE RELEVANT?
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
            return ActiveUserLoggingIn;
        }
        // TODO: skriv kommentarer till denna
        public BankAccount LetUserChooseBankAccount(User loginUser)
        {
            Console.WriteLine("You have multiple bank accounts.");
            while (true)
            {
                for (int i = 0; i < loginUser.userBankAccounts.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {loginUser.userBankAccounts[i].accountNumber}");
                }
                Console.Write("Please choose a bankaccount for login:");
                if (Int32.TryParse(Console.ReadLine(), out int userChoice))
                {
                    if (userChoice > 0 && userChoice <= loginUser.userBankAccounts.Count)
                    {
                        return loginUser.userBankAccounts[userChoice - 1];
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Choice out of range.");
                        continue;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    continue;
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
        public bool CheckIfMultipleBankAccount(User currentUser)
        {
            if (currentUser.userBankAccounts.Count > 1)
            {
                return true;
            }
            else { return false; }
        }
        /// <summary>
        /// Loops through saved accounts in "allBankAccouts" list. Trying to match user input to saved account numbers.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>Recipient account number</returns>
        public (User, BankAccount) GetBankAccountByAccountNumber(int searchingAccountNumber)
        {
            User nonExistingUserAccountToReturn = null;
            BankAccount nonExistingBankAccountToReturn = null;

            foreach (User user in allUserAccounts) //Loops through all saved accounts
            {
                foreach(BankAccount bankAccount in user.userBankAccounts)
                {
                    if (bankAccount.accountNumber == searchingAccountNumber) //Checks user input for exact matches 
                    {
                        
                        return (user, bankAccount); //returns a tuple of the current bankaccount and useraccount
                    }
                }
                
            }
            return (nonExistingUserAccountToReturn, nonExistingBankAccountToReturn);
        }
        /// <summary>
        /// Lets user input an account number thats matching currently saved accounts for a transfer.
        /// </summary>
        /// <returns>recipients account number as a class</returns>
        private (BankAccount, User) GetRecipientBankAccountAndUserAccount()
        {
           //(User, BankAccount) recepientBankAccount = (null, null); //set recipient account as null, local variable

            while (true)
            {

                Console.Write("Please enter the account number of the recipient: ");
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    //Checks users input to saved account and banknumbers. If they exist they are saved as a tuple.
                    var (recepientUserAccount, recepientBankAccount) = GetBankAccountByAccountNumber(result);
                    if (recepientUserAccount != null && recepientBankAccount != null) //If match is found.
                    {
                        Console.Write("Type the name of the recipient: ");
                        string nameOfRecipientAccount = Console.ReadLine().Trim();
                        string[] temp = nameOfRecipientAccount.Split(' '); //splits "nameOfRecipientAccount" from 1 string into 2 elements in an array.
                        //Checks to see if the recipients first and last name matches account number TODO: Hur hanterar vi namn på t.ex. tre ord? T.ex. Otto von Snorre.
                        if (temp.Length != 2 || temp[0] != recepientUserAccount.firstName || temp[1] != recepientUserAccount.lastName) //If the match is false, you get the error message inside the statement.
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Account number and name don't match. Please try again.");
                            Console.ResetColor();
                            continue;
                        }
                        return (recepientBankAccount, recepientUserAccount); //if the match is exact, it will return name and account number.
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Invalid input.");
                    Console.ResetColor();
                    Console.WriteLine("Press \"Enter\" to return to main menu");
                    Console.WriteLine();
                    Console.ReadKey();
                    return (null, null);
                }
            }
        }

        public bool PasswordCheck(User userLoggingIN, string loginPassword)
        {

            if (userLoggingIN.Password == loginPassword) // Controlls if password given by the user logging in matches the password saved to the system.
            {
                return true;// successful login
            }
            return false;// unsuccessful login.
        }
        /// <summary>
        /// Checks to see if there's an account with the exact name.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>User already exists = true</returns>
        public bool UserExistCheck(string firstName, string lastName)
        {
            foreach (User user in allUserAccounts) //Loops through all saved accounts
            {
                if (user.firstName.ToLower() == firstName.ToLower() && user.lastName.ToLower() == lastName.ToLower()) //Compares user input to saved accounts.
                {
                    Console.WriteLine($"User {firstName} {lastName} already exist.");
                    return true; //if person exists, returns true
                }
            }
            return false; //If person doesnt exist, return false. Lets user create new account.
        }
                
        #endregion

        #region Create account methods
        /// <summary>
        /// Generates a random number between 10,000-99,999. Doublechecks current saved account numbers to create a new unique number.
        /// </summary>
        /// <returns>BankAccount.accountNumber</returns>
        public int GenerateAccountNumber()
        {
            int newAccountNumber;
            bool isUnique;

            do
            {
                newAccountNumber = random.Next(10000, 99999); // Generate a new account number.

                // Check if the newAccountNumber is unique across all user accounts and their bank accounts.
                isUnique = !allUserAccounts.Any(user => user.userBankAccounts.Any(account => account.accountNumber == newAccountNumber));

            } while (!isUnique);

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
                int newAccountNumber = GenerateAccountNumber(); // Generates an unique number for the users first account.
                User newUserAccount = new User(firstName, lastName, password, newAccountNumber);
                allUserAccounts.Add(newUserAccount); //Saves the newly constructed account to the list.
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Bank account created!" +
                    $"\nAccount owner: {firstName} {lastName}" +
                    $"\nAccount number: {newAccountNumber}" +
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
        public void DepositMoney(BankAccount currentBankAccount)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("\nPlease enter the amount you would like to deposit: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount)) //Checks to see if user inputs a number
                {
                    if (amount >= 1) //Checks to see if user tries to deposit a value greater than 0
                    {
                        currentBankAccount.Balance += amount; //Old balance + deposit = new balance
                        Console.WriteLine();
                        Console.WriteLine($"{amount:C} have successfully been added to your account.");
                        currentBankAccount.SaveEventToTransferHistory("Deposit", amount); //saves new balance to users account.
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
        public void SeeTransferHistory(BankAccount currentBankAccount)

        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Nr\tType".PadRight(15) + "\tAmount".PadRight(10) + "\tBalance".PadRight(10) + "\tDate and time");
            Console.WriteLine("----------------------------------------------------------------------------");

            for (int i = 0; i < currentBankAccount.transferHistorik.Count; i++) //Loops through saved transferhistory, 10 transfers = 10 loops.
            {
                // Hämtar ut varje specifik händelse som en separat sträng i listan transferHistorik.
                string historikAttSkrivaUt = currentBankAccount.transferHistorik[i]; //copies everything saved in the list onto a single string.
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
        public void TransferMoney(BankAccount currentBankAccount)
        {
            Console.Clear();
            Console.WriteLine();
            var (recepientBankAccount, recepientUserAccount) = GetRecipientBankAccountAndUserAccount(); //gets all info from recipient.        

            if (recepientBankAccount != null && recepientUserAccount != null)
            {
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You can't transfer a non-positive amount.");
                        Console.ResetColor();
                        Console.ReadKey();
                        return;
                    }
                    if (amount > currentBankAccount.Balance) //Makes sure you can afford the transfer
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Your balance ({currentBankAccount.Balance:C}) is too low for the requested transfer.");
                        Console.ResetColor();
                        Console.ReadKey();
                        return;
                    }
                    currentBankAccount.Balance -= amount; //current user loses x-amount of money
                    recepientBankAccount.Balance += amount; //recipient gains x-amount of money
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine($"You have transferred {amount:C} to {recepientUserAccount.firstName} {recepientUserAccount.lastName}.");
                    Console.ResetColor();
                    Console.ReadKey();
                    //saves the new balance for each account. Current user and Recipient.               
                    currentBankAccount.SaveEventToTransferHistory($"Trans > {recepientBankAccount.accountNumber}", amount);
                    recepientBankAccount.SaveEventToTransferHistory($"Trans < {currentBankAccount.accountNumber}", amount);
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
            
        }
        /// <summary>
        /// Lets the user input an amount to withdraw from the account.
        /// </summary>
        /// <param name="currentUserAccount"></param>
        public void WithdrawMoney(BankAccount currentBankAccount)
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
                    if (amount <= currentBankAccount.Balance) //makes sure the amount withdrawl doesnt not exceed current balance.
                    {
                        currentBankAccount.Balance -= amount;
                        Console.WriteLine();
                        Console.WriteLine($"You withdrew {amount:C}");
                        currentBankAccount.SaveEventToTransferHistory("Withdrawal", amount); //saves new balance
                        Console.ReadKey();
                        return;
                    }
                    else //error response
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Your withdrawl request ({amount}) exceeds your current balance: ({currentBankAccount.Balance:C}).");
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