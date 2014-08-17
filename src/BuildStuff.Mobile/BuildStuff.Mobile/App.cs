using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildStuff.Mobile.Model;
using BuildStuff.Mobile.ViewModels;
using BuildStuff.Mobile.Views;
using Xamarin.Forms;

namespace BuildStuff.Mobile
{
    static class FakeServer
    {
        static FakeServer()
        {
            var speakers = Enumerable.Range(0, 12)
                .Select(_ => new
                {
                    id = Guid.NewGuid().ToString(),
                    Name = "Greg Young",
                    twitter = "@gregyoung",
                    bio = @"Gregory Young coined the term “CQRS” (Command Query Responsibility Segregation) and it was instantly picked up by the community who have elaborated upon it ever since. Greg is an independent consultant and serial entrepreneur. He has 15+ years of varied experience in computer science from embedded operating systems to business systems and he brings a pragmatic and often times unusual viewpoint to discussions. He’s a frequent contributor to InfoQ, speaker/trainer at Skills Matter and also a well-known speaker at international conferences. Greg also writes about CQRS, DDD and other hot topics on codebetter.com.",
                    website = "http://goodenoughsoftware.net"
                })
                .ToDictionary(x => x.id);

            GetSpeakersList = page => Task.FromResult(
                from item in speakers.Values
                select new SpeakerListDto(item.id, item.Name, new byte[0]));

            GetSpeakerDetail = id =>
            {
                var model = speakers[id];
                var dto = new SpeakerDetailDto(
                    id,
                    model.Name,
                    new byte[0],
                    model.bio,
                    model.twitter,
                    new Uri(model.website));
                return Task.FromResult(dto);
            };
        }

        public static readonly GetSpeakerList GetSpeakersList;
        public static GetSpeakerDetail GetSpeakerDetail;
    }
	public class App
	{
		public static Page GetMainPage()
		{
			return new SpeakersListView
			{
			    ViewModel = new SpeakersListViewModel(
			        FakeServer.GetSpeakersList, FakeServer.GetSpeakerDetail)
			        
			};
		}
	}
}
