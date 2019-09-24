
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.ComponentModel;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace FAN.Remoting
{
    /// <summary>
    /// Remoting服务类
    /// </summary>
    public class RemotingService
    {
        #region 变量
        private DateTime _StartTime;
        private int _Port;
        private long _ProcessCount = 0;
        private string _UriName = string.Empty;
        private EServiceStatus _Status = EServiceStatus.Stopped;
        private TcpServerChannel _TcpChannel = null;
        private List<Type> _TypeLists = new List<Type>();
        #endregion

        #region 构造函数
        public RemotingService()
        {
        }
        #endregion

        #region 属性
        public DateTime StartTime
        {
            get { return _StartTime; }
        }
        public int Port
        {
            get { return _Port; }
            set
            {
                if (_Status == EServiceStatus.Stopped)
                    _Port = value;
            }
        }
        public string UriName
        {
            get { return _UriName; }
            set
            {
                if (_Status == EServiceStatus.Stopped)
                    _UriName = value;
            }
        }
        public long ProcessCount
        {
            get { return _ProcessCount; }
        }
        public EServiceStatus Status
        {
            get { return _Status; }
        }
        public List<Type> TypeLists
        {
            get { return _TypeLists; }
            set { _TypeLists = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 注册处理类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool RegHandler(Type type)
        {
            if (type.BaseType == typeof(MarshalByRefObject))
            {
                if (!_TypeLists.Exists(new Predicate<Type>((t) =>
                {
                    if (t.Name == type.Name)
                        return true;
                    return false;
                })))
                {
                    _TypeLists.Add(type);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        public void StartService()
        {
            if (_Status != EServiceStatus.Stopped)
            {
                return;
            }
            if (_TypeLists.Count == 0)
            {
                return;
            }
            _Status = EServiceStatus.Starting;
            if (_TcpChannel == null)
            {
                //_TcpChannel = new TcpChannel(_Port);
                _TcpChannel = new TcpServerChannel(Guid.NewGuid().ToString(),_Port);
            }
            else
            {
                try
                {
                    _TcpChannel.StopListening(null);
                    ChannelServices.UnregisterChannel(_TcpChannel);
                }
                catch (Exception ex)
                {
                    _Status = EServiceStatus.Stopped;
                    throw ex;
                }
                Thread.Sleep(2000);
                //_TcpChannel = new TcpChannel(_Port);
                _TcpChannel = new TcpServerChannel(Guid.NewGuid().ToString(), _Port);
            }
            try
            {
                ChannelServices.RegisterChannel(_TcpChannel, false);

            }
            catch (Exception ex)
            {
                _TcpChannel = null;
                _Status = EServiceStatus.Stopped;
                throw ex;
            }
            foreach (Type type in _TypeLists)
            {
                RemotingConfiguration.RegisterWellKnownServiceType(type, _UriName + "/" + type.Name, WellKnownObjectMode.Singleton);
            }
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.On;
            RemotingConfiguration.CustomErrorsEnabled(false);
            _Status = EServiceStatus.Started;
            _StartTime = DateTime.Now;
        }
        /// <summary>
        /// 关闭服务
        /// </summary>
        public void StopService()
        {
            if (_Status != EServiceStatus.Started)
            {
                return;
            }
            _Status = EServiceStatus.Stopping;
            try
            {
                if (_TcpChannel != null)
                {
                    _TcpChannel.StopListening(null);
                    ChannelServices.UnregisterChannel(_TcpChannel);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            _TcpChannel = null;
            _Status = EServiceStatus.Stopped;
        }

        public void IncrementProcessCount()
        {
            _ProcessCount++;
        }
        #endregion

        public enum EServiceStatus
        {
            /// <summary>
            /// 启动中
            /// </summary>
            [Description("启动中")]
            Starting = 0,
            /// <summary>
            /// 已启动
            /// </summary>
            [Description("已启动")]
            Started = 1,
            /// <summary>
            /// 停止中
            /// </summary>
            [Description("停止中")]
            Stopping = 2,
            /// <summary>
            /// 已停止
            /// </summary>
            [Description("已停止")]
            Stopped = 3
        }
    }
}
