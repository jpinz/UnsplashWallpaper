using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unsplash {
    class Config {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public bool Featured { get; set; }
        public string Schedule { get; set; }
        public string Category { get; set; }
        public string SearchTerm { get; set; }
        public string WallpaperStyle{ get; set; }
    }
}
