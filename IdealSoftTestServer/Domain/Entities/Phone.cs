using PhoneNumbers;

namespace IdealSoftTestServer.Domain.Entities
{
    public class Phone
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Number { get; private set; }
        public string RegionCode { get; private set; }
        public string Type { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public Phone(string number, string regionCode, string type)
        {
            Validate(number, regionCode, type);

            Number = number;
            RegionCode = regionCode;
            Type = type;
        }

        public void Update(string number, string regionCode, string type)
        {
            Validate(number, regionCode, type);

            Number = number;
            RegionCode = regionCode;
            Type = type;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            DeletedAt = DateTime.UtcNow;
        }

        private void Validate(string number, string regionCode, string type)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Phone number cannot be empty.", nameof(Number));

            if (string.IsNullOrWhiteSpace(regionCode))
                throw new ArgumentException("Region code cannot be empty.", nameof(regionCode));

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.", nameof(type));

            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneUtil.Parse(number, regionCode);
                if (!phoneUtil.IsValidNumber(phoneNumber))
                    throw new ArgumentException("Invalid phone", nameof(number));
            }
            catch (NumberParseException ex)
            {
                throw new ArgumentException($"Invalid phone: {ex.Message}");
            }
        }
    }
}
