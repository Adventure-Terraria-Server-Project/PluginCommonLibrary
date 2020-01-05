using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common.Collections {
  public class SerializableDictionary<TKey, TValue>: Dictionary<TKey, TValue>, IXmlSerializable {
    public string ItemNodeName { get; private set; }
    protected string KeyNodeName { get; private set; }
    public string ValueNodeName { get; private set; }


    protected SerializableDictionary(string itemNodeName, string keyNodeName, string valueNodeName = "Value") {
      this.ItemNodeName = itemNodeName;
      this.KeyNodeName = keyNodeName;
      this.ValueNodeName = valueNodeName;
    }

    #region [IXmlSerializable Implementation]
    public XmlSchema GetSchema() {
      return null;
    }

    public void ReadXml(XmlReader reader) {
      if (reader.IsEmptyElement) {
        reader.Read();
        return;
      }

      XmlSerializer keySerializer = null;
      XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
      Type keyType = typeof(TKey);
      bool isFlatKey = (keyType.FullName == "System.String" || keyType.FullName == "System.Drawing.Point" || keyType.IsEnum);

      if (!isFlatKey)
        keySerializer = new XmlSerializer(typeof(TKey));

      reader.Read();
      while (reader.ReadToNextSibling(this.ItemNodeName)) {
        TKey key;
        TValue value;
        if (isFlatKey) {
          if (keyType.FullName == "System.String") {
            key = (TKey)(object)reader.GetAttribute(this.KeyNodeName);
          } else if (keyType.FullName == "System.Drawing.Point") {
            key = (TKey)(object)PointEx.Parse(reader.GetAttribute(this.KeyNodeName));
          } else if (keyType.IsEnum) {
            string keyRaw = reader.GetAttribute(this.KeyNodeName);
            if (keyRaw == null) {
              throw new FormatException(
                $"The XML-Attribute \"{this.KeyNodeName}\" is missing on element \"{reader.Name}\"."
              );
            }

            key = (TKey)Enum.Parse(keyType, keyRaw);
          } else {
            throw new InvalidOperationException();
          }

          reader.ReadStartElement(this.ItemNodeName);
          value = (TValue)valueSerializer.Deserialize(reader);
        } else {
          reader.ReadStartElement(this.ItemNodeName);

          reader.ReadStartElement(this.KeyNodeName);
          key = (TKey)keySerializer.Deserialize(reader);
          reader.ReadEndElement();

          reader.ReadStartElement(this.ValueNodeName);
          value = (TValue)valueSerializer.Deserialize(reader);
          reader.ReadEndElement();
        }
        reader.ReadEndElement();

        this.Add(key, value);
      }

      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer) {
      XmlSerializer keySerializer = null;
      XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
      Type keyType = typeof(TKey);
      bool isFlatKey = (keyType.FullName == "System.String" || keyType.FullName == "System.Drawing.Point" || keyType.IsEnum);

      if (!isFlatKey)
        keySerializer = new XmlSerializer(typeof(TKey));

      foreach (KeyValuePair<TKey,TValue> pair in this) {
        writer.WriteStartElement(this.ItemNodeName);

        if (isFlatKey) {
          string keyString;
          if (keyType.FullName == "System.Drawing.Point")
            keyString = ((DPoint)(object)pair.Key).ToSimpleString();
          else 
            keyString = pair.Key.ToString();

          writer.WriteAttributeString(this.KeyNodeName, keyString);

          valueSerializer.Serialize(writer, pair.Value);
        } else {
          writer.WriteStartElement(this.KeyNodeName);
          keySerializer.Serialize(writer, pair.Key);
          writer.WriteEndElement();

          writer.WriteStartElement(this.ValueNodeName);
          valueSerializer.Serialize(writer, pair.Value);
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }
    }
    #endregion
  }
}
