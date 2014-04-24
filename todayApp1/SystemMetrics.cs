using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace SystemMetric
{
    class SystemMetrics
    {
        [DllImport("coredll.dll")]
        extern static int GetSystemMetrics(SystemMetricsCodes nIndex);
        
        public static int getVScrollWidth()
        {
            return GetSystemMetrics(SystemMetricsCodes.SM_CXVSCROLL);
        }
        public static int getHScrollHeight()
        {
            return GetSystemMetrics(SystemMetricsCodes.SM_CYHSCROLL);
        }

        public static int GetSystemMetric(SystemMetricsCodes nIndex)
        {
            return GetSystemMetrics(nIndex);
        }

        public enum SystemMetricsCodes:int
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
            SM_CXVSCROLL = 2,
            SM_CYHSCROLL = 3,
            SM_CYCAPTION = 4,
            SM_CXBORDER = 5,
            SM_CYBORDER = 6,
            SM_CXDLGFRAME = 7,
            SM_CYDLGFRAME = 8,
            SM_CXICON = 11,
            SM_CYICON = 12,
            SM_CXCURSOR = 13,
            SM_CYCURSOR = 14,
            SM_CYMENU = 15,
            SM_CXFULLSCREEN = 16,
            SM_CYFULLSCREEN = 17,
            SM_MOUSEPRESENT = 19,
            SM_CYVSCROLL = 20,
            SM_CXHSCROLL = 21,
            SM_DEBUG = 22,
            SM_CXDOUBLECLK = 36,
            SM_CYDOUBLECLK = 37,
            SM_CXICONSPACING = 38,
            SM_CYICONSPACING = 39,
            SM_CXEDGE = 45,
            SM_CYEDGE = 46,
            SM_CXSMICON = 49,
            SM_CYSMICON = 50,

            SM_XVIRTUALSCREEN = 76,
            SM_YVIRTUALSCREEN = 77,
            SM_CXVIRTUALSCREEN = 78,
            SM_CYVIRTUALSCREEN = 79,
            SM_CMONITORS = 80,
            SM_SAMEDISPLAYFORMAT = 81,

            SM_CXFIXEDFRAME = SM_CXDLGFRAME,
            SM_CYFIXEDFRAME = SM_CYDLGFRAME,
        }
    }
}
