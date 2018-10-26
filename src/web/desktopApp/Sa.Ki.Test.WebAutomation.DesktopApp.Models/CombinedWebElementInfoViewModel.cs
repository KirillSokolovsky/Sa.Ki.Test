namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CombinedWebElementInfoViewModel : WebElementInfoViewModel
    {
        public CombinedWebElementInfoViewModel(CombinedWebElementInfo combinedWebElement = null)
            : base(combinedWebElement ?? new CombinedWebElementInfo())
        {
            Elements = new ObservableCollection<WebElementInfoViewModel>();
            var items = (_sourceWebElement as CombinedWebElementInfo).Elements;
            if (items != null)
            {
                Elements = new ObservableCollection<WebElementInfoViewModel>();
                foreach (var item in items)
                {
                    var model = WebElementsViewModelsFactory.CreateModelFromInfo(item);
                    model.Parent = this;
                    Elements.Add(model);
                }
            }
        }

        private ObservableCollection<WebElementInfoViewModel> _elements;
        public ObservableCollection<WebElementInfoViewModel> Elements
        {
            get => _elements;
            set => this.RaiseAndSetIfChanged(ref _elements, value);
        }


        public bool IsAscendantFor(WebElementInfoViewModel elementInfo)
        {
            if (elementInfo == null) return false;
            if (Elements == null) return false;
            return Elements.Any(child => (child as CombinedWebElementInfoViewModel)?.IsAscendantFor(elementInfo) ?? false);
        }
    }
}
