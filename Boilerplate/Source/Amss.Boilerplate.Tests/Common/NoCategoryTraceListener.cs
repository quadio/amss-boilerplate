namespace Amss.Boilerplate.Tests.Common
{
    using System.Diagnostics;
    using System.Linq;

    internal class NoCategoryTraceListener : TextWriterTraceListener
    {
        #region Constants and Fields

        private readonly TraceListener traceListener;

        #endregion

        #region Constructors and Destructors

        public NoCategoryTraceListener(TraceListener traceListener)
        {
            this.traceListener = traceListener;
        }

        #endregion

        #region Public Methods

        public static void Install()
        {
            var nunitListener = Debug.Listeners.Cast<TraceListener>().FirstOrDefault(tl => tl.Name == "NUnit");
            if (nunitListener != null)
            {
                if (!Debug.Listeners.OfType<NoCategoryTraceListener>().Any())
                {
                    var noCategoryTraceListener = new NoCategoryTraceListener(nunitListener);
                    Debug.Listeners.Add(noCategoryTraceListener);
                }

                Debug.Listeners.Remove(nunitListener);
            }
        }

        public override void Write(string message, string category)
        {
            this.Write(message);
        }

        public override void Write(string message)
        {
            this.traceListener.Write(message);
        }

        public override void WriteLine(string message, string category)
        {
            this.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            this.traceListener.WriteLine(message);
        }

        #endregion
    }
}