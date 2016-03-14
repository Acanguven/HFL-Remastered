namespace LoLLauncher.Assets
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class AsyncHelpers
    {
        public static void RunSync(Func<Task> task)
        {
            ExclusiveSynchronizationContext synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(delegate (object _) {
                <>c__DisplayClass0_0.<<RunSync>b__0>d local;
                local.<>4__this = (<>c__DisplayClass0_0) this;
                local.<>t__builder = AsyncVoidMethodBuilder.Create();
                local.<>1__state = -1;
                local.<>t__builder.Start<<>c__DisplayClass0_0.<<RunSync>b__0>d>(ref local);
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(SynchronizationContext.Current);
        }

        public static T RunSync<T>(Func<Task<T>> task)
        {
            ExclusiveSynchronizationContext synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(delegate (object _) {
                <>c__DisplayClass1_0<T>.<<RunSync>b__0>d local;
                local.<>4__this = (<>c__DisplayClass1_0<T>) this;
                local.<>t__builder = AsyncVoidMethodBuilder.Create();
                local.<>1__state = -1;
                local.<>t__builder.Start<<>c__DisplayClass1_0<T>.<<RunSync>b__0>d>(ref local);
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(SynchronizationContext.Current);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            private readonly Queue<Tuple<SendOrPostCallback, object>> items = new Queue<Tuple<SendOrPostCallback, object>>();
            private readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);

            public void BeginMessageLoop()
            {
                while (!this.done)
                {
                }
            }

            public void EndMessageLoop()
            {
                this.Post(_ => this.done = true, null);
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                Queue<Tuple<SendOrPostCallback, object>> items = this.items;
                lock (items)
                {
                    this.items.Enqueue(Tuple.Create<SendOrPostCallback, object>(d, state));
                }
                this.workItemsWaiting.Set();
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public Exception InnerException { get; set; }
        }
    }
}

