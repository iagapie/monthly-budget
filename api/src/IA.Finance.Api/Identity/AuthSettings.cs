namespace IA.Finance.Api.Identity
{
    public class AuthSettings
    {
        public string SecretKey { get; set; }
        
        public string LoginPath { get; set; }
        
        public string RefreshTokenPath { get; set; }
    }
}