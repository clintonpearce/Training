using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class AOLParser : IDomain
    {
        private String html;
        private Person p;

        public AOLParser(String html)
        {
            this.html = html;
            p = new Person();
        }

        public void parseEmail()
        {
            //Console.WriteLine("Html is : {0}", html);
            string AOLEmailPattern = "\"ActiveUserEmailAddress\":\"(\\w*@\\w*.\\w*)\"";
            Regex r = new Regex(AOLEmailPattern);

            // Match the regular expression pattern against a text string. 
            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                //Console.WriteLine(g);
                p.Email = g.ToString();
            }
        }

        public void parseUserName()
        {
            //Console.WriteLine("Html is : {0}", html);
            string AOLUserNamePattern = "\"ActiveUserScreenName\":\"(\\w*)\"";
            Regex r = new Regex(AOLUserNamePattern);

            // Match the regular expression pattern against a text string. 
            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                //Console.WriteLine(g);
                p.UserName = g.ToString();
            }
        }

        public void parseName()
        {
            //Console.WriteLine("Html is : {0}", html);
            string AOLNamePattern = "\"UserFullName\":\"(\\w*\\s\\w*)\"";
            Regex r = new Regex(AOLNamePattern);

            // Match the regular expression pattern against a text string. 
            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                //Console.WriteLine(g);
                p.Name = g.ToString();
            }
        }

        public void parseGender()
        {
            string AOLGenderPattern = "\"UserGender\":\"(\\w)\"";
            Regex r = new Regex(AOLGenderPattern);

            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                p.Gender = g.ToString();
            }
        }

        public Person getPerson()
        {
            return p;
        }
    }
}
