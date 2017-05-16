using System.Runtime.Remoting.Messaging;

namespace GSXF.Core
{
    public class ContextFactory
    {
        public static GSXFContext CurrentContext()
        {
            GSXFContext _Context = CallContext.GetData("GSXFContext") as GSXFContext;
            if (_Context == null)
            {
                _Context = new GSXFContext();
                CallContext.SetData("GSXFContext", _Context);
            }
            return _Context;
        }
    }
}
