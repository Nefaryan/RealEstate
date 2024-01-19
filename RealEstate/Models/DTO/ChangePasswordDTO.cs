namespace RealEstate.Models.DTO
{
    public class ChangePasswordDTO
    {
        public string Password { get; set; }    

        public ChangePasswordDTO()
        {

        }
        public ChangePasswordDTO(User U)
        {
            Password = U.Salt;

        }
    }
}
