using System;
using System.IO;
using System.Xml.Serialization;

public static class SaveHelper
{
    public static string Serialize<T>(this T toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter stringWriter = new StringWriter();
        xml.Serialize(stringWriter, toSerialize);
        return stringWriter.ToString();
    }

    public static T Deserialize<T>(this string toDeserialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof (T));
        StringReader stringReader = new StringReader(toDeserialize);
        return (T)xml.Deserialize(stringReader);
    }
}
