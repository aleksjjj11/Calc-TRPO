using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calc.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public BaseViewModel()
        {
            _validationDictionary = new Dictionary<string, string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected Dictionary<string, string> _validationDictionary;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string this[string columnName] => _validationDictionary.ContainsKey(columnName) ? _validationDictionary[columnName] : null;

        public string Error =>
            _validationDictionary.Any(x => string.IsNullOrWhiteSpace(x.Value)) 
                ? string.Join(Environment.NewLine, _validationDictionary.Where(x => string.IsNullOrWhiteSpace(x.Value) == false).GetEnumerator().Current) 
                : null;
    }
}
