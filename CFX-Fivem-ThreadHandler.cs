namespace CFX-Fivem-ThreadHandler {
    public static class ThreadHandler {
        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == 3;

        public static async Task GoToMainThread() {
            if (!IsMainThread) await BaseScript.Delay(0);
        }

        public static async Task LeaveMainThread() {
            if (IsMainThread) await Task.Run(() => { }).ConfigureAwait(false);
        }

        public static async Task CallOnMainThread(Action functionToInvoke) {
            await GoToMainThread();
            functionToInvoke();
            await LeaveMainThread();
        }

        public static async Task<T> CallOnMainThread<T>(Func<T> functionToInvoke) {
            await GoToMainThread();
            T _result = functionToInvoke();
            await LeaveMainThread();
            return _result;
        }
    }
}
