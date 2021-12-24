using System.Collections.ObjectModel;
using DynamicData;
using Noggog.WPF;

namespace DynamicDataDemo
{
    public class ClassB : ViewModel
    {
        public ObservableCollection<ClassC> ListOfC { get; } = new();
    }
}