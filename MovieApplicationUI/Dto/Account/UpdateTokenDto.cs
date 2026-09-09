namespace MovieApplicationUI.Dto.Account
{
    public class UpdateTokenDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
