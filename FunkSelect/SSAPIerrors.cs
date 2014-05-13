using System;
using System.Collections.Generic;
using System.Text;

namespace ssAPIhelper
{
    static class SSerrorStrings
    {
        public static string getErrorString(string err)
        {
            //  Smart system errors are 4 byte long.  On windows platform they can be mapped to
            //  an HRESULT whose values are layed out as follows:
            //
            //   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
            //   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
            //  +---+-+-+-----------------------+-------------------------------+
            //  |Sev|C|R|     Facility          |               Code            |
            //  +---+-+-+-----------------------+-------------------------------+
            //
            //  where
            //
            //      Sev (2 bits) - is the severity code.
            //
            //          00 - Success
            //          01 - Informational
            //          10 - Warning
            //          11 - Error
            //
            //      C - is the Customer code flag 
            //
            //      R - is a reserved bit
            //
            //      Facility (12 bits) - is the facility code.  Reserved codes are stored in 
            //			\IVA\Documentation\IVA Notes\Table Of Facility Numbers.htm. 	
            //
            //      Code (16 bits) - is the facility's status code

            // ie err=822B0003 is
            // 1000001000101011
            //                 0000000000000011
            // 10 = Warning
            //   0 = Customer Code Flag
            //    0 = Reserved
            //     001000101011 = Facility code
            //                 0000000000000011 = code
            System.Diagnostics.Debug.WriteLine("Loooking for error '" + err + "'\n");

            // 0x3245211688
            // 0011 0010 0100 0101 - 0010 0001 0001 0110
            //filter whole xml answer for FIRST error

            /*
                Error in ss.Get: 0x3245211688

                <Subsystem Name=Device Monitor>
                <Group Name=ITCHealth>
                <Field Name=System\Info\LastBoot Error=822B0006>{1}</Field>
                </Group>
                </Subsystem>
                E_SSClient_UNRECOGNIZED_XML_TAG            
             */
            if (err.IndexOf("Error=") > -1)
                err = err.Substring(err.IndexOf("Error=") + "Error=".Length, 10);

            if (err.StartsWith("\""))
                err = err.Substring(1);
            if (err.EndsWith("\""))
                err = err.Substring(0, err.Length - 1);


            int i = 1;
            int l = err.Length;
            bool foundit = false;
            string s;
            //search the known error strings and shorten the search-for string if not found
            do
            {
                i = 1;
                do
                {
                    s = ErrorStrings[i].Substring(err.Length - l);
                    if (err.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                        return ErrorStrings[i - 1];
                    i += 2;
                } while (i < ErrorStrings.Length);
                l--;
            } while (!foundit && l > 4); //at least the last 5 chars should match
            return "unknown Error code";
        }
        public static string getSSAPIErrorString(uint uErr){
            string sRet = "unknown";
            string sHex = "0x" + uErr.ToString("X8");
            for (int i = 0; i < ErrorStrings.Length-1; i+=2)
            {
                if (ErrorStrings[i + 1] == sHex)
                    sRet = ErrorStrings[i];
            }
            return sRet;
        }
        static string[] ErrorStrings = { 
			"E_CONFIGLET2_ERROR", "0x822B0000",
			"E_CONFIGLET2_DATA_TOO_BIG", "0x822B0001",
			"E_CONFIGLET2_VALUE_INVALID", "0x822B0002",
			"E_CONFIGLET2_NAME_INVALID", "0x822B0003",
			"E_CONFIGLET2_INSTANCE_INVALID", "0x822B0004",
			"E_CONFIGLET2_DISABLED_BY_POLICY", "0x822B0005",
			"E_CONFIGLET2_NO_LICENSE", "0x822B0006",
		
            "E_SSClient_OPEN_INPUT_FAILED", "C16B0001", 
            "E_SSClient_READ_INPUT_FAILED", "C16B0002", 
            "E_SSClient_PARSE_INPUT_FAILED", "C16B0003", 
            "E_SSClient_OPEN_OUTPUT_FAILED",  "C16B0004", 
            "E_SSClient_WRITE_OUTPUT_FAILED",  "C16B0005", 
            "E_SSClient_UNRECOGNIZED_XML_TAG",  "C16B0006", 
            "E_SSClient_MISSING_ATTRIBUTE",  "C16B0007", 
            "E_SSClient_TAG_OUT_OF_CONTEXT",  "C16B0008", 
            "E_SSClient_BAD_NAME",  "C16B0009", 
            "E_SSClient_BAD_VALUE",  "C16B000A", 
            "E_SSClient_BAD_INSTANCE",  "C16B000B", 
            "E_SSClient_VERSION_MISMATCH",  "C16B000C", 
            "E_SSClient_SERVICE_IS_UNAVAILABLE",  "C16B000D", 
            "E_SSClient_ERROR_IN_SUBELEMENT",  "C16B000E", 
            "E_ITCCONFIG_OPERATION_FAILED",  "C16E0001", 
            "E_ITCCONFIG_OPEN_CONN_FAILED",  "C16E0002", 
            "E_ITCCONFIG_SEND_REQ_FAILED",  "C16E0003", 
            "E_ITCCONFIG_RCV_RESP_FAILED",  "C16E0004", 
            "E_ITCCONFIG_RCV_TIMEOUT",  "C16E0005", 
            "E_ITCCONFIG_RCV_BUFFER_TOO_SMALL",  "C16E0006", 
            "E_ITCCONFIG_CONN_NOT_OPENED",  "C16E0007", 
            "E_ITCCONFIG_READ_FILE_FAILED",  "C16E0008", 
            "E_ITCCONFIG_CREATE_DOM_FAILED",  "C16E0009", 
            "E_ITCCONFIG_OPEN_FILE_FAILED",  "C16E000A", 

            "E_SSAPI_OPERATION_FAILED",  "C16E0021", 
            "E_SSAPI_OPEN_CONN_FAILED",  "C16E0022", 
            "E_SSAPI_SEND_REQ_FAILED",  "C16E0023", 
            "E_SSAPI_RCV_RESP_FAILED",  "C16E0024", 
            "E_SSAPI_RCV_TIMEOUT",  "C16E0025", 
            "E_SSAPI_RCV_BUFFER_TOO_SMALL",  "C16E0026", 
            "E_SSAPI_CONN_NOT_OPENED",  "C16E0027", 
            "E_SSAPI_READ_FILE_FAILED",  "C16E0028", 
            "E_SSAPI_CREATE_DOM_FAILED",  "C16E0029", 
            "E_SSAPI_OPEN_FILE_FAILED",  "C16E002A", 
            "E_SSAPI_XML_MATCH_NOT_FOUND",  "C16E002B", 
            "E_SSAPI_NO_MORE_CONNECTION_ALLOWED",  "C16E002C", 
            "E_SSAPI_SYS_RSC_ALLOC_FAILED",  "C16E002D", 
            "E_SSAPI_MISSING_REQUIRED_PARM",  "C16E002E", 
            "E_SSAPI_INVALID_EVENT", "C16E002F", 
            "E_SSAPI_TIMEOUT",  "C16E0030", 
            "E_SSAPI_MALFORMED_XML", "C16E0031", 
            "E_SSAPI_INVALID_PARM", "C16E0032", 
            "E_SSAPI_FUNCTION_UNAVAILABLE",  "C16E0033", 
            "E_SSAPI_MSG_ID_IN_USE ",  "C16E0034", 
            "E_SSAPI_NOT_GROUP_MEMBER ",  "C16E0035", 
            "E_SSCONFIGLET_RULE_EXISTS",  "C4110001", 
            "E_SSCONFIGLET_UNSUPP_RULE_TYPE",  "C4110002", 
            "E_SSCONFIGLET_RULE_NOT_FOUND",  "C4110003", 
            "E_SSCONFIGLET_EMPTY_LIST",  "C4110004", 
            "E_SSCONFIGLET_NO_STATE",  "C4110005", 

            "E_FILEXFERCONFIGLET_UPGRADE_IN_PROGRESS",  "C4111001", 
            "E_FILEXFERCONFIGLET_NOT_ENOUGH_SPACE",  "C4111002", 
            "E_FILEXFERCONFIGLET_NO_POWER",  "C4111003", 
            "E_FILEXFERCONFIGLET_NO_MCCLIENT_VERSION",  "C4111004", 
            "E_FILEXFERCONFIGLET_TEST_FAILED", "C4111005" 
        };
    }
    public static class SSAPIerrors
    {

