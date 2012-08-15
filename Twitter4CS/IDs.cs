using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;


namespace Twitter4CS
{
    public class IDs
    {
        private IDs()
        {
        }

        public static IDs Create(XElement node)
        {
            if (node == null)
                throw new ArgumentNullException();
            var ids = new IDs();
            IEnumerable<XElement> elements = from el in node.Element("ids").Elements("id") select el;
            foreach (XElement el in elements)
            {
                ids.IDsList.Add(el.ToLong());
            }
            ids.NextCursor = node.Element("next_cursor").ToLong();
            ids.PreviousCursor = node.Element("previous_cursor").ToLong();
            return ids;
        }

        public long[] GetIDs()
        {
            return IDsList.ToArray();
        }

        private List<long> IDsList = new List<long>();
        public long NextCursor { get; private set;}
        public long PreviousCursor { get; private set;}
    }
}
