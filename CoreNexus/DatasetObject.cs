using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNexus
{
    public class DatasetObject
    {
        public string name = "datasetobject";
        public List<string> data;

        public DatasetObject(List<string> data)
        {
            this.data = data;
        }
    }
}
