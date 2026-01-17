using IdealSoftTestWPFClient.Commands;
using IdealSoftTestWPFClient.Models;
using IdealSoftTestWPFClient.Services;
using IdealSoftTestWPFClient.Views;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace IdealSoftTestWPFClient.ViewModels.Customers
{
    public class CustomersViewModel : BaseViewModel
    {
        private readonly ICustomerService _service;
        private readonly IPhoneService _phoneService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;

        public ObservableCollection<Customer> Customers { get; }
            = new ObservableCollection<Customer>();

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand InitCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public CustomersViewModel(
            ICustomerService service,
            IPhoneService phoneService,
            IAuthService authService,
            IConfiguration config)
        {
            _service = service;
            _phoneService = phoneService;
            _authService = authService;
            _config = config;

            InitCommand = new RelayCommand(async () => await InitAsync());
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            NewCommand = new RelayCommand(async () => await NewAsync());
            EditCommand = new RelayCommand(async () => await EditAsync(), () => SelectedCustomer != null);
            DeleteCommand = new RelayCommand(async () => await DeleteAsync(), () => SelectedCustomer != null);
            _phoneService = phoneService;
        }

        private async Task InitAsync()
        {
            var login = _config["Api:Login"]?.ToString() ?? throw new Exception("Api:Login not configured");
            var password = _config["Api:Password"]?.ToString() ?? throw new Exception("Api:Password not configured");
            await _authService.LoginAsync(login, password);
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            Customers.Clear();
            var customers = await _service.GetAllAsync();
            foreach (var c in customers)
                Customers.Add(c);
        }

        private async Task DeleteAsync()
        {
            if (SelectedCustomer?.Id != null)
            {
                await _service.DeleteAsync((Guid)SelectedCustomer.Id);
                Customers.Remove(SelectedCustomer);
            }
        }

        private async Task NewAsync()
        {
            await OpenEditorAsync(new Customer());
        }

        private async Task EditAsync()
        {
            if (SelectedCustomer != null)
                await OpenEditorAsync(new Customer
                {
                    Id = SelectedCustomer.Id,
                    FirstName = SelectedCustomer.FirstName,
                    LastName = SelectedCustomer.LastName
                });
        }

        private async Task OpenEditorAsync(Customer customer)
        {
            var editorVm = new CustomerEditorViewModel(customer, _service, _phoneService);
            var view = new CustomerEditorView { DataContext = editorVm };
            editorVm.Close = () => view.Close();
            view.ShowDialog();
            if (editorVm.Saved)
                await LoadAsync();
        }
    }
}