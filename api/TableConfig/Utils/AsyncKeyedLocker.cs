/*
* ==============================================================================
*
* FileName: AsyncKeyedLocker.cs
* Created: 2026/1/9 9:08:20
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace TableConfig.Utils
{
    /// <summary>
    /// 异步锁
    /// </summary>
    public class AsyncKeyedLocker
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public static async Task<IDisposable> LockAsync(string key)
        {
            var semaphore = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            return new Releaser(key, semaphore, _locks);
        }

        public static int Count()
        {
            return _locks.Count;
        }

        private class Releaser : IDisposable
        {
            private readonly string _key;
            private readonly SemaphoreSlim _semaphore;
            private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks;

            public Releaser(string key, SemaphoreSlim semaphore, ConcurrentDictionary<string, SemaphoreSlim> locks)
            {
                _key = key;
                _semaphore = semaphore;
                _locks = locks;
            }

            public void Dispose()
            {
                _semaphore.Release();

                // 可选: 当无等待线程时，移除释放内存
                if (_semaphore.CurrentCount == 1)
                {
                    _locks.TryRemove(_key, out _);
                }
            }
        }
    }
}
