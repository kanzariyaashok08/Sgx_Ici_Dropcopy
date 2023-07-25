using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.API
{
    public class FIXMethods
    {
        public static string GetRejectionReason(int rejectedcode)
        {
            switch (rejectedcode)
            {
                case 0:
                    return "Invalid tag number";
                case 1:
                    return "Required tag missing";
                case 2:
                    return "Tag not defined for this message type";
                case 3:
                    return "Undefined Tag";
                case 4:
                    return "Tag specified without a value";
                case 5:
                    return "Incorrect value";
                case 6:
                    return "Incorrect data format for a tag value";
                case 7:
                    return "Decryption problem";
                case 8:
                    return "Signature problem";
                case 9:
                    return "CompID problem";
                case 10:
                    return "Sending time accuracy problem";
                case 11:
                    return "Invalid message type";
                case 99:
                    return "Other";
                default:
                    return "Unknown";
            }
        }

        public static string GetOrdRejReason(int rejreason)
        {
            switch (rejreason)
            {
                case 0:
                    return "Broker option";
                case 1:
                    return "Unknown symbol";
                case 2:
                    return "Exchange closed";
                case 3:
                    return "Order exceeds limit";
                case 4:
                    return "Too late to enter";
                case 5:
                    return "Unknown order";
                case 6:
                    return "Duplicate order";
                case 7:
                    return "Duplicate of a verbally-communicated order";
                case 8:
                    return "Stale order";
                case 9:
                    return "Trade along required";
                case 10:
                    return "Invalid Investor ID";
                case 11:
                    return "Unsupported order characteristic";
                case 12:
                    return "Surveillance option";
                case 13:
                    return "Incorrect quantity";
                case 14:
                    return "Incorrect allocated quantity";
                case 15:
                    return "Unknown account";
                case 16:
                    return "Price exceeds current price band";
                case 18:
                    return "Invalid price increment";
                case 19:
                    return "Message pending";
                case 20:
                    return "Routing error";
                case 99:
                    return "Other";
                case 1003:
                    return "Market closed";
                case 1007:
                    return "FIX field missing or incorrect";
                case 1010:
                    return "Required field missing";
                case 1011:
                    return "FIX field incorrect";
                case 1012:
                    return "Price must be greater than zero";
                case 1013:
                    return "Invalid order qualifier";
                case 1014:
                    return "User not authorized";
                case 2013:
                    return "Market hours not suported by opposite";
                case 2019:
                    return "Invalid expire date";
                case 2044:
                    return "Order not in book";
                case 2045:
                    return "Order not in book 2";
                case 2046:
                    return "Disclosed qty cannot be greater";
                case 2047:
                    return "Unknown contract";
                case 2048:
                    return "Cancel with different sender comp id";
                case 2049:
                    return "ClOrdId different than correleation ClOrdId";
                case 2050:
                    return "ClOrdId different than original ClOrdId";
                case 2051:
                    return "Different side";
                case 2052:
                    return "Different group";
                case 2053:
                    return "Different security type";
                case 2054:
                    return "Different account";
                case 2055:
                    return "Different qty";
                case 2056:
                    return "Cancel with different trader id";
                case 2058:
                    return "Stop price must be greater";
                case 2059:
                    return "Stop price must be smaller";
                case 2060:
                    return "Sell stop price must be below ltp";
                case 2061:
                    return "Buy stop price must be above ltp";
                case 2100:
                    return "Different product";
                case 2101:
                    return "Different inflight fill modification";
                case 2102:
                    return "Modify with different sender comp id";
                case 2103:
                    return "Modify with different trader id";
                case 2115:
                    return "Order qty outside allowable range";
                case 2130:
                    return "Invalid order type for pcp";
                case 2137:
                    return "Order price outside limits";
                case 2179:
                    return "Order price outside bands";
                case 2311:
                    return "Invalid order type for group";
                case 2500:
                    return "Instrument cross request in process";
                case 2501:
                    return "Ordr qty too low";
                case 2600:
                    return "Market maker protection has tripped";
                case 4000:
                    return "Engine did not respond";
                case 6001:
                    return "Pending replace";
                case 6002:
                    return "Pending cancel";
                case 7000:
                    return "Order rejected";
                case 7001:
                    return "Contract not gtc gtd eligible";
                case 7009:
                    return "Contract past expiration";
                case 7011:
                    return "Max contract working qty exceeded";
                case 7015:
                    return "Modify with different side";
                case 7018:
                    return "Contract not gtc gtd eligible 2";
                case 7020:
                    return "No trading calendar for expire date";
                case 7021:
                    return "Expire date beyond instrument expiration";
                case 7022:
                    return "Expire date beyond leg instrument expiration";
                case 7024:
                    return "Market in no cancel";
                case 7027:
                    return "Invalid order type for reserved market";
                case 7028:
                    return "Order session date in past";
                case 7613:
                    return "Disclosed qty cannot be smaller";
                case 9999:
                    return "Technical error function not performed";
                default:
                    return "Unknown";
            }
        }

        public static string GetBooleanValue(bool boolValue)
        {
            if (boolValue)
                return "Y";
            else
                return "N";
        }

        public static bool SetBooleanValue(string value)
        {
            if (value == "Y")
                return true;
            else
                return false;
        }

        public static string GenerateCheckSum(char[] checksumdata)
        {
            int sum = 0;
            long i;
            for (i = 0L; i < checksumdata.Length; i++)
            {
                sum += checksumdata[i];
            }
            int checksum = sum % 256;
            return checksum.ToString().PadLeft(3, '0');
        }

        public static bool IsValidChecksum(string checksum)
        {
            try
            {
                string tempcheckSum = GenerateCheckSum(checksum.ToCharArray());
                if (Convert.ToInt32(tempcheckSum) == Convert.ToInt32(checksum.Trim()))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string GetMsgType(string strdata)
        {
            string msgtype = string.Empty;
            string[] arrydata = strdata.Split(FixConst.Delimeter);

            foreach (string item in arrydata)
            {
                string[] arrySubdata = item.Split('=');
                if (arrySubdata[0] == "35")
                {
                    msgtype = arrySubdata[1];
                    break;
                }
            }

            return msgtype;
        }

       

    }

}
