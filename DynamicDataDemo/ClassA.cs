using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Noggog;
using Noggog.WPF;
using ReactiveUI;

namespace DynamicDataDemo
{
    public class ClassA : ViewModel
    {
        public ObservableCollection<ClassB> ListOfB { get; } = new();

        public ClassA()
        {
            #region AddSomeContent

            ListOfB.Add(
                new ClassB()
                {
                    ListOfC =
                    {
                        new ClassC()
                        {
                            SomeProperty = "Hello",
                        },
                        new ClassC()
                        {
                            SomeProperty = "World",
                        },
                    }
                });
            ListOfB.Add(
                new ClassB()
                {
                    ListOfC =
                    {
                        new ClassC()
                        {
                            SomeProperty = "Who",
                        },
                        new ClassC()
                        {
                            SomeProperty = "Goes",
                        },
                        new ClassC()
                        {
                            SomeProperty = "There",
                        },
                    }
                });

            #endregion
            
            // Subscribe to changes
            ListOfB.ToObservableChangeSet()
                // Drill into the sublist of C
                .TransformMany(x => x.ListOfC)
                // For each C
                .Transform(c =>
                {
                    // Subscribe to changes on C's SomeProperty
                    var unsubDisposable = c.WhenAnyValue(x => x.SomeProperty)
                        // It immediately fires its current value, skip that
                        .Skip(1)
                        // Call refresh when C's property changes
                        .Subscribe(_ => Refresh());
                    // Return disposable for downstream use
                    return unsubDisposable;
                })
                // Convenience call to dispose of any unsubDisposable that we created from a ClassC that was removed from
                // the original list(s)
                .DisposeMany()
                // We just made IObservable instructions above.  Need to subscribe to put them
                // into effect.  Sort of like actually enumerating an IEnumerable.
                .Subscribe()
                // Stop watching the list if this classA gets disposed.  Not really needed here,
                // but always good practice
                .DisposeWith(this);
        }

        public void Refresh()
        {
            foreach (var b in ListOfB)
            {
                foreach (var c in b.ListOfC)
                {
                    // Check something?
                }
            }
        }
    }
}