        // 10000010 00101011 00000000 00000011
        // 82       2B       00       03
        //
        //  Smart system errors are 4 byte long.  On windows platform they can be mapped to
        //  an HRESULT whose values are layed out as follows:
        //
        //   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
        //   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
        //  +---+-+-+-----------------------+-------------------------------+
        //  |Sev|C|R|     Facility          |               Code            |
        //  +---+-+-+-----------------------+-------------------------------+
        //
        //  where
        //
        //      Sev (2 bits) - is the severity code.
        //
        //          00 - Success
        //          01 - Informational
        //          10 - Warning
        //          11 - Error
        //
        //      C - is the Customer code flag 
        //
        //      R - is a reserved bit
        //
        //      Facility (12 bits) - is the facility code.  Reserved codes are stored in 
        //			\IVA\Documentation\IVA Notes\Table Of Facility Numbers.htm. 	
        //
        //      Code (16 bits) - is the facility's status code

        public static bool isWarning(uint uErr)
        {            
            if ((uErr & 0x80000000) == 0x80000000)
                return true;
            else
                return false;
        }
        public enum ss_errors:uint{

            E_SSClient_OPEN_INPUT_FAILED = 0xC16B0001,
            E_SSClient_READ_INPUT_FAILED,
            E_SSClient_PARSE_INPUT_FAILED,
            E_SSClient_OPEN_OUTPUT_FAILED,
            E_SSClient_WRITE_OUTPUT_FAILED,
            E_SSClient_UNRECOGNIZED_XML_TAG,
            E_SSClient_MISSING_ATTRIBUTE,
            E_SSClient_TAG_OUT_OF_CONTEXT,
            E_SSClient_BAD_NAME,
            E_SSClient_BAD_VALUE,// = 0xC16B000A,
            E_SSClient_BAD_INSTANCE,//= 0xC16B000B,
            E_SSClient_VERSION_MISMATCH,// = 0xC16B000C,
            E_SSClient_SERVICE_IS_UNAVAILABLE,//= 0xC16B000D,
            E_SSClient_ERROR_IN_SUBELEMENT,// = 0xC16B000E,

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
        public static string getErrString(long lError){
            ss_errors ssErr = (ss_errors)lError;
            string sRet;
            if (ssErr == ss_errors.E_SSClient_BAD_INSTANCE)
                sRet = "E_SSClient_BAD_INSTANCE";
            else if (ssErr == ss_errors.E_SSClient_BAD_NAME)
                sRet = "E_SSClient_BAD_NAME";
            else if (ssErr == ss_errors.E_SSClient_BAD_VALUE)
                sRet = "E_SSClient_BAD_VALUE";
            else if (ssErr == ss_errors.E_SSClient_ERROR_IN_SUBELEMENT)
                sRet = "E_SSClient_ERROR_IN_SUBELEMENT";
            else if (ssErr == ss_errors.E_SSClient_MISSING_ATTRIBUTE)
                sRet = "E_SSClient_MISSING_ATTRIBUTE";
            else if (ssErr == ss_errors.E_SSClient_OPEN_INPUT_FAILED)
                sRet = "E_SSClient_OPEN_INPUT_FAILED";
            else if (ssErr == ss_errors.E_SSClient_OPEN_OUTPUT_FAILED)
                sRet = "E_SSClient_OPEN_OUTPUT_FAILED";
            else if (ssErr == ss_errors.E_SSClient_PARSE_INPUT_FAILED)
                sRet = "E_SSClient_PARSE_INPUT_FAILED";
            else if (ssErr == ss_errors.E_SSClient_READ_INPUT_FAILED)
                sRet = "E_SSClient_READ_INPUT_FAILED";
            else if (ssErr == ss_errors.E_SSClient_SERVICE_IS_UNAVAILABLE)
                sRet = "E_SSClient_SERVICE_IS_UNAVAILABLE";
            else if (ssErr == ss_errors.E_SSClient_TAG_OUT_OF_CONTEXT)
                sRet = "E_SSClient_TAG_OUT_OF_CONTEXT";
            else if (ssErr == ss_errors.E_SSClient_UNRECOGNIZED_XML_TAG)
                sRet = "E_SSClient_UNRECOGNIZED_XML_TAG";
            else if (ssErr == ss_errors.E_SSClient_VERSION_MISMATCH)
                sRet = "E_SSClient_VERSION_MISMATCH";
            else if (ssErr == ss_errors.E_SSClient_WRITE_OUTPUT_FAILED)
                sRet = "E_SSClient_WRITE_OUTPUT_FAILED";

            else if (ssErr == ss_errors.E_SSAPI_CONN_NOT_OPENED)
                sRet = "E_SSAPI_CONN_NOT_OPENED";
            else if (ssErr == ss_errors.E_SSAPI_CREATE_DOM_FAILED)
                sRet = "E_SSAPI_CREATE_DOM_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_FUNCTION_UNAVAILABLE)
                sRet = "E_SSAPI_FUNCTION_UNAVAILABLE";
            else if (ssErr == ss_errors.E_SSAPI_INVALID_EVENT)
                sRet = "E_SSAPI_INVALID_EVENT";
            else if (ssErr == ss_errors.E_SSAPI_INVALID_PARM)
                sRet = "E_SSAPI_INVALID_PARM";
            else if (ssErr == ss_errors.E_SSAPI_MALFORMED_XML)
                sRet = "E_SSAPI_MALFORMED_XML";
            else if (ssErr == ss_errors.E_SSAPI_MISSING_REQUIRED_PARM)
                sRet = "E_SSAPI_MISSING_REQUIRED_PARM";
            else if (ssErr == ss_errors.E_SSAPI_MSG_ID_IN_USE)
                sRet = "E_SSAPI_MSG_ID_IN_USE";
            else if (ssErr == ss_errors.E_SSAPI_NO_MORE_CONNECTION_ALLOWED)
                sRet = "E_SSAPI_NO_MORE_CONNECTION_ALLOWED";
            else if (ssErr == ss_errors.E_SSAPI_NOT_GROUP_MEMBER)
                sRet = "E_SSAPI_NOT_GROUP_MEMBER";
            else if (ssErr == ss_errors.E_SSAPI_OPEN_CONN_FAILED)
                sRet = "E_SSAPI_OPEN_CONN_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_OPEN_FILE_FAILED)
                sRet = "E_SSAPI_OPEN_FILE_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_OPERATION_FAILED)
                sRet = "E_SSAPI_OPERATION_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_RCV_BUFFER_TOO_SMALL)
                sRet = "E_SSAPI_RCV_BUFFER_TOO_SMALL";
            else if (ssErr == ss_errors.E_SSAPI_RCV_RESP_FAILED)
                sRet = "E_SSAPI_RCV_RESP_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_RCV_TIMEOUT)
                sRet = "E_SSAPI_RCV_TIMEOUT";
            else if (ssErr == ss_errors.E_SSAPI_READ_FILE_FAILED)
                sRet = "E_SSAPI_READ_FILE_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_SEND_REQ_FAILED)
                sRet = "E_SSAPI_SEND_REQ_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_SYS_RSC_ALLOC_FAILED)
                sRet = "E_SSAPI_SYS_RSC_ALLOC_FAILED";
            else if (ssErr == ss_errors.E_SSAPI_TIMEOUT)
                sRet = "E_SSAPI_TIMEOUT";
            else if (ssErr == ss_errors.E_SSAPI_XML_MATCH_NOT_FOUND)
                sRet = "E_SSAPI_XML_MATCH_NOT_FOUND";
            else 
                sRet = "__UNKNOW__" + ssErr;

            return sRet;
        }
        public static string getErrString(uint lError)
        {
            return getErrString((long)lError);
        }
    }
}
