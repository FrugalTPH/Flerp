using PhoneNumbers;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Flerp.DomainModel
{
    public class PartyDetailType : EnumEx
    {
        public static readonly PartyDetailType Address = new PartyDetailType { Value = 0, DisplayName = "Address" };
        public static readonly PartyDetailType Email = new PartyDetailType { Value = 1, DisplayName = "Email" };
        public static readonly PartyDetailType Phone = new PartyDetailType { Value = 2, DisplayName = "Phone" };
        public static readonly PartyDetailType Web = new PartyDetailType { Value = 3, DisplayName = "Web" };
        public static readonly PartyDetailType Misc = new PartyDetailType { Value = 4, DisplayName = "Misc" };


        public static bool IsValid(string value, PartyDetailType category)
        {
            try
            {
                if (category.Equals(Phone)) { return IsPhoneValid(value); }
                if (category.Equals(Email)) { return IsEmailValid(value); }
                if (category.Equals(Web)) { return IsWebValid(value); }
                return category.Equals(Address) || category.Equals(Misc);
            }
            catch
            {
                return false;
            }
        }
        
        private static bool IsPhoneValid(string s)
        {
            var util = PhoneNumberUtil.GetInstance();
            var gb = util.Parse(s, "GB");
            return util.IsValidNumber(gb);
        }
        
        private static bool IsEmailValid(string s)
        {
            var addr = new MailAddress(s);
            return addr.Address == s;
        }
        
        private static bool IsWebValid(string s)
        {
            if (IsEmailValid(s)) { return false; }

            var reg = new Regex(
                @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$", 
                RegexOptions.Compiled | 
                RegexOptions.IgnoreCase);

            return reg.IsMatch(s);
        }
    }
}
