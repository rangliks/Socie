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
            FacebookHelper helper = new FacebookHelper("CAAC3FZAauWzEBAP7FQAZBT0P2m5zIL1ImfyRcgOy1qJWIanCyhmiD1GszN0Fk0gI4ZBL2bOaxIGln3NlrlzPnZA3Nchfy0xdfSSumgBbfQLaRSOTtl4I4LZCVxh4zG0ETOrLYAsFRZBOwdUyycmAbRrs66AGghNVN9FL2C761UXUrGmZBdnzsGmG4IOD5K5tul3vjNZA6qm8pwZDZD");
            helper.GetFamily();
        }
    }
}
