using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface IDomain
    {
        void parseEmail();
        void parseName();
        void parseGender();
        Person getPerson();
    }
}
