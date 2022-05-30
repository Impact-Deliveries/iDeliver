using IDeliverObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.Common
{
    public class MediaType
    {

        public int GetMediaType(string extintion)
        {
            int type = 0;
            switch (extintion.ToUpper())
            {
                case ".GIF":
                case ".SVG":
                case ".PNG":
                case ".JPG":
                case ".JPEG":
                    type = 1;
                    break;
                case ".WMV":
                case ".3G2":
                case ".3GP":
                case ".AVI":
                case ".FLV":
                case ".M4V":
                case ".MPG":
                case ".MP4":
                    type = 2;
                    break;
                case ".WAV":
                case ".WMA":
                case ".AIF":
                case ".M3U":
                case ".M4A":
                case ".MP3":
                case ".MPA":
                case ".MID":
                    type = 3;
                    break;
                default:
                    type = 4;
                    break;
            }
            return type;
        }


    }

}
