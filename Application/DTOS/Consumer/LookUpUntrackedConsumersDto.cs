using Application.DTOS.Service;
using Application.Models;

namespace Application.DTOS
{
    public class LookUpUntrackedConsumersDto
    {
        public string PersonalAccount { get; set; }

        public string Owner { get; set; }

        public string OwnerId { get; set; }

        public List<LookUpUserServiceDto> Untracked { get; set; }

        public LookUpUntrackedConsumersDto(Consumer consumer, List<LookUpUserServiceDto> untracked)
        {
            this.PersonalAccount = consumer.PersonalAccount;
            this.Owner = consumer.ConsumerOwner + " - " + consumer.PersonalAccount;
            this.OwnerId = consumer.PersonalAccount;
            this.Untracked = untracked;
        }
    }
}
