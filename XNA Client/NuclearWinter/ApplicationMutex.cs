using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NuclearWinter
{
    public class ApplicationMutex: IDisposable
    {
        Mutex mMutex;

        public bool HasHandle { get; private set; }

        public ApplicationMutex()
        {
            // Get application GUID
            string appGuid = ((GuidAttribute)Assembly.GetEntryAssembly().GetCustomAttributes( typeof(GuidAttribute), false ).GetValue(0)).Value.ToString();

            // Build mutexId from appGuid
            string mutexId = string.Format( "Global\\{{{0}}}", appGuid );

            // Based on http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
            // Prevent running multiple instances of CraftStudio, they would corrupt each other's files
            mMutex = new Mutex( false, mutexId );

            HasHandle = false;
            try
            {
                HasHandle = mMutex.WaitOne( 10, false );
            }
            catch( AbandonedMutexException )
            {
                // The mutex was abandoned in another process, it will still get acquired
                HasHandle = true;
            }
        }

        public void Dispose()
        {
            if( HasHandle )
            {
                mMutex.ReleaseMutex();
            }
        }
    }
}
