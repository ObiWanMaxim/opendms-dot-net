﻿using System;
using Newtonsoft.Json.Linq;

namespace OpenDMS.Storage.Providers.CouchDB.Transactions.Tasks.Remoting
{
    public abstract class Base
    {
        public delegate void CompletionDelegate(Base sender, ICommandReply reply);
        public delegate void ErrorDelegate(Base sender, string message, Exception exception);
        public delegate void ProgressDelegate(Base sender, OpenDMS.Networking.Http.DirectionType direction, int packetSize, decimal sendPercentComplete, decimal receivePercentComplete);
        public delegate void TimeoutDelegate(Base sender);

        public event CompletionDelegate OnComplete;
        public event ErrorDelegate OnError;
        public event ProgressDelegate OnProgress;
        public event TimeoutDelegate OnTimeout;

        protected Uri _uri = null;
        protected IDatabase _db;
        protected string _id;
        protected JObject _input;

        public ICommandReply Reply { get; protected set; }

        public abstract void Process();

        public Base(IDatabase db, string id)
        {
            _uri = UriBuilder.Build(db, id);
            _db = db;
            _id = id;
        }

        protected void TriggerOnComplete(ICommandReply reply)
        {
            if (OnComplete == null)
                throw new NotImplementedException("OnComplete must be implemented.");

            Reply = reply;
            OnComplete(this, reply);
        }

        protected void TriggerOnError(string message, Exception exception)
        {
            if (OnError == null)
                throw new NotImplementedException("OnError must be implemented.");

            OnError(this, message, exception);
        }

        protected void TriggerOnProgress(OpenDMS.Networking.Http.DirectionType direction, int packetSize, decimal sendPercentComplete, decimal receivePercentComplete)
        {
            if (OnProgress != null) OnProgress(this, direction, packetSize, sendPercentComplete, receivePercentComplete);
        }

        protected void TriggerOnTimeout()
        {
            if (OnTimeout != null) OnTimeout(this);
        }
    }
}
