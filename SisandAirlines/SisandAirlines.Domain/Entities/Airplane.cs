namespace SisandAirlines.Domain.Entities
{
    public class Airplane : Entity
    {
        public string Model { get; private set; }
        public string Code { get; private set; }

        public Airplane(string model)
            :base()
        {
            Model = model;
            Code = GenerateCode();
        }

        public Airplane(Guid id, string model, string code)
            : base(id)
        {
            Model = model;
            Code = code;
        }

        private string GenerateCode()
        {
            return $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}";
        }
    }
}
