﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public class VertexDescription
    {
        private VertexElement[] elements;

        /// <summary>
        /// Ruft die Anzahl der Elemente ab
        /// </summary>
        public int ElementCount
        {
            get { return elements.Length; }
        }

        /// <summary>
        /// Ruft die die Größe in Bytes ab
        /// </summary>
        public int Size
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
    }
}
