using System;

namespace LegacyApp
{
    public class UserService
    {

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            
            if (!ValidateFullName(firstName, lastName) || !ValidateEmail(email) || !ValidateAge(dateOfBirth))
                return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
                
            var user = new User(client, dateOfBirth, email, firstName, lastName);
            
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
            
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }
        
        private bool ValidateFullName(string firstName, string lastName)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName);
        }

        private bool ValidateEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && (email.Contains("@") || email.Contains("."));
        }

        private bool ValidateAge(DateTime dateOfBirth)
        {
            int age = CalculateAge(dateOfBirth);
            return age >= 21;
        }
    }
}
