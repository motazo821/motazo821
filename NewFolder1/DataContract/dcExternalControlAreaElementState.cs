using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace My_Client
{    
    /// <summary>
    /// External Control Area Element State Data
    /// </summary>
    [Serializable]
    public class ExternalControlAreaElementStateData : ICloneable
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
        public ExternalControlAreaElementStateData DeepCopy()
        {
            return (ExternalControlAreaElementStateData)this.MemberwiseClone();
        }

        /// <summary>
        /// 
        /// </summary>
        public ExternalControlAreaElementStateData()
        {
            ControlState = ExternalControlAreaStates.Unknown;
            Comment = "";
        }

        /// <summary>
        /// 처리 결과 판정
        /// </summary>
        [XmlIgnore]
        public bool IsResultOK
        {
            get
            {
                return ControlState == ExternalControlAreaStates.Run;
            }
        }

        /// <summary>
        /// State
        /// </summary>
        [XmlElement(ElementName = "ControlState", Order = 0)]
        public ExternalControlAreaStates ControlState { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [XmlElement(ElementName = "Comment", Order = 1)]
        public string Comment { get; set; }
    }
}
