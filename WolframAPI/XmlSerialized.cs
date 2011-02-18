namespace WolframAPI
{
    using System.IO;
    using System.Xml.Serialization;

    public abstract class XmlSerialized : ISerializableType
    {
        #region Implementation of ISerializableType

        /// <summary>
        /// Serializes the current instance and returns the result as a <see cref="string"/>
        /// <para>Should be used with XML serialization only.</para>
        /// </summary>
        /// <returns>The serialized instance.</returns>
        public string Serialize()
        {
            string data;

            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(GetType());
                var nss = new XmlSerializerNamespaces();
                nss.Add("", "");

                serializer.Serialize(ms, this, nss);

                using (var reader = new StreamReader(ms))
                {
                    data = reader.ReadToEnd();
                }
            }

            return data;
        }

        #endregion
    }
}
