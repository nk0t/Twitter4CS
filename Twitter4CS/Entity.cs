using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Twitter4CS
{
	public abstract class Entity
	{
        public int StartIndex { get; protected set; }
        public int EndIndex { get; protected set; }
	}
}
