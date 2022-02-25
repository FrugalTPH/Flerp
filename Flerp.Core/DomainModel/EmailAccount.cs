using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Flerp.DomainModel
{
    public class EmailAccount
    {
        public bool Active { get; set; }
        public string Uri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SpecificMailbox { get; set; }
        public string SpecifiedPort { get; set; }
        public bool UseSsl { get; set; }
        public int Cap { get { return int.Parse(ConfigurationManager.AppSettings["EmailDownloadCap"]); } }

        public EmailAccount() { }

        public static EmailAccount Parse(string value)
        {
            var array = value.Split(';');

            var n = new EmailAccount
            {
                Active = bool.Parse(array[0]),
                Uri = array[1],
                UserName = array[2],
                Password = array[3],
                SpecificMailbox = array[4],
                SpecifiedPort = array[5],
                UseSsl = bool.Parse(array[6])
            };

            return n;
        }

        public string Serialize()
        {
            return string.Join(";",
                Active.ToString(), 
                Uri, 
                UserName, 
                Password, 
                SpecificMailbox, 
                SpecifiedPort, 
                UseSsl.ToString()
                );
        }

        public static IEnumerable<EmailAccount> GetAll()
        {
            var accounts = new List<EmailAccount>();
            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("EmailAccount_")))
                accounts.Add(EmailAccount.Parse(ConfigurationManager.AppSettings[key]));
            return accounts.Where(x => x.Active);
        }
    }
}
