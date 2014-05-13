using System;

using System.Collections.Generic;
using System.Text;

using Intermec.DeviceManagement.SmartSystem;

namespace ssAPIhelper
{
    public static class ssAPIhelper
    {
        /// <summary>
        /// wrapper for ITCSSApi
        /// </summary>
        /// <param name="sIn">a smartsystems config xml</param>
        /// <returns>true if success</returns>
        public static bool _ssApiSet(string sIn)
        {
            Intermec.DeviceManagement.SmartSystem.ITCSSApi ssAPI = new ITCSSApi();
            int iDataSize = 8192;
            StringBuilder sbReturn = new StringBuilder(iDataSize);
            int returnedDataSize = iDataSize;
            int iTimeout = 3000;
            uint uErr = ssAPI.Set(sIn, sbReturn, ref returnedDataSize, iTimeout);
            if (uErr == Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
            {
                debugSuccess("SSAPI Set OK: " + sbReturn.ToString());
                return true;
            }
            else
            {
                debugError(uErr);
                return false;
            }

        }
        /// <summary>
        /// wrapper for ITCSSApi GET
        /// </summary>
        /// <param name="sIn">provide a vlaid SmartSystems config xml</param>
        /// <returns>true if success</returns>
        public static string _ssApiGet(string sIn)
        {
            Intermec.DeviceManagement.SmartSystem.ITCSSApi ssAPI = new ITCSSApi();
            int iDataSize = 8192;
            StringBuilder sbReturn = new StringBuilder(iDataSize);
            int returnedDataSize = iDataSize;
            int iTimeout = 3000;
            uint uErr = ssAPI.Get(sIn, sbReturn, ref returnedDataSize, iTimeout);
            if (uErr == Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
            {
                debugSuccess("SSAPI get OK: " + sbReturn.ToString());
                return sbReturn.ToString();
            }
            else
            {
                debugError(uErr);
                return "";
            }
        }
        /// <summary>
        /// return a string value of a provided SSAPI xml answer
        /// </summary>
        /// <param name="sb">the answer of a SSAPI GET</param>
        /// <param name="sField">the name of the field</param>
        /// <returns>field value as string</returns>
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

        /// <summary>
        /// return a int value of a provided SSAPI xml answer
        /// </summary>
        /// <param name="sb">the answer of a SSAPI GET</param>
        /// <param name="sField">the name of the field</param>
        /// <returns>field value as int</returns>
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
        /// <summary>
        /// return a bool value of a provided SSAPI xml answer
        /// </summary>
        /// <param name="sb">the answer of a SSAPI GET</param>
        /// <param name="sField">the name of the field</param>
        /// <returns>field value as bool</returns>
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
        /// <summary>
        /// convert encoded strings back to there normal char
        /// ie "&amp;" back to "&"
        /// </summary>
        /// <param name="xmlString">the string to vonvert</param>
        /// <returns></returns>
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
        /// <summary>
        /// convert dangerous chars into xml enconding
        /// ie & to &amp;
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
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
        private static void debugSuccess(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
        private static void debugError(uint uErr)
        {
            string sErr = SSAPIerrors.getErrString((long)uErr);
            System.Diagnostics.Debug.WriteLine(sErr);
        }
    }
}
