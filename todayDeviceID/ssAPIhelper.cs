using System;

using System.Collections.Generic;
using System.Text;

using Intermec.DeviceManagement.SmartSystem;

namespace ssAPIhelper
{
    public static class ssAPIhelper
    {
        public static string getStrSetting(StringBuilder sb, String sField)
        {
            string sVal = sb.ToString();
            string sFieldString = "<Field Name=\"" + sField + "\">";
            int iIdx = sVal.IndexOf(sFieldString);
            string sResult = "";
            if (iIdx >= 0)
            {
                string fieldStr = sVal.Substring(iIdx);
                string ssidStr = fieldStr.Remove(0, sFieldString.Length);
                iIdx = ssidStr.IndexOf("<");
                sResult = ssidStr.Substring(0, iIdx);
            }
            return sResult;
        }

        public static int getIntSetting(StringBuilder sb, String sField)
        {
            string sVal = sb.ToString();
            string sFieldString = "<Field Name=\"" + sField + "\">";
            int iIdx = sVal.IndexOf(sFieldString);
            int iRes = -1;
            if (iIdx >= 0)
            {
                string fieldStr = sVal.Substring(iIdx);
                string ssidStr = fieldStr.Remove(0, sFieldString.Length);
                iIdx = ssidStr.IndexOf("<");
                string sResult = ssidStr.Substring(0, iIdx);
                try
                {
                    iRes = Convert.ToInt16(sResult);
                }
                catch (Exception)
                {
                    iRes = -9999; //error code
                }
            }
            return iRes;
        }
        public static bool getBoolSetting(StringBuilder sb, String sField)
        {
            string sVal = sb.ToString();
            string sFieldString = "<Field Name=\"" + sField + "\">";
            int iIdx = sVal.IndexOf(sFieldString);
            if (iIdx >= 0)
            {
                string fieldStr = sVal.Substring(iIdx);
                string ssidStr = fieldStr.Remove(0, sFieldString.Length);
                iIdx = ssidStr.IndexOf("<");
                string sResult = ssidStr.Substring(0, iIdx);
                if (sResult.Equals("1"))
                    return true;
            }
            return false;
        }
        public static string unEscapeXML(string xmlString)
        {
            //Dim xmlString As String = "&lt;&gt;&amp;"
            xmlString = xmlString.Replace("&lt;", "<");
            xmlString = xmlString.Replace("&gt;", ">");
            xmlString = xmlString.Replace("&amp;", "&");
            xmlString = xmlString.Replace("&quot;", "\"");
            xmlString = xmlString.Replace("&apos;", "'");
            //xmlString now is "<>&"

            return xmlString;
        }

        public static string escapeXML(string xmlString)
        {
            //Dim xmlString As String = "<>&"
            xmlString = xmlString.Replace("<", "&lt;");
            xmlString = xmlString.Replace(">", "&gt;");
            xmlString = xmlString.Replace("&", "&amp;");
            xmlString = xmlString.Replace("\"", "&quot;");
            xmlString = xmlString.Replace("'", "&apos;");
            //xmlString now is "&lt;&gt;&amp;"
            return xmlString;
        }

        public static bool applyXMLfile(string sFileXML)
        {
            bool bRet = true;
            Intermec.DeviceManagement.SmartSystem.ITCSSApi ssAPI = new ITCSSApi();
            string sOutFile = sFileXML + ".out";
            StringBuilder sbReturnData = new StringBuilder(32000);
            int iDataSize = 32000;
            int iTimeout = 15000;   //15000 ms = 15 Seconds
            uint uRet = ssAPI.ConfigFromFile(sFileXML, sOutFile, sbReturnData, ref iDataSize, iTimeout);
            if (uRet == ITCSSErrors.E_SS_SUCCESS)
            {
                System.Diagnostics.Debug.WriteLine("SSAPI ConfigFromFile OK");
                bRet = true;
            }
            else
            {   // 3245211681
                SS_API_Errors sErr = (SS_API_Errors)uRet;
                System.Diagnostics.Debug.WriteLine("SSAPI ConfigFromFile failed: " + uRet.ToString() + " = " + sErr.ToString());
                bRet = false;
            }
            return bRet;
        }
        //   ITCConfig/SS_API Errors (C16E0000-C16EFFFF)
        enum SS_API_Errors:uint{
            E_ITCCONFIG_OPERATION_FAILED		= 0xC16E0001,
            E_ITCCONFIG_OPEN_CONN_FAILED		= 0xC16E0002,
            E_ITCCONFIG_SEND_REQ_FAILED		= 0xC16E0003,
            E_ITCCONFIG_RCV_RESP_FAILED		= 0xC16E0004,
            E_ITCCONFIG_RCV_TIMEOUT		= 0xC16E0005,
            E_ITCCONFIG_RCV_BUFFER_TOO_SMALL 	= 0xC16E0006,
            E_ITCCONFIG_CONN_NOT_OPENED		= 0xC16E0007,
            E_ITCCONFIG_READ_FILE_FAILED		= 0xC16E0008,
            E_ITCCONFIG_CREATE_DOM_FAILED	= 0xC16E0009,
            E_ITCCONFIG_OPEN_FILE_FAILED		= 0xC16E000A,

            E_SSAPI_OPERATION_FAILED					= 0xC16E0021,
            E_SSAPI_OPEN_CONN_FAILED					= 0xC16E0022,
            E_SSAPI_SEND_REQ_FAILED					= 0xC16E0023,
            E_SSAPI_RCV_RESP_FAILED					= 0xC16E0024,
            E_SSAPI_RCV_TIMEOUT						= 0xC16E0025,
            E_SSAPI_RCV_BUFFER_TOO_SMALL 			= 0xC16E0026,
            E_SSAPI_CONN_NOT_OPENED					= 0xC16E0027,
            E_SSAPI_READ_FILE_FAILED					= 0xC16E0028,
            E_SSAPI_CREATE_DOM_FAILED				= 0xC16E0029,
            E_SSAPI_OPEN_FILE_FAILED					= 0xC16E002A,
            E_SSAPI_XML_MATCH_NOT_FOUND				= 0xC16E002B,
            E_SSAPI_NO_MORE_CONNECTION_ALLOWED 	= 0xC16E002C,
            E_SSAPI_SYS_RSC_ALLOC_FAILED 			= 0xC16E002D,
            E_SSAPI_MISSING_REQUIRED_PARM			= 0xC16E002E,
            E_SSAPI_INVALID_EVENT						= 0xC16E002F,
            E_SSAPI_TIMEOUT								= 0xC16E0030,
            E_SSAPI_MALFORMED_XML						= 0xC16E0031,
            E_SSAPI_INVALID_PARM						= 0xC16E0032,
            E_SSAPI_FUNCTION_UNAVAILABLE			= 0xC16E0033,
            E_SSAPI_MSG_ID_IN_USE                = 0xC16E0034,
            E_SSAPI_NOT_GROUP_MEMBER             = 0xC16E0035,
    }

    }
}
