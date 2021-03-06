using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmos.IL2CPU.Plugs;
using Cosmos.Assembler.x86;
using Cosmos.Assembler;

namespace Cosmos.Sys.Plugs.Assemblers
{
    public class GetVbeModeAssembler: AssemblerMethod
    {
        public override void AssembleNew(Cosmos.Assembler.Assembler aAssembler, object aMethodInfo)
        {
            XS.Xor(XSRegisters.EAX, XSRegisters.Registers.EAX);
            XS.Mov(XSRegisters.AX, Cosmos.Assembler.ElementReference.New("MultibootGraphicsRuntime_VbeMode"), sourceIsIndirect: true);
            XS.Push(XSRegisters.EAX);
        }
    }
}