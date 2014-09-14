using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildStuff14.Model.EventStore
{
    public static class Extensions
    {
        public static Atom.Link
            GetRelation(this IEnumerable<Atom.Link> links, string relation)
        {
            return links.FirstOrDefault(link => link.Relation == relation);
        }

        public static string ToPartiallyQualifiedName(this Type type)
        {
            var sb = new StringBuilder();

            sb.Append(type.FullName);
            sb.Append(", ").Append((string) GetAssemblyName(type));
            return sb.ToString();
        }

        private static string GetAssemblyName(Type type)
        {
            var pieces = type.AssemblyQualifiedName.Split(new[] {','}, 3);

            return String.Join(", ", pieces[0], pieces[1]);
        }
    }
}
