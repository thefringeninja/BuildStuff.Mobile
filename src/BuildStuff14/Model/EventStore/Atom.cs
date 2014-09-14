using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildStuff14.Model.EventStore
{
    public static class Atom
    {
        #region Nested type: Entry

        public class Entry
        {
            private readonly IEnumerable<Link> _links;

            public Entry(dynamic entry)
            {
                _links = GetLinks(entry.links).ToList();
            }

            public IEnumerable<Link> Links
            {
                get { return _links; }
            }

            private static IEnumerable<Link> GetLinks(IEnumerable<dynamic> links)
            {
                return links.Select(link => new Link(link));
            }
        }

        #endregion

        #region Nested type: Feed

        public class Feed
        {
            private readonly IEnumerable<Entry> _entries;
            private readonly IEnumerable<Link> _links;

            public Feed(dynamic feed)
            {
                _links = GetLinks(feed.links).ToList();
                _entries = GetEntries(feed.entries).ToList();
            }

            public IEnumerable<Link> Links
            {
                get { return _links; }
            }

            public IEnumerable<Entry> Entries
            {
                get { return _entries; }
            }

            private static IEnumerable<Entry> GetEntries(IEnumerable<dynamic> entries)
            {
                return entries.Select(entry => new Entry(entry));
            }

            private static IEnumerable<Link> GetLinks(IEnumerable<dynamic> links)
            {
                return links.Select(link => new Link(link));
            }
        }

        #endregion

        #region Nested type: Link

        public class Link
        {
            private readonly string _relation;
            private readonly Uri _uri;

            public Link(dynamic link)
            {
                _relation = link.relation;
                _uri = new Uri((string) link.uri);
            }

            public Uri Uri
            {
                get { return _uri; }
            }

            public string Relation
            {
                get { return _relation; }
            }
        }

        #endregion
    }
}
