using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeRuler
{
    public class Scene
    {
        public SortedDictionary<int, Slice> Slices { get; set; }

        public Scene()
        {
            Slices = new SortedDictionary<int, Slice>();
        }
    }
}
