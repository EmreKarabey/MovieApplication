using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MovieApplicationUI.Dto.Account
{
    public class UpdateAccountDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
