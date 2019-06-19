using System;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.UserAggregate
{
    public class RefreshToken : Entity
    {
        private string _token;

        private DateTime _expires;

        private long _userId;

        private string _remoteIpAddress;

        public string Token => _token;

        public DateTime Expires => _expires;

        public long UserId => _userId;

        public string RemoteIpAddress => _remoteIpAddress;
        
        public bool Active => DateTime.UtcNow <= Expires;

        public RefreshToken(long userId, string token, DateTime expires, string remoteIpAddress)
        {
            _userId = userId;
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _expires = expires;
            _remoteIpAddress = remoteIpAddress;
        }
    }
}