using System;

namespace IA.Finance.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class FinanceDomainException : Exception
    {
        public FinanceDomainException()
        {
        }

        public FinanceDomainException(string message) : base(message)
        {
        }

        public FinanceDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}