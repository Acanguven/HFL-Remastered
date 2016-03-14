namespace RitoBot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle<T>(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (rng == null)
            {
                throw new ArgumentNullException("rng");
            }
            return source.ShuffleIterator<T>(rng);
        }

        [IteratorStateMachine(typeof(<ShuffleIterator>d__2))]
        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
        {
            return new <ShuffleIterator>d__2<T>(-2) { <>3__source = source, <>3__rng = rng };
        }

        [CompilerGenerated]
        private sealed class <ShuffleIterator>d__2<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private T <>2__current;
            public Random <>3__rng;
            public IEnumerable<T> <>3__source;
            private int <>l__initialThreadId;
            private List<T> <buffer>5__1;
            private int <i>5__3;
            private int <j>5__2;
            private Random rng;
            private IEnumerable<T> source;

            [DebuggerHidden]
            public <ShuffleIterator>d__2(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Environment.CurrentManagedThreadId;
            }

            private bool MoveNext()
            {
                int num = this.<>1__state;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        return false;
                    }
                    this.<>1__state = -1;
                    this.<buffer>5__1[this.<j>5__2] = this.<buffer>5__1[this.<i>5__3];
                    int num2 = this.<i>5__3;
                    this.<i>5__3 = num2 + 1;
                }
                else
                {
                    this.<>1__state = -1;
                    this.<buffer>5__1 = this.source.ToList<T>();
                    this.<i>5__3 = 0;
                }
                if (this.<i>5__3 >= this.<buffer>5__1.Count)
                {
                    return false;
                }
                this.<j>5__2 = this.rng.Next(this.<i>5__3, this.<buffer>5__1.Count);
                this.<>2__current = this.<buffer>5__1[this.<j>5__2];
                this.<>1__state = 1;
                return true;
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                EnumerableExtensions.<ShuffleIterator>d__2<T> d__;
                if ((this.<>1__state == -2) && (this.<>l__initialThreadId == Environment.CurrentManagedThreadId))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtensions.<ShuffleIterator>d__2<T>) this;
                }
                else
                {
                    d__ = new EnumerableExtensions.<ShuffleIterator>d__2<T>(0);
                }
                d__.source = this.<>3__source;
                d__.rng = this.<>3__rng;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            T IEnumerator<T>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

