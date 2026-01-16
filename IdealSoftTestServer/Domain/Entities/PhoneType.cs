//namespace IdealSoftTestServer.Domain.Entities
//{
//    // TODO!: Create an enum for PhoneType with values Mobile, Home, Work

//    public class PhoneType
//    {
//        public Guid Id { get; private set; } = Guid.NewGuid();
//        public string Name { get; private set; }
//        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
//        public DateTime? UpdatedAt { get; private set; }
//        public DateTime? DeletedAt { get; private set; }

//        public PhoneType(string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//                throw new ArgumentException("Phone type name cannot be empty.", nameof(name));
            
//            Name = name;
//        }

//        public void Update(string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//                throw new ArgumentException("Phone type name cannot be empty.", nameof(name));
            
//            Name = name;
//            UpdatedAt = DateTime.UtcNow;
//        }

//        public void Delete()
//        {
//            DeletedAt = DateTime.UtcNow;
//        }
//    }
//}
