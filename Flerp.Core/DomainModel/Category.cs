using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Flerp.DomainModel
{
    public class Category
    {
        public Category() { }

        public string Name { get; set; }
        public bool AS { get; set; }
        public bool WS { get; set; }
        public bool AN { get; set; }
        public bool EN { get; set; }
        public bool LN { get; set; }
        public bool WN { get; set; }
        public bool AX { get; set; }
        public bool EX { get; set; }
        public bool LX { get; set; }
        public bool WX { get; set; }
        public string Permissions 
        { 
            get
            {
                var str = new StringBuilder();
                if (AS) str.Append("AS ");
                if (WS) str.Append("WS ");
                if (AN) str.Append("AN ");
                if (EN) str.Append("EN ");
                if (LN) str.Append("LN ");
                if (WN) str.Append("WN ");
                if (AX) str.Append("AX ");
                if (EX) str.Append("EX ");
                if (LX) str.Append("LX ");
                if (WX) str.Append("WX ");
                return str.ToString();
            }
        }
                

        public static Category Parse(string value)
        {
            var array = value.Split(';');

            var n = new Category
            {
                Name = array[0],
                AS = bool.Parse(array[1]),
                WS = bool.Parse(array[2]),
                AN = bool.Parse(array[3]),
                EN = bool.Parse(array[4]),
                LN = bool.Parse(array[5]),
                WN = bool.Parse(array[6]),
                AX = bool.Parse(array[7]),
                EX = bool.Parse(array[8]),
                LX = bool.Parse(array[9]),
                WX = bool.Parse(array[10]),
            };

            return n;
        }

        public string Serialize()
        {
            return string.Join(";",
                Name,
                AS.ToString(),
                WS.ToString(),
                AN.ToString(),
                EN.ToString(),
                LN.ToString(),
                WN.ToString(),
                AX.ToString(),
                EX.ToString(),
                LX.ToString(),
                WX.ToString()
                );
        }

        public static IEnumerable<Category> GetAll()
        {
            var categories = new List<Category>();

            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("Category_")))
                categories.Add(Category.Parse(ConfigurationManager.AppSettings[key]));

            return categories.OrderBy(x => x.Name);
        }
    }
}
