using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using BuildStuff.Mobile.Model;
using ReactiveUI;

namespace BuildStuff.Mobile.ViewModels
{
    public class SpeakersListViewModel : ReactiveObject, IEnumerable<SpeakerListItemViewModel>
    {
        public SpeakersListViewModel(GetSpeakerList getSpeakerList, GetSpeakerDetail getSpeakerDetail)
        {
            Items = new ReactiveList<SpeakerListItemViewModel>();
            getSpeakerList().ToObservable().ObserveOn(RxApp.MainThreadScheduler).Subscribe(
                items =>
                {
                    using (Items.SuppressChangeNotifications())
                    {
                        Items.Clear();
                        Items.AddRange(
                            from item in items
                            select new SpeakerListItemViewModel(item, getSpeakerDetail));
                    }
                });
        }

        public IReactiveList<SpeakerListItemViewModel> Items { get; private set; }

        #region IEnumerable<SpeakerListItemViewModel> Members

        public IEnumerator<SpeakerListItemViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
