using Noggog.WPF;
using ReactiveUI.Fody.Helpers;

namespace DynamicDataDemo
{
    public class ClassC : ViewModel
    {
        [Reactive]
        public string SomeProperty { get; set; }
    }
}