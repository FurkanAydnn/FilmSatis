using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Models
{
    public class Slider
    {
        public int SliderId { get; set; }
        public string ThumbnailURL { get; set; }
        public string LargeImageURL { get; set; }
    }
}