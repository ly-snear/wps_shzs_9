using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    interface IAVPacketProvider
    {
        void Start();
        void Stop();
        void Dispose();
        event EventHandler<NewAVPacketEventArgs> NewAVPacket;
    }
}
