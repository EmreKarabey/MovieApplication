namespace MovieApplicationUI.Dto.Login
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public bool Requires2FA { get; set; }
    }
}
