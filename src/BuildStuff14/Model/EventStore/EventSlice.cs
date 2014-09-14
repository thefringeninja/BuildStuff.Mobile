using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildStuff14.Model.EventStore
{
    public class EventSlice
    {
        public static readonly EventSlice Empty = new EventSlice();
        public readonly dynamic[] EventStoreEvents;

        private readonly Atom.Link _first;
        private readonly Atom.Link _last;
        private readonly Atom.Link _next;
        private readonly Atom.Link _previous;

        private EventSlice()
        {
            EventStoreEvents = new dynamic[0];
        }

        public EventSlice(dynamic[] eventStoreEvents, IEnumerable<Atom.Link> links)
        {
            EventStoreEvents = eventStoreEvents;
            
            links = links.ToList();
            _first = links.ToList().GetRelation("first");
            _last = links.ToList().GetRelation("last");
            _next = links.ToList().GetRelation("next");
            _previous = links.ToList().GetRelation("previous");
        }

        public bool IsEndOfStream
        {
            get { return _next == null || _next.Equals(_first); }
        }

        public Uri First
        {
            get
            {
                return _first == null
                    ? null
                    : _first.Uri;
            }
        }

        public Uri Last
        {
            get
            {
                return _last == null
                    ? null
                    : _last.Uri;
            }
        }

        public Uri Next
        {
            get
            {
                return _next == null
                    ? null
                    : _next.Uri;
            }
        }

        public Uri Previous
        {
            get
            {
                return _previous == null
                    ? null
                    : _previous.Uri;
            }
        }
    }
}
