using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace kicom {

    // To FaceAPI
    public class Person {
        public string name { get; set; }
        public string imgname { get; set; }
        public string relation { get; set; }
        public string etc { get; set; }
        public string faceid { get; set; }

        public Person(string name, string imgname, string relation, string etc, string faceid = null) {
            this.name = name;
            this.imgname = imgname;
            this.relation = relation;
            this.etc = etc;
            this.faceid = null;
        }
    }

    public class VisitorInfo {
        public string filepath { get; set; }
        public Boolean stranger { get; set; }

        public VisitorInfo(Boolean stranger, string filepath) {
            this.filepath = filepath;
            this.stranger = stranger;
        }
    }

    // To XMLWriter
    public class Result {
        public string name { get; set; }
        public string filepath { get; set; }
        public string relation { get; set; }

        public Result(string name, string filepath, string relation) {
            this.name = name;
            this.filepath = filepath;
            this.relation = relation;
        }
    }
}
