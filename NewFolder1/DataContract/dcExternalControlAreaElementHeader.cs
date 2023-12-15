using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace My_Client
{
    /// <summary>
    /// Remote Control Area Element Header Data
    /// </summary>
    [Serializable]
    public class ExternalControlAreaElementHeaderData : ICloneable
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
        public ExternalControlAreaElementHeaderData DeepCopy()
        {
            return (ExternalControlAreaElementHeaderData)this.MemberwiseClone();
        }

        /// <summary>
        /// 
        /// </summary>
        public ExternalControlAreaElementHeaderData()
        {
            SystemType = ExternalControlAreaTypes.Vessel;
            SessionID = 0;
            CommandName = ExternalControlAreaCommands.RequestData;
        }

        /// <summary>
        /// (읽기 전용) Remote Command Key
        /// </summary>
        public int CommandKey
        {
            get
            {
                return (int)CommandName;
            }
        }

        /// <summary>
        /// 시스템 ID
        /// </summary>
        [XmlElement(ElementName = "SystemType", Order = 0)]
        public ExternalControlAreaTypes SystemType { get; set; }

        /// <summary>
        /// Remote Command Name
        /// </summary>
        [XmlElement(ElementName = "CommandName", Order = 1)]
        public ExternalControlAreaCommands CommandName { get; set; }

        /// <summary>
        /// Session ID
        /// </summary>
        [XmlElement(ElementName = "SessionID", Order = 2)]
        public int SessionID { get; set; }
    }
}
