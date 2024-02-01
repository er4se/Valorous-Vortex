using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VV.Models
{
    class GameProduct //Добавить GameProductDbService, разобраться с изображениями
    {
        public uint GameProductId { get; set; }
        public string GameProductName { get; set; }
        public string GameProductVersion { get; set; }
        public string GameProductDiscription { get; set; }
        public uint GameProductRating { get; set; }
        public OpenFileDialog GameFileDialog { get; set; }
    }
}
