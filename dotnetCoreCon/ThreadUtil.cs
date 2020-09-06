using System.Threading;

static class ThreadUtil
{
    public static void DumpThreadId(string location)
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;

        $"{location}: Thread ID = {threadId}".Dump();
    }
}