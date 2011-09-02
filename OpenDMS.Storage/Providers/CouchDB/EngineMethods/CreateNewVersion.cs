﻿using System;
using System.Collections.Generic;

namespace OpenDMS.Storage.Providers.CouchDB.EngineMethods
{
    public class CreateNewVersion : Base
    {
        private CreateVersionArgs _args;

        public CreateNewVersion(EngineRequest request, CreateVersionArgs args)
            : base(request)
        {
            _args = args;
        }

        public override void Execute()
        {
            Transactions.Transaction t;
            Transactions.Processes.CreateNewVersion process;

            process = new Transactions.Processes.CreateNewVersion(_request.Database, _args,
                _request.RequestingPartyType, _request.Session, _request.Database.Server.Timeout,
                _request.Database.Server.Timeout, _request.Database.Server.BufferSize, _request.Database.Server.BufferSize);
            t = new Transactions.Transaction(process);

            AttachSubscriber(process, _request.OnActionChanged);
            AttachSubscriber(process, _request.OnAuthorizationDenied);
            AttachSubscriber(process, _request.OnComplete);
            AttachSubscriber(process, _request.OnError);
            AttachSubscriber(process, _request.OnProgress);
            AttachSubscriber(process, _request.OnTimeout);

            t.Execute();
        }
    }
}
