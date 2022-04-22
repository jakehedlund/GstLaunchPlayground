using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace GstPlayground
{
    // Borrowed from: https://stackoverflow.com/a/6194818
    //  and: http://www.blackwasp.co.uk/CustomAppSettings_2.aspx
    [Serializable, SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class SerializableStringDictionary : System.Collections.Specialized.StringDictionary, System.Xml.Serialization.IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read() &&
               !(reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.LocalName == this.GetType().Name))
            {
                var name = reader["Name"];
                if (name == null)
                    throw new FormatException();

                var value = reader["Value"];
                this[name] = value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (System.Collections.DictionaryEntry entry in this)
            {
                writer.WriteStartElement("Pair");
                writer.WriteAttributeString("Name", (string)entry.Key);
                writer.WriteAttributeString("Value", (string)entry.Value);
                writer.WriteEndElement();
            }
        }
    }
}
