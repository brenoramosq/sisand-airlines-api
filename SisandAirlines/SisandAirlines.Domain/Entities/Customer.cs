using SisandAirlines.Domain.Utils;

namespace SisandAirlines.Domain.Entities
{
    public class Customer : Entity
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Document { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecondaryPasswordHash { get; private set; }   
        public DateTime CreateDate { get; private set; }
        public bool Active { get; private set; }

        public Customer(string fullName, string email, string document, DateTime birthDate, string passwordHash, string secondaryPasswordHash)
            :base()
        {
            FullName = fullName;
            Email = email;
            Document = document;
            BirthDate = birthDate;
            PasswordHash = EncryptPassword.ToSHA256(passwordHash);
            SecondaryPasswordHash = EncryptPassword.ToSHA256(secondaryPasswordHash);
            CreateDate = DateTime.Now;
            Active = true;
        }

        public Customer
        (
            Guid id, 
            string fullName, 
            string email, 
            string document, 
            DateTime birthDate, 
            string passwordHash, 
            string secondaryPasswordHash, 
            DateTime createDate, 
            bool active
        )
            :base(id)
        {
            FullName = fullName;
            Email = email;
            Document = document;
            BirthDate = birthDate;
            SecondaryPasswordHash = secondaryPasswordHash;
            PasswordHash = passwordHash;
            CreateDate = createDate;
            Active = active;
        }
    }
}
