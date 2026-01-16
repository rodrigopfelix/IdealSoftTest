using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IdealSoftTestWPFClient.Models
{
    public class Customer : INotifyPropertyChanged
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private List<Phone> _phones = new();

        public Guid? Id { get; set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public List<Phone> Phones
        {
            get => _phones;
            set
            {
                _phones = value;
                OnPropertyChanged();
            }
        }

        public string OneLinePhones
        {
            get
            {
                if (Phones.Count == 0)
                    return "No phones";
                return string.Join(", ", Phones.Select(p => $"({p.Type}) {p.RegionCode} - {p.Number}"));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
