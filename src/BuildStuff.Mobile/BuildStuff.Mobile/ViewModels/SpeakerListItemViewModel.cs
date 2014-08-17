using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using BuildStuff.Mobile.Model;
using ReactiveUI;

namespace BuildStuff.Mobile.ViewModels
{
    public class SpeakerListItemViewModel : ReactiveObject
    {
        private readonly string id;

        public SpeakerListItemViewModel(SpeakerListDto item, GetSpeakerDetail getSpeakerDetail)
        {
            Image = item.Image;
            Name = item.Name;
            id = item.Id;

            ViewDetails = ReactiveCommand.CreateAsyncObservable(_ => getSpeakerDetail(id).ToObservable());

            ViewDetails.SubscribeOn(RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<SpeakerDetailDto> ViewDetails { get; private set; }


        public byte[] Image { get; private set; }
        public string Name { get; private set; }
    }
}