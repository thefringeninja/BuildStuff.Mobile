using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildStuff14.Model.EventStore
{
    internal class EventStoreHttpConnection : IEventStoreHttpConnection, IDisposable
    {
        private readonly HttpClient _httpClient;

        public EventStoreHttpConnection(
            Uri baseAddress, HttpClient httpClient = null,
            UserCredentials defaultCredentials = null)
        {
            _httpClient = httpClient ?? new HttpClient(
                new HttpClientHandler
                {
                    Credentials = defaultCredentials == null
                        ? null
                        : new NetworkCredential(defaultCredentials.Login, defaultCredentials.Password),
                })
            {
                BaseAddress = baseAddress
            };
        }

        #region IDisposable Members

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        #endregion

        #region IEventStoreHttpConnection Members

        public async Task AppendToStream(string streamId, int expectedVersion, IEnumerable<JObject> eventData)
        {
            var batch = new JArray(eventData.OfType<object>().ToArray());

            var response = await _httpClient.SendAsync(
                new HttpRequestMessage(new HttpMethod("POST"), "/streams/" + streamId)
                {
                    Content = new StringContent(batch.ToString(), Encoding.UTF8, ContentTypes.ApplicationJson),
                    Headers =
                    {
                        {"Accept", ContentTypes.ApplicationJson},
                        {"ES-ExpectedVersion", expectedVersion.ToString()}
                    }
                });

            if ((int) response.StatusCode >= 400)
                throw new InvalidOperationException(await response.Content.ReadAsStringAsync());
        }

        public Task AppendToStream(string streamId, int expectedVersion, params JObject[] eventData)
        {
            return AppendToStream(streamId, expectedVersion, eventData.AsEnumerable());
        }

        public async Task<EventSlice> ReadStreamEventsForward(string streamId, int start, int count)
        {
            var streamUri = new Uri(
                _httpClient.BaseAddress,
                string.Format("/streams/{0}/{1}/forwards/20", streamId, start));

            var feed = await ReadAtomFeed(streamUri);

            return await ReadSlice(feed, start, count, "next");
        }

        #endregion

        private async Task<EventSlice> ReadSlice(Atom.Feed feed, int start, int count, string relation)
        {
            var end = start + count;

            var events = new List<dynamic>();

            for (var current = start; current <= end; current += count)
            {
                var eventData = await Task.WhenAll(feed.Entries.Select(ReadEventData));

                foreach (var @event in eventData)
                {
                    if (@event.eventNumber > end)
                    {
                        return new EventSlice(events.ToArray(), feed.Links);
                    }

                    events.Add(@event);
                }

                feed = await ReadAtomFeed(feed.Links.GetRelation(relation).Uri);
            }
            return new EventSlice(events.ToArray(), feed.Links);
        }

        private async Task<dynamic> ReadEventData(Atom.Entry @event)
        {
            var alternate = @event.Links.GetRelation("alternate");
            var response =
                await _httpClient.SendAsync(
                    new HttpRequestMessage(new HttpMethod("GET"), alternate.Uri.PathAndQuery)
                    {
                        Headers =
                        {
                            {"Accept", ContentTypes.EventStoreJson}
                        },
                    });

            return JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
        }

        private async Task<Atom.Feed> ReadAtomFeed(Uri uri)
        {
            var head = await _httpClient.SendAsync(
                new HttpRequestMessage(new HttpMethod("GET"), uri)
                {
                    Headers =
                    {
                        {"Accept", ContentTypes.ApplicationJson}
                    }
                });
            var statusCode = (int) head.StatusCode;
            if (statusCode == 404)
                return null;


            if (statusCode >= 400)
            {
                throw new InvalidOperationException(await head.Content.ReadAsStringAsync());
            }

            return new Atom.Feed(JObject.Parse(await head.Content.ReadAsStringAsync()));
        }
    }
}
