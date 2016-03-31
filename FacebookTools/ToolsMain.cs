using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookTools
{
    class ToolsMain
    {
        static void Main(string[] args)
        {
            FacebookHelper helper = new FacebookHelper("");
            helper.GetFamily();
        }
    }
}
