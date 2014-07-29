using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class NetflixParser : IDomain
    {
        private String html;
        private Person p;

        public NetflixParser(String html)
        {
            this.html = html;
            p = new Person();
        }

        public void parseEmail()
        {
            string netflixEmailPattern = "input.*id=\"email\".*value=\"(.+@.+?)\".*";

            Regex r = new Regex(netflixEmailPattern);
            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                Console.WriteLine(g);
                p.Email = g.ToString();
            }
            else
            {

            }

        }

        public void parseName()
        {
            //Console.WriteLine("Html is : {0}", html);
            string netflixNamePattern = "\"cfn\":\"(.*?)\"";
            Regex r = new Regex(netflixNamePattern);

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

        }

        public void parseUserName()
        {

        }

        public Person getPerson()
        {
            return p;
        }
    }
}
