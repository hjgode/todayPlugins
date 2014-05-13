using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using ssAPIhelper;

namespace FunkSelect
{
    class wlan
    {
        public bool setProfile(string profileLabel)
        {
            bool bRet = false;
            bRet = ssAPIhelper.ssAPIhelper._ssApiSet(String.Format(setProfileXML, profileLabel));
            return bRet;
        }

        public int getProfile(int pNum, out string sName)
        {
            int iRet = 0;
            string sRet = ssAPIhelper.ssAPIhelper._ssApiGet(getProfileXML);
            if (sRet != "")
            {
                string sProfile = ssAPIhelper.ssAPIhelper.getStrSetting(new StringBuilder(sRet), "ActiveProfile");
                if (sProfile != "")
                {
                    sName = sProfile;
                }
                else
                {
                    sName = "error 1";
                    iRet = -1;
                }
            }
            else
            {
                sName = "error 2";
                iRet = -2;
            }

            return iRet;
        }

        const string getProfileXML =//"<?xml version=\"1.0\" encoding=\"UTF-8\" ?> "+
            // "<DevInfo Action=\"Get\" > "+
                                    " <Subsystem Name=\"Funk Security\"> " +
                                    "  <Field Name=\"ActiveProfile\">Profile_1</Field> " +
                                    " </Subsystem> " +
                                    "";
                                   // "</DevInfo> \"";
        const string setProfileXML =//"<?xml version=\"1.0\" encoding=\"UTF-8\" ?> "+
            // "<DevInfo Action=\"Get\" > "+
                                    " <Subsystem Name=\"Funk Security\"> " +
                                    "  <Field Name=\"ActiveProfile\">{0}</Field> " +
                                    " </Subsystem> " +
                                    "";
    }
}
