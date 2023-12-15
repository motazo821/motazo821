using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace My_Client
{
    /// <summary>
    /// Exteranal Control Area Device Data
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "external", Namespace = "")]
    public class ExternalControlAreaDeviceData : ICloneable
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
        public ExternalControlAreaDeviceData DeepCopy()
        {
            ExternalControlAreaDeviceData ecadData = (ExternalControlAreaDeviceData)this.MemberwiseClone();

            if (this.ExternalVesselLevelStates != null)
                ecadData.ExternalVesselLevelStates = this.ExternalVesselLevelStates.Select(x => x.DeepCopy()).ToArray();

            if (this.ExternalBubbleFreeSystemState != null)
                ecadData.ExternalBubbleFreeSystemState = this.ExternalBubbleFreeSystemState.DeepCopy();

            return ecadData;
        }

        [XmlElement(ElementName = "Header", Order = 0)]
        public ExternalControlAreaElementHeaderData Header { get; set; }

        /// <summary>
        /// 외부 시스템 동작 상태
        /// </summary>
        [XmlElement(ElementName = "State", Order = 1)]
        public ExternalControlAreaElementStateData State { get; set; }

        /// <summary>
        /// 외부 용기 상태 데이터 (무게: kg)
        /// </summary>
        [XmlElement(ElementName = "Vessel", Order = 2)]
        public VesselLevelStateData[] ExternalVesselLevelStates { get; set; }

        /// <summary>
        /// 외부 Bubble Free System 상태 데이터
        /// </summary>
        [XmlElement(ElementName = "BFS", Order = 3)]
        public ExternalElementBFSData ExternalBubbleFreeSystemState { get; set; }
    }

    /// <summary>
    /// Scale Tank Data
    /// </summary>
    [Serializable]
    public class VesselLevelStateData : ICloneable
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
        public VesselLevelStateData DeepCopy()
        {
            return (VesselLevelStateData)this.MemberwiseClone();
        }

        /// <summary>
        /// Tank ID (00 - 04)
        /// </summary>
        [XmlAttribute(AttributeName = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// 그룹 ID
        /// </summary>
        [XmlAttribute(AttributeName = "Group")]
        public int GroupID { get; set; }

        /// <summary>
        /// 최대 무게
        /// </summary>
        [XmlElement(ElementName = "MaxWeight", Order = 0)]
        public double MaxWeight { get; set; }

        /// <summary>
        /// 현재 무게
        /// </summary>
        [XmlElement(ElementName = "CurrentWeight", Order = 1)]
        public double CurrentWeight { get; set; }

        /// <summary>
        /// State Code (0: OK, Others: NG)
        /// </summary>
        [XmlElement(ElementName = "StateCode", Order = 2)]
        public int StateCode { get; set; }
    }

    /// <summary>
    /// BFS 매개 변수 데이터
    /// </summary>
    [Serializable]
    public class ExternalElementBFSData : ICloneable
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
        public ExternalElementBFSData DeepCopy()
        {
            return (ExternalElementBFSData)this.MemberwiseClone();
        }

        /// <summary>
        /// Valve Index (00 - 03)
        /// </summary>
        [XmlAttribute(AttributeName = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// 운전 모드
        ///  - Active: 자동 압력(RPM) 보정, Passive: 지령 RPM 으로 Inverter 구동
        /// </summary>
        [XmlAttribute(AttributeName = "Mode")]
        public ExternalControlAreaModes ControlMode { get; set; }

        /// <summary>
        /// 목표 값 (mm)
        /// </summary>
        [XmlElement(ElementName = "TargetWidth", Order = 0)]
        public double TargetWidth { get; set; }

        /// <summary>
        /// 현재 측정 값 (mm)
        /// </summary>
        [XmlElement(ElementName = "MeasuredWidth", Order = 1)]
        public double MeasuredWidth { get; set; }

        /// <summary>
        /// 펌프 RPM 값 (-1 = default speed)
        /// </summary>
        [XmlElement(ElementName = "RPM", Order = 2)]
        public int PumpRPM { get; set; }

        /// <summary>
        /// 역 압력 (psi, -1 = default pressure)
        /// </summary>
        [XmlElement(ElementName = "Pressure", Order = 3)]
        public int BackPressure { get; set; }

        /// <summary>
        /// 설정 변경 후 안정화 지연 시간(ms)
        /// </summary>
        [XmlElement(ElementName = "StabilizingTime", Order = 4)]
        public int StabilizingTime { get; set; }
    }
}
