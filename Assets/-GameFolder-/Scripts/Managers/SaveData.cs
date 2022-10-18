using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
[XmlRoot("SaveData")]
public class SaveData 
{
    [XmlArray("Purchased"), XmlArrayItem("Purchase")]
    public List<bool> purchased = new List<bool>();
    [XmlArray("Upgraded"), XmlArrayItem("Upgrade")]
    public List<int> upgraded = new List<int>();

    public void Save()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, this);
            PlayerPrefs.SetString("Save", writer.ToString());
        }
    }

    public static SaveData Load()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

        using (StringReader reader = new StringReader(PlayerPrefs.GetString("Save")))
        {
            if (PlayerPrefs.HasKey("Save"))
            {
                return serializer.Deserialize(reader) as SaveData;
            }
            else
            {
                return new SaveData();
            }
        }
    }
}
