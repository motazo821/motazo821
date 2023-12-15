using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace My_Client
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "external", Namespace = "")]
    public class ExternalControlDeviceHeaderData : ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ExternalControlDeviceHeaderData DeepCopy()
        {
            ExternalControlDeviceHeaderData data = (ExternalControlDeviceHeaderData)this.MemberwiseClone();
            if (Header != null)
                data.Header = Header.DeepCopy();

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExternalControlDeviceHeaderData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "Header", Order = 0)]
        public ExternalControlAreaElementHeaderData Header { get; set; }
    }
}
