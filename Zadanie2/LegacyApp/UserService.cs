using System;

namespace LegacyApp
{
    public class UserService(UserCreditService userCreditService)
    {
        public UserService() : this(new UserCreditService())
        {
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateFullName(firstName, lastName) || !ValidateEmail(email) || !ValidateAge(dateOfBirth, 21))
                return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User(client, dateOfBirth, email, firstName, lastName);
            
            var calculateCreditLimit = SelectProperCalculator(client);
            var creditLimit = calculateCreditLimit.CalculateLimit(lastName, dateOfBirth);
            
            user.AssignLimit(creditLimit);
            if (user.IsNotSatisfyThreshold(500))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private ICalculateCreditLimit SelectProperCalculator(Client client)
        {
            return client.Type switch
            {
                "VeryImportantClient" => new NoLimitCalculator(),
                "ImportantClient" => new ExtraCalculator(userCreditService),
                _ => new SimpleCalculator(userCreditService)
            };
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

        private bool ValidateAge(DateTime dateOfBirth, int ageThreshold)
        {
            int age = CalculateAge(dateOfBirth);
            return age >= ageThreshold;
        }
    }
}