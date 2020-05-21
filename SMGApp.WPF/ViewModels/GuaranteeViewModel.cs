using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels
{
    public class GuaranteeViewModel : ViewModelBase
    {
        private readonly CustomersDataService _customerService;
        private readonly GuaranteeDataService _guaranteeDataService;

        public GuaranteeViewModel(CustomersDataService customerService, GuaranteeDataService guaranteeDataService)
        {
            _customerService = customerService;
            _guaranteeDataService = guaranteeDataService;

            Task.Run(async () => await LoadGuaranties());
        }

        private async Task LoadGuaranties()
        {
            GuaranteeEntries = (await _guaranteeDataService.GetAll()).OrderByDescending(r => r.ID).ToList();
        }

        #region SearchBox
        private string _searchBox;
        public string SearchBox
        {
            get => _searchBox;
            set
            {
                _searchBox = value;
                this.OnPropertyChanged(nameof(SearchBox));
                SearchBoxChanged(value);
            }
        }
        private async void SearchBoxChanged(string value)
        {
            if (value == null)
            {
                await LoadGuaranties();
                return;
            }

            if (int.TryParse(value, out int id))
            {
                GuaranteeEntries = (await _guaranteeDataService.GetAll()).OrderByDescending(r => r.ID).Where(i => i.ID == id).ToList();
            }
            else
            {
                GuaranteeEntries = (await _guaranteeDataService.GetAll()).Where(c => c.CustomerDetails.ToUpper().Contains(value.ToUpper())).OrderByDescending(r => r.ID).ToList();
            }
        }
        #endregion

        #region GuaranteeEntries
        private IEnumerable<Guarantee> _guaranteeEntries;
        public IEnumerable<Guarantee> GuaranteeEntries
        {
            get => _guaranteeEntries;
            set
            {
                _guaranteeEntries = value;
                OnPropertyChanged(nameof(GuaranteeEntries));
            }
        }
        #endregion


    }
}