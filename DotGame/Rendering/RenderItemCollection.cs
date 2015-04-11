using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering
{
    public class RenderItemCollection
    {
        private List<RenderItem> items = new List<RenderItem>();

        public RenderItemCollection()
        {

        }

        public void AddItem(RenderItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            items.Add(item);
        }

        public RenderItem[] GetItems()
        {
            return items.ToArray();
        }
    }
}
