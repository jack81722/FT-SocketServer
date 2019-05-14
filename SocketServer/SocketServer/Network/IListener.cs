﻿using System.Net;
using System.Threading.Tasks;

namespace FTServer.Network
{
    public interface IListener
    {
        Task<ReceiveResult> ReceiveAsync();
    }

    public struct ReceiveResult
    {
        public bool IsOk;
        public IPEndPoint RemoteEndPoint;
        public ReceiveResult(IPEndPoint iPEndPoint)
        {
            IsOk = true;
            RemoteEndPoint = iPEndPoint;
        }
    }
}
