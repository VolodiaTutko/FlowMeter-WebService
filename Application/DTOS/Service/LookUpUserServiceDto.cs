using Application.Models;

namespace Application.DTOS.Service
{
    public class LookUpUserServiceDto
    {
        public string Account { get; set; }

        public string Type { get; set; }

        public LookUpUserServiceDto(string account, string type)
        {
            Account = account;
            Type = type;
        }
    }
}
