using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class AmazonParser : IDomain
    {
        private String html;
        private Person p;

        public AmazonParser(String html)
        {
            this.html = html;
            p = new Person();
        }

        public void parseEmail()
        {

        }

        public void parseUserName()
        {

        }

        public void parseName()
        {
            //Console.WriteLine("Html is : {0}", html);
            string amazonNamePattern = @"id='nav-signin-text'<?[^>]*>(.*?)</span>"; 
                        Regex r = new Regex(amazonNamePattern);   

                        // Match the regular expression pattern against a text string. 
                        Match m = r.Match(html); 
                        if(m.Success && m.Groups[1].ToString()!="Sign in") 
                        { 
                            Group g = m.Groups[1]; 
                            //Console.WriteLine(g);
                            p.Name = g.ToString();
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
