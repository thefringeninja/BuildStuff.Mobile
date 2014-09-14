using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BuildStuff14.Model.EventStore
{
    public interface IEventStoreHttpConnection
    {
        Task AppendToStream(string streamId, int expectedVersion, IEnumerable<JObject> eventData);
        Task AppendToStream(string streamId, int expectedVersion, params JObject[] eventData);
        Task<EventSlice> ReadStreamEventsForward(string streamId, int start, int count);
    }
}
