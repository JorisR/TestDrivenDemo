using System;
using System.Collections.Generic;
using System.Text;

namespace EventDrivenDemo.Publisher.Interfaces
{
    public interface IPublisher
    {
        void PublishMessage(string message, string queue);
        void CreateQueue(string queue);
    }
}
