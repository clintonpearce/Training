using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Person
    {
        private String name;
        private String email;
        private String gender;
        public bool NameExists { get; set; }
        public bool EmailExists { get; set; }
        public bool GenderExists { get; set; }

        public Person()
        {

        }

        public String Name
        {
            get { return name; }
            set { name = value; NameExists = true; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; EmailExists = true; }
        }

        public String Gender
        {
            get { return gender; }
            set { gender = value; GenderExists = true; }
        }
    }
}
