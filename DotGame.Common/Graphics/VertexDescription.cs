using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public class VertexDescription : IEquatable<VertexDescription>
    {
        private VertexElement[] elements;

        /// <summary>
        /// Ruft die Anzahl der Elemente ab
        /// </summary>
        public int ElementCount
        {
            get { return elements.Length; }
        }

        public VertexDescription(params VertexElement[] elements)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");
            if (elements.Length == 0)
                throw new ArgumentException("Elements must not be empty", "elements");

            this.elements = elements;
            
            // TODO (henrik1235) Auf doppelte Elemente prüfen (gleicher Usage + UsageIndex)
        }

        public VertexElement[] GetElements()
        {
            return (VertexElement[])elements.Clone();
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexDescription)
                return Equals((VertexDescription)obj);
            return false;
        }

        public bool Equals(VertexDescription other)
        {
            return Enumerable.SequenceEqual(elements, other.elements);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + elements.Length.GetHashCode();
                for (int i = 0; i < elements.Length; i++)
                    hash = hash * 23 + elements[i].GetHashCode();
                return hash;
            }
        }
    }
}
