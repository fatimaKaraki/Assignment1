namespace SharedObjects.TockenHandling
{
    public interface ITokenHandler
    {
        public string GenerateToken(string username);
        public bool ValidateToken(string token);
    }
}