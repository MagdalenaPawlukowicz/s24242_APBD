using System;

namespace LegacyApp
{
    public interface ICalculateCreditLimit
    {
        int CalculateLimit(string lastName, DateTime dateOfBirth);
    }
    
    public class NoLimitCalculator : ICalculateCreditLimit
    {
        public int CalculateLimit(string lastName, DateTime dateOfBirth)
        {
            return 0;
        }
    }

    public class SimpleCalculator(UserCreditService userCreditService) : ICalculateCreditLimit
    {
        public int CalculateLimit(string lastName, DateTime dateOfBirth)
        {
            return userCreditService.GetCreditLimit(lastName, dateOfBirth);
        }
    }
    public class ExtraCalculator(UserCreditService userCreditService) : ICalculateCreditLimit
    {
        public int CalculateLimit(string lastName, DateTime dateOfBirth)
        {
            return userCreditService.GetCreditLimit(lastName, dateOfBirth) * 2;
        }
    }
}
