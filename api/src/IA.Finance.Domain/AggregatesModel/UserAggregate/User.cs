using System;
using System.Collections.Generic;
using System.Linq;
using IA.Finance.Domain.Exceptions;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        private string _firstName;

        private string _lastName;

        private string _userName;

        private string _email;

        private string _identityId;

        private string _role;
        
        private readonly List<RefreshToken> _refreshTokens;

        public string FirstName => _firstName;

        public string LastName => _lastName;

        public string UserName => _userName;

        public string Email => _email;

        public string IdentityId => _identityId;

        public string Role => _role;

        public List<RefreshToken> RefreshTokens => _refreshTokens;

        protected User() => _refreshTokens = new List<RefreshToken>();

        public User(string userName, string email, string identityId, string role, string firstName = null, string lastName = null) : this()
        {
            _identityId = identityId ?? throw new ArgumentNullException(nameof(identityId));
            
            SetNewUserName(userName);
            SetNewEmail(email);
            SetNewRole(role);
            SetNewFirstName(firstName);
            SetNewLastName(lastName);
        }

        public void SetNewUserName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new FinanceDomainException("Invalid username.");
            }

            _userName = userName;
        }
        
        public void SetNewEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new FinanceDomainException("Invalid email.");
            }

            _email = email;
        }
        
        public void SetNewRole(string role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new FinanceDomainException("Invalid role.");
            }

            _role = role;
        }
        
        public void SetNewFirstName(string firstName) => _firstName = firstName;
        
        public void SetNewLastName(string lastName) => _lastName = lastName;

        public void AddRefreshToken(string token, string remoteIpAddress, double daysToExpire = 1) =>
            _refreshTokens.Add(new RefreshToken(Id, token, DateTime.UtcNow.AddDays(daysToExpire), remoteIpAddress));

        public void RemoveRefreshToken(string token) =>
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == token));

        public bool HasValidRefreshToken(string token) => _refreshTokens.Any(t => t.Token == token && t.Active);
    }
}