﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Receiver
{
    public interface ISimpleConnectReceiver
    {
        void onMessage(String sSessionId, byte[] bData);
    }
}
