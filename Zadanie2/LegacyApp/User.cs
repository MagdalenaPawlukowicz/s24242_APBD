﻿using System;

namespace LegacyApp
{
    public class User
    {
        public object Client { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
        public string EmailAddress { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public bool HasCreditLimit { get; internal set; }

        public int CreditLimit { get; internal set; }

        public void AssignLimit(int limit)
        {
            if (limit > 0)
            {
                HasCreditLimit = true;
            }

            CreditLimit = limit;
        }

        public User()
        {
        }

        public User(object client, DateTime dateOfBirth, string emailAddress, string firstName, string lastName)
        {
            Client = client;
            DateOfBirth = dateOfBirth;
            EmailAddress = emailAddress;
            FirstName = firstName;
            LastName = lastName;
        }

        public bool IsNotSatisfyThreshold(int threshold)
        {
            return HasCreditLimit && CreditLimit < threshold;
        }
    }
}