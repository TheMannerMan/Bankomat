using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat
{
    /// <summary>
    /// Saves all into onto a json file
    /// </summary>
    public class DataRepository
    {
        private const string AccountsFileName = "accounts.json";

        public void SaveBankAccountsToFile(List<User> accounts)
        {
            var json = JsonConvert.SerializeObject(accounts); //Takes the list of accounts, and serializing it as a var type
            File.WriteAllText(AccountsFileName, json); //Saves the json variable into a json file. 
        }
        /// <summary>
        /// loads all data from a json file.
        /// </summary>
        /// <returns></returns>
        public List<User> LoadUserAccountsFromFile()
        {
            if (File.Exists(AccountsFileName)) //checks if the file exsists before trying to load it
            {
                var json = File.ReadAllText(AccountsFileName); //creates a var type and reads all contents of the json file onto it
                return JsonConvert.DeserializeObject<List<User>>(json); //Deserializing the variable and returns a list filled with all account information.
            }
            return new List<User>()
        {
            new User("Test", "Testare", "test", 11111)
        };
        }
    }
}
