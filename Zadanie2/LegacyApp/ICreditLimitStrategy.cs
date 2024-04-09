using System;

namespace LegacyApp;

public interface ICreditLimitStrategy
{
    int GetCreditLimit(string lastName, DateTime dateOfBirth);
}