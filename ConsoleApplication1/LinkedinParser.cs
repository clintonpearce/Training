using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class LinkedinParser : IDomain
    {
        private String html;
        private Person p;

        public LinkedinParser(String html)
        {
            this.html = html;
            p = new Person();
        }

        public void parseEmail()
        {

        }

        public void parseName()
        {
            //Console.WriteLine("Html is : {0}", html);
            string linkedinNamePattern = "img-defer-hidden img-defer nav-profile-photo.*alt=\"(.+?)\"";
            string linkedinEmailPattern1 = ".*ismiss this message.*email\":\"(.+@.+?)\".*";
            string linkedinEmailPattern2 = ".*name=\"email\" value=\"(.+@.+?)\".*";


            Regex r = new Regex(linkedinNamePattern);

            // Match the regular expression pattern against a text string. 
            Match m = r.Match(html);
            if (m.Success)
            {
                Group g = m.Groups[1];
                //Console.WriteLine(g);
                p.Name = g.ToString();
            }

            Regex mEmail = new Regex(linkedinEmailPattern1);
            Match e = mEmail.Match(html);
            Regex mEmail2 = new Regex(linkedinEmailPattern2);
            Match e2 = mEmail.Match(html);
            if (e.Success)
            {
                Group g = e.Groups[1];
                p.Email = g.ToString();
            }
            if(e2.Success)
            {
                Group g = e2.Groups[1];
                p.Email = g.ToString();
            }
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
