using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class CraigslistParser : IDomain
    {
        private String html;
        private Person p;

        public CraigslistParser(String html)
        {
            this.html = html;
            p = new Person();
        }

        public void parseEmail()
        {
            string craigslistEmailPatt = @"<b><a href='https://accounts.craigslist.org/login'>***********@*****.***</a>";
            string craigslistEmailPatt2 = @"name='FromEMail' value='(.*@.*\..*)'\>";

            Regex r = new Regex(craigslistEmailPatt);
            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                Console.WriteLine(g);
                p.Email = g.ToString();
            }
            else
            {
                r = new Regex(craigslistEmailPatt2);
                m = r.Match(html);
                if (m.Success)
                {
                    Group g = m.Groups[1];
                    Console.WriteLine(g);
                    p.Email = g.ToString();
                }
            }

        }

        public void parseName()
        {
           
        }

        public void parseGender()
        {

        }

        public Person getPerson()
        {
            return p;
        }

    }
}
