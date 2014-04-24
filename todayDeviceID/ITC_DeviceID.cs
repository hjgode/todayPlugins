using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Intermec.DeviceManagement.SmartSystem;

namespace todayDeviceID
{
    class ITC_DeviceID
    {
        public static string getDeviceID()
        {
            string s = "";
            Intermec.DeviceManagement.SmartSystem.ITCSSApi ssApi = new Intermec.DeviceManagement.SmartSystem.ITCSSApi();
            try
            {
                int iSize = 1024;
                StringBuilder sb = new StringBuilder(iSize);
                string sQuery = "<Subsystem Name=\"SS_Client\"> \r\n<Group Name=\"Identity\"> \r\n<Field Name=\"UniqueId\">otto</Field> \r\n</Group> \r\n</Subsystem>";
                uint uErr = ssApi.Get(sQuery, sb, ref iSize, 2000);
                if (uErr == Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
                {
                    string temp = ssAPIhelper.ssAPIhelper.getStrSetting(sb, "UniqueId");
                    s = temp;
                }
                else
                {
                    s = "ssAPI err=" + uErr.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Exception! " + ex.Message;
            }
            return s;
        }
    }
}
