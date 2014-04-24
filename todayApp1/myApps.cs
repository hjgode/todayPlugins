using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace todayApp1
{
    
    public class theApps
    {
        public List<myApps.myApp> myAppList = new List<myApps.myApp>();

        public theApps()
        {
            loadApps();
        }

        string sAppPath = @"\Program Files\managed today screen framework\";
        string sAppXmlFile = "myApps.xml";

        public bool loadApps()
        {
            bool bRet = false;
            string filename = sAppPath + sAppXmlFile;
            if (System.IO.File.Exists(filename))
            {
                readFile(filename);
                bRet = true;
            }
            return bRet;
        }
        private void readFile(String filename)
        {
            try
            {
                myApps mApps = new myApps();
                mApps = mApps.ReadFile(filename);
                
                foreach (myApps.myApp app in mApps._myApps)
                    myAppList.Add(app);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception in deserialize: " + ex.Message);
            }
        }
    }

    [XmlRoot("myApps")]
    public class myApps
    {
        [XmlElement("myApp")]
        public myApp[] _myApps;

        private XmlSerializer s = null;
        private Type type = null;
        public myApps(){
                this.type = typeof(myApps);
                this.s = new XmlSerializer(this.type);
        }
        public myApps Deserialize(string xml)
        {
            TextReader reader = new StringReader(xml);
            return Deserialize(reader);
        }
        public myApps Deserialize(TextReader reader)
        {
            myApps o = (myApps)s.Deserialize(reader);
            reader.Close();
            return o;
        }
        public myApps Deserialize(XmlDocument doc)
        {
            TextReader reader = new StringReader(doc.OuterXml);
            return Deserialize(reader);
        }
        public myApps ReadFile(string sXMLfile)
        {
            XmlSerializer xs = new XmlSerializer(typeof(myApps));
            //StreamReader sr = new StreamReader("./SystemHealth.xml");
            StreamReader sr = new StreamReader(sXMLfile);
            myApps s = (myApps)xs.Deserialize(sr);
            sr.Close();
            return s;
        }
        public static void serialize(myApps myapps, string sXMLfile)
        {
            XmlSerializer xs = new XmlSerializer(typeof(myApps));
            //omit xmlns:xsi from xml output
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //Add an empty namespace and empty value
            ns.Add("", "");
            //StreamWriter sw = new StreamWriter("./SystemHealth.out.xml");
            StreamWriter sw = new StreamWriter(sXMLfile);
            xs.Serialize(sw, myapps, ns);
        }

        public class myApp
        {
            [XmlElement("title")]
            public string title;
            [XmlElement("iconFile")]
            public string iconFile = "";
            [XmlElement("exeFile")]
            public string exeFile = "";
            [XmlElement("args")]
            public string args;

            public myApp()
            {
            }
        }
    }

}
