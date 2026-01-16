using IdealSoftTestWPFClient.Commands;
using IdealSoftTestWPFClient.Models;
using IdealSoftTestWPFClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace IdealSoftTestWPFClient.ViewModels.Customers
{
    public class CustomerPhoneEditorViewModel : BaseViewModel
    {
        private readonly IPhoneService _service;

        public Customer Customer { get; }
        public Phone Phone { get; }

        public bool Saved { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action? Close { get; set; }

        public CustomerPhoneEditorViewModel(
            Customer customer,
            Phone phone,
            IPhoneService service)
        {
            Customer = customer;
            Phone = phone;
            _service = service;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => Close?.Invoke());
        }

        private async Task SaveAsync()
        {
            if (Phone != null && Customer.Id != null)
            {
                if (Phone.Id == null)
                    await _service.CreateForCustomerAsync((Guid)Customer.Id, Phone);
                else
                    await _service.UpdateAsync((Guid)Customer.Id, Phone);
            }

            Saved = true;
            Close?.Invoke();
        }
    }
}
