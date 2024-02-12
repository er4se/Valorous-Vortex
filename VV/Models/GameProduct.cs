using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VV.Models
{
    public class GameProduct //Добавить GameProductDbService, разобраться с изображениями
    {
        public string GameProductId { get; set; }
        public string GameProductName { get; set; }
        public string GameProductVersion { get; set; }
        public string GameProductDiscription { get; set; }
        public uint GameProductRating { get; set; }
        public string GameProductDeveloper { get; set; }
        public byte[] GameProductSplashScreen { get; set; }
        public OpenFileDialog GameFileDialog { get; set; }
    }
}
