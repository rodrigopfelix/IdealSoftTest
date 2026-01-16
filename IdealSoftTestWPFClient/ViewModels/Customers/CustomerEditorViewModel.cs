using IdealSoftTestWPFClient.Commands;
using IdealSoftTestWPFClient.Models;
using IdealSoftTestWPFClient.Services;
using IdealSoftTestWPFClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace IdealSoftTestWPFClient.ViewModels.Customers
{
    public class CustomerEditorViewModel : BaseViewModel
    {
        private readonly ICustomerService _service;
        private readonly IPhoneService _phoneService;

        public ObservableCollection<Phone> Phones { get; } = new ();
        public Customer Customer { get; }

        private Phone? _selectedPhone;
        public Phone? SelectedPhone
        {
            get => _selectedPhone;
            set
            {
                _selectedPhone = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool Saved { get; private set; }

        public ICommand LoadPhoneCommand { get; }
        public ICommand NewPhoneCommand { get; }
        public ICommand EditPhoneCommand { get; }
        public ICommand DeletePhoneCommand { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action? Close { get; set; }

        public CustomerEditorViewModel(
            Customer customer,
            ICustomerService service,
            IPhoneService phoneService)
        {
            Customer = customer;
            _service = service;
            _phoneService = phoneService;

            LoadPhoneCommand = new RelayCommand(async () => await LoadPhoneAsync());
            NewPhoneCommand = new RelayCommand(async () => await NewPhoneAsync());
            EditPhoneCommand = new RelayCommand(async () => await EditPhoneAsync(), () => SelectedPhone != null);
            DeletePhoneCommand = new RelayCommand(async () => await DeletePhoneAsync(), () => SelectedPhone != null);

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => Close?.Invoke());
        }

        private async Task LoadPhoneAsync()
        {
            Phones.Clear();
            if (Customer.Id != null)
            {
                var phone = await _phoneService.GetAllByCustomerIdAsync((Guid)Customer.Id);
                foreach (var c in phone)
                    Phones.Add(c);
            }
        }

        private async Task DeletePhoneAsync()
        {
            if (SelectedPhone?.Id != null && Customer.Id != null)
            {
                await _phoneService.DeleteAsync((Guid)Customer.Id, (Guid)SelectedPhone.Id);
                Phones.Remove(SelectedPhone);
            }
        }

        private async Task NewPhoneAsync()
        {
            await OpenEditorAsync(new Phone());
        }

        private async Task EditPhoneAsync()
        {
            if (SelectedPhone != null)
                await OpenEditorAsync(new Phone
                {
                    Id = SelectedPhone.Id,
                    Number = SelectedPhone.Number,
                    Type = SelectedPhone.Type
                });
        }

        private async Task OpenEditorAsync(Phone phone)
        {
            var editorVm = new CustomerPhoneEditorViewModel(Customer, phone, _phoneService);
            var view = new CustomerPhoneEditorView { DataContext = editorVm };
            editorVm.Close = () => view.Close();
            view.ShowDialog();
            if (editorVm.Saved)
                await LoadPhoneAsync();
        }

        private async Task SaveAsync()
        {
            if (Customer != null)
            {
                if (Customer.Id == null)
                    await _service.CreateAsync(Customer);
                else
                    await _service.UpdateAsync(Customer);
            }

            Saved = true;
            Close?.Invoke();
        }
    }
}
