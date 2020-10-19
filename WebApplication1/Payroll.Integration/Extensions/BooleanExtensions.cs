namespace Payroll.Integration.Extensions
{
    public static class BooleanExtensions
    {
        private const string StringRepresentationOfTrue = "true";
        private const string PayrollDcSchemeValue = "11";
        private const string PayrollNotInSchemeValue = "00";
        private const string StringRepresentationOfFalse = "false";
        private const string PayrollTrueBoolFlag = "Y";
        private const string PayrollFalseBoolFlag = "N";

        public static string ToYesNoString(this bool value)
        {
            return value ? PayrollTrueBoolFlag : PayrollFalseBoolFlag;
        }

        public static string ToTrueFalseString(this string value)
        {
            return value == PayrollTrueBoolFlag ? StringRepresentationOfTrue : StringRepresentationOfFalse;
        }

        public static bool FromYorNString(this string value)
        {
            return value == PayrollTrueBoolFlag;
        }

        public static string ToIsInSchemeString(this string value)
        {
            return value == PayrollDcSchemeValue ? StringRepresentationOfTrue : StringRepresentationOfFalse;
        }

        public static bool SchemeToBool(this string value)
        {
            return value == PayrollDcSchemeValue;
        }

        public static string ToIsInSchemePayrollString(this bool value)
        {
            return value ? PayrollDcSchemeValue : PayrollNotInSchemeValue;
        }

        public static string ToPayrollRepresentation(this double theValue)
        {
            // 006.00
            return theValue.ToString("F").PadLeft(6, '0');
        }
    }
}