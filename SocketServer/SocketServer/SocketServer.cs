﻿using System;
using System.Net;
using System.Timers;
using FTServer.Log;
using FTServer.Network;
using FTServer.ClientInstance;

namespace FTServer
{
    public abstract class SocketServer
    {
        private const float TickConsoleClear = 60 * 1000;//5 * 60 * 1000; // 30 minute

        protected Core mListener;
        private int Port;
        private Protocol _Protocol;
        private Timer consoleClear;

        protected void StartListen(int port, Protocol protocol)
        {
            // CompositeResolver is singleton helper for use custom resolver.
            // Ofcourse you can also make custom resolver.
            MessagePack.Resolvers.CompositeResolver.RegisterAndSetAsDefault(
                // use generated resolver first, and combine many other generated/custom resolvers
                MessagePack.Resolvers.GeneratedResolver.Instance,

                // finally, use builtin/primitive resolver(don't use StandardResolver, it includes dynamic generation)
                MessagePack.Resolvers.BuiltinResolver.Instance,
                MessagePack.Resolvers.AttributeFormatterResolver.Instance,
                MessagePack.Resolvers.PrimitiveObjectResolver.Instance
            );

            Port = port;
            _Protocol = protocol;
            Listen(protocol);

            string serverInfo = string.Format("Socket Server Start. Base Info => {0}     Listen Port :  {1}{2}     Network Protocol : {3}"
                , "\n" ,port,"\n",protocol.ToString());
            Printer.WriteLine(serverInfo);

            //定時把 console clear 掉，嘗試解決GamingServer 過一段時間後會死在 Printer.WriteLine 的問題
            //BeginConsoleClearAsync();
        }

        /// <summary>
        /// 開始等待封包傳入
        /// </summary>
        private void BeginConsoleClearAsync()
        {
            consoleClear = new Timer(TickConsoleClear);
            consoleClear.Elapsed += (sender, eventArg) =>
            {
                Console.Clear();
            };
            consoleClear.Start();
        }

        private async void Listen(Protocol protocol)
        {
            switch (protocol)
            {
                case Protocol.TCP:
                    mListener = new Tcp(this, new IPEndPoint(IPAddress.Any, Port));
                    break;
                case Protocol.UDP:
                    mListener = new Udp(this, new IPEndPoint(IPAddress.Any, Port));
                    break;
                case Protocol.WebSocket:
                    mListener = new WebSocket(this, Port);
                    break;
                case Protocol.RUDP:
                    mListener = new RUdp(this, new IPEndPoint(IPAddress.Any, Port));
                    break;
                default:
                    Printer.WriteLine("Not Support this Protocol.");
                    break;
            }

            await mListener.StartListen();
        }

        public void CloseClient(IPEndPoint iPEndPoint)
        {
            mListener.DisConnect(iPEndPoint);
        }

        public abstract ClientNode GetPeer(Core core, IPEndPoint iPEndPoint, SocketServer application);
    }
}