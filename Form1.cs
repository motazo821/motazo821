using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;  // 추가
using System.Net; // 추가
using System.Net.Sockets;  // 추가
using System.IO;  // 추가
using System.Xml;
using System.Xml.Serialization;

namespace My_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        StreamReader streamReader;  // 데이타 읽기 위한 스트림리더
        StreamWriter streamWriter;  // 데이타 쓰기 위한 스트림라이터 
        TcpClient tcpClient1;  // TcpClient 객체 생성
        ExternalControlAreaDeviceData scaleData = new ExternalControlAreaDeviceData();


        private void button1_Click(object sender, EventArgs e)  // '연결하기' 버튼이 클릭되면
        {
            Thread thread1 = new Thread(connect);  // Thread 객채 생성, Form과는 별도 쓰레드에서 connect 함수가 실행됨.
            thread1.IsBackground = true;  // Form이 종료되면 thread1도 종료.
            thread1.Start();  // thread1 시작.
        }

        private void connect()  // thread1에 연결된 함수. 메인폼과는 별도로 동작한다.
        {
             tcpClient1 = new TcpClient();
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));  // IP주소와 Port번호를 할당
            tcpClient1.Connect(ipEnd);  // 서버에 연결 요청

            writeRichTextbox("서버 연결됨...");

            streamReader = new StreamReader(tcpClient1.GetStream());  // 읽기 스트림 연결
            streamWriter = new StreamWriter(tcpClient1.GetStream());  // 쓰기 스트림 연결
            streamWriter.AutoFlush = true;  // 쓰기 버퍼 자동으로 뭔가 처리..

            while (tcpClient1.Connected)  // 클라이언트가 연결되어 있는 동안
            {
                #region 구조 생성
                List<VesselLevelStateData> vessels = new List<VesselLevelStateData>();
                {
                    for (int i = 0; i < 4; i++)
                    {
                        VesselLevelStateData vessel = new VesselLevelStateData()
                        {
                            StateCode = 0,
                            CurrentWeight = 0,
                            GroupID = 0,
                            ID = 0,
                            MaxWeight = 100,
                        };
                        vessels.Add(vessel);
                    }
                }


                ExternalControlAreaDeviceData data = new ExternalControlAreaDeviceData()
                {
                    Header = new ExternalControlAreaElementHeaderData()
                    {
                        CommandName = ExternalControlAreaCommands.RequestData,
                        SessionID = 0,
                        SystemType = ExternalControlAreaTypes.Vessel,
                    },

                    State = new ExternalControlAreaElementStateData()
                    {
                        ControlState = ExternalControlAreaStates.Run,
                        Comment = "",
                    },

                    ExternalVesselLevelStates = vessels.ToArray(),
                    ExternalBubbleFreeSystemState = new ExternalElementBFSData(),
                };
                #endregion 구조 생성


                NetworkStream streamNetwork = tcpClient1.GetStream();

                //Encoding encoding = Encoding.UTF8;

                //string receiveData1 = streamReader.ReadLine();// 수신 데이타를 읽어서 receiveData1 변수에 저장
                string receiveData1;
                string receiveData2;
                byte[] receiveData = new byte[1060];
                streamNetwork.Read(receiveData, 0, receiveData.Length);
                scaleData = GetXMLDeserializedExternalControlDeviceData(receiveData);
                double weight1 = scaleData.ExternalVesselLevelStates[0].CurrentWeight;
                double weight2 = scaleData.ExternalVesselLevelStates[1].CurrentWeight;
                receiveData1 = weight1.ToString();
                receiveData2 = weight2.ToString();




                writeRichTextbox("Scale1 :" + receiveData1);  // 데이타를 수신창에 쓰기
                writeRichTextbox("Scale2 :" + receiveData2);
            }
        }

        private void writeRichTextbox(string data)  // richTextbox1 에 쓰기 함수
        {
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.AppendText(data + "\r\n"); }); //  데이타를 수신창에 표시, 반드시 invoke 사용. 충돌피함.
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.ScrollToCaret(); });  // 스크롤을 젤 밑으로.
        }

        private void button2_Click(object sender, EventArgs e)  // '보내기' 버튼이 클릭되면
        {
            #region 구조 생성
            List<VesselLevelStateData> vessels = new List<VesselLevelStateData>();
            {

                VesselLevelStateData vessel = new VesselLevelStateData()
                {
                    StateCode = 0,
                    CurrentWeight = 0,
                    GroupID = 0,
                    ID = 0,
                    MaxWeight = 100,
                };
                vessels.Add(vessel);
            }


            ExternalControlAreaDeviceData data = new ExternalControlAreaDeviceData()
            {
                Header = new ExternalControlAreaElementHeaderData()
                {
                    CommandName = ExternalControlAreaCommands.RequestData,
                    SessionID = 0,
                    SystemType = ExternalControlAreaTypes.Vessel,
                },

                State = new ExternalControlAreaElementStateData()
                {
                    ControlState = ExternalControlAreaStates.Run,
                    Comment = "",
                },

                ExternalVesselLevelStates = vessels.ToArray(),
                ExternalBubbleFreeSystemState = new ExternalElementBFSData(),
            };
            #endregion 구조 생성
            NetworkStream streamNetwork = tcpClient1.GetStream();
            byte[] sendDatas = GetXMLSerializedScaleData(data);
            streamNetwork.Write(sendDatas,0,sendDatas.Length);   // 스트림라이터를 통해 데이타를 전송

             

            // TODO : 응답 올때가지 기다려야겠지
            // TODO : 메인에서 기다리면 뻗지
            // TODO : 나중에는 클래스 처리해서 UI 뻗지 않게 처리

            // TODO : 기다리다 정보 들어오면 표현

            // HACK : 데이터가 없을때 처리가 어떻게 되는지??
        }

        #region XML - Serialize

        /// <summary>
        /// SCCData XML Deserialized - 제어코드 자동 제거
        /// </summary>
        /// <param name="bytes">수신 데이터 바이트 배열</param>
        /// <returns>SCCData 변환 객체</returns>
        public ExternalControlDeviceHeaderData GetXMLDeserializedExternalControlData(byte[] bytes)
        {
            ExternalControlDeviceHeaderData sccData = null;
            try
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExternalControlDeviceHeaderData));
                    sccData = (ExternalControlDeviceHeaderData)xmlSerializer.Deserialize(stream);
                    return sccData;
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
                return sccData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>ExternalControlAreaDeviceData</returns>
        public ExternalControlAreaDeviceData GetXMLDeserializedExternalControlDeviceData(byte[] bytes)
        {
            ExternalControlAreaDeviceData sccData = null;
            try
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExternalControlAreaDeviceData));
                    sccData = (ExternalControlAreaDeviceData)xmlSerializer.Deserialize(stream);
                    return sccData;
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
                return sccData;
            }
        }

        /// <summary>
        /// SCCData XML Serialized - 제어코드 자동 삽입
        /// </summary>
        /// <param name="sccData">SCCData 객체</param>
        /// <returns>변환된 데이터 바이트 배열</returns>
        public static byte[] GetXMLSerializedScaleData(ExternalControlAreaDeviceData sccData)
        {
            byte[] xmlBytes = null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings()
                    {
                        Indent = true
                    };

                    using (XmlWriter writer = XmlWriter.Create(ms, settings))
                    {
                        // XML namespace 제거
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        XmlSerializer xs = new XmlSerializer(typeof(ExternalControlAreaDeviceData));
                        xs.Serialize(writer, sccData, ns);

                        // 제어코드 삽입
                        //xmlBytes = EBR.GetBytesWithControlCode(ms.ToArray(), EBR.INI_SCCS_Start, EBR.INI_SCCS_End);
                        xmlBytes = ms.ToArray();
                    }
                }
                return xmlBytes;
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                return xmlBytes;
            }
        }

        #endregion XML - Serialize


    }
}
