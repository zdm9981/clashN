using ReactiveUI.Fody.Helpers;

namespace ClashN.Mode
{
    [Serializable]
    public class ProxyModel
    {
        [Reactive]
        public string name { get; set; }

        [Reactive]
        public string type { get; set; }

        [Reactive]
        public string now { get; set; }

        [Reactive]
        public int delay { get; set; }

        [Reactive]
        public string delayName { get; set; }

        [Reactive]
        public bool isActive { get; set; }

        public string countryCode => Tool.CountryFlagHelper.GetCountryCode(name);

        public string displayName => Tool.CountryFlagHelper.GetDisplayName(name);

        public string nowCountryCode => Tool.CountryFlagHelper.GetCountryCode(now);

        public string nowDisplayName => Tool.CountryFlagHelper.GetDisplayName(now);
    }
}
