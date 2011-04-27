using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace testConsole_dotNET
{
   class Test
   {
      internal void Go()
      {
         testEventThreads();
      }

      event SendMessage OnSendMessage;

      delegate void SendMessage(string msg);

      private void testEventThreads()
      {
         Console.WriteLine("testEventThreads()");

         outputThreadId();
         OnSendMessage += new SendMessage(Test_OnSendMessage);

         OnSendMessage("Testing event firing");
      }

      void Test_OnSendMessage(string msg)
      {
         outputThreadId();
      }

      private void outputThreadId()
      {
         Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
      }
   }
}
