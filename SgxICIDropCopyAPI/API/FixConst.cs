using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.API
{
    public class FixConst
    {
        public const char Delimeter = '\u0001';
        public const char Equalto = '=';
        public const string FixEndString = "\u000110=";
        public const string BeginigString = "FIX.4.2";
        public const string TimeFormat = "yyyyMMdd-HH:mm:ss.fff";
        public static string[] DelimeterS;
    }
}
