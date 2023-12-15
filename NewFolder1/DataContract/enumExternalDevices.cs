using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace My_Client
{
    /// <summary>
    /// 
    /// </summary>
    public enum ExternalControlAreaTypes
    {
        /// <summary>
        /// Vessel Scale
        /// </summary>
        //[Description("Vessel Scale")]
        Vessel,

        /// <summary>
        /// Bubble Free System
        /// </summary>
        //[Description("Bubble Free System")]
        BFS,
    }

    /// <summary>
    /// 외부 시스템 동작 상태
    /// </summary>
    public enum ExternalControlAreaStates
    {
        /// <summary>
        /// 없음
        /// </summary>
        //[Description("")]
        Unknown = 0,

        /// <summary>
        /// 운전
        /// </summary>
        //[Description("")]
        Run = 1,

        /// <summary>
        /// 정지
        /// </summary>
        //[Description("")]
        Stop = 2,

        /// <summary>
        /// 에러
        /// </summary>
        //[Description("")]
        Error = 3,
    }

    /// <summary>
    /// 외부 제어 명령어
    /// </summary>
    public enum ExternalControlAreaCommands
    {
        /// <summary>
        /// 현재 상태 데이터 요청
        /// </summary>
        RequestData,

        /// <summary>
        /// Circulation Circuit RPM 변경
        /// </summary>
        ChangeData,

        /// <summary>
        /// 
        /// </summary>
        SendData,
    }

    /// <summary>
    /// 외부 시스템 제어 모드
    /// </summary>
    public enum ExternalControlAreaModes
    {
        /// 자동 압력 계통 보정
        /// </summary>
        Active,

        /// <summary>
        /// 지령 파나미터(펌프 RPM, Back-Pressure) 값으로 압력 계통 구동
        /// </summary>
        Passive,
    }
}
