namespace CFX-Fivem-ThreadHandler {
    public sealed class ThreadHandler {
        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == 3;

        public static async Task ConditionalJumpToMainThread() {
            if (!IsMainThread)
                await BaseScript.Delay(0);
        }

        public static void ThrowIfNotMainThread() {
            if (!IsMainThread) throw new Exception("!!! NOT ON MAIN THREAD");
        }

        public static async Task CallOnMainThread(string name, Action functionToInvoke) {
            if (IsMainThread) {
                functionToInvoke();
                return;
            }

            await Task.Run(async () => {
                await BaseScript.Delay(0);
                ThrowIfNotMainThread();
                functionToInvoke();
                ThrowIfNotMainThread();
            })
            .ContinueWith(t => {
                if (t.IsFaulted)
                    throw t.Exception;
            }).ConfigureAwait(false);
        }

        public static async Task CallOnMainThread(string name, Func<Task> functionToInvoke) {
            if (IsMainThread) {
                await functionToInvoke().ConfigureAwait(false);
                return;
            }

            await Task.Run(async () => {
                await BaseScript.Delay(0);
                ThrowIfNotMainThread();
                await functionToInvoke().ConfigureAwait(false);
                ThrowIfNotMainThread();
            })
            .ContinueWith(t => {
                if (t.IsFaulted)
                    throw t.Exception;
            }).ConfigureAwait(false);
        }

        public static async Task<T> CallOnMainThread<T>(string name, Func<T> functionToInvoke) {
            if (IsMainThread) {
                return functionToInvoke();
            }

            return await Task.Run(async () => {
                await BaseScript.Delay(0);
                ThrowIfNotMainThread();
                var temp = functionToInvoke();
                ThrowIfNotMainThread();
                return temp;
            })
            .ContinueWith(t => {
                if (t.IsFaulted)
                    throw t.Exception;
                return t.Result;
            }).ConfigureAwait(false);
        }
    }
}
