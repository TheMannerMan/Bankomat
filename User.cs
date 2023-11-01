using Bankomat;

public class User
{
    public string firstName;
    public string lastName;
    public string Password { get; }
    public List<BankAccount> userBankAccounts = new List<BankAccount>();

    public User(string firstName, string lastName, string password, int generatedBankAccountNumber)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.Password = password;
        userBankAccounts.Add(new BankAccount(generatedBankAccountNumber));
    }
}