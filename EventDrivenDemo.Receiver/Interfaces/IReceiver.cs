using System;
using System.Collections.Generic;
using System.Text;

namespace EventDrivenDemo.Receiver.Interfaces
{
    public  interface IReceiver
    {
        void Receive(string queue);
        void StopReceiving();
    }
}
