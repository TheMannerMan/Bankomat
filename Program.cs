using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Security;
using System.Xml;
using Bankomat; //Makes sure the entire solution uses all files.


//creating new instances
DataRepository dataRepository = new DataRepository();
LogicAndData ATM = new LogicAndData();


UI atmUI = new UI(ATM, dataRepository); //Creates a new instance with the 2 previous objects.
atmUI.MainMenu();// Starts the program with the MainMenu.

public class User
{
    public string firstName;
    public string lastName;
    public string Password { get; }

    List<Bankomat.User> usersBankAccounts = new List<Bankomat.User>();

    public User(string firstName, string lastName, string password)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.Password = password;
    }
}






