using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildStuff14.Model.EventStore
{
    public class JsonEventConverter
    {
        private readonly JsonSerializer _serializer;

        public JsonEventConverter(JsonSerializerSettings serializerSettings)
        {
            _serializer = JsonSerializer.Create(serializerSettings);
        }
        public static readonly JsonEventConverter Default = new JsonEventConverter(new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None
        });
        
        public JObject SerializeHttp(object @event)
        {
            var eventType = @event.GetType();

            var headers = new Dictionary<string, string>
            {
                {"Type", eventType.ToPartiallyQualifiedName()},
                {"Timestamp", DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)}
            };

            return JObject.FromObject(
                new
                {
                    eventId = Guid.NewGuid(),
                    eventType = eventType.Name,
                    metadata = headers,
                    data = @event
                });
        }

        public object Deserialize(dynamic content)
        {
            IDictionary<string, object> headers;
            return Deserialize(content, out headers);
        }

        public object Deserialize(dynamic content, out IDictionary<string, object> headers)
        {
            headers = null;
            
            var data = content.data;

            var metadata = content.metadata;

            if (metadata == null)
                return null;

            string typeName = metadata.Type;

            var type = Type.GetType(typeName, false);

            if (type == null)
                return null;

            headers = metadata.ToObject<Dictionary<string, object>>(_serializer);
            
            return data.ToObject(type, _serializer);
        }
    }
}
