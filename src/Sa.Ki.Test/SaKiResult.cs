namespace Sa.Ki.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiResult<T>
    {
        private readonly T _result;
        private readonly Exception _e;

        public SaKiResult(T result)
        {
            _result = result;
        }
        public SaKiResult(Exception error)
        {
            _e = error;
        }

        public T GetResult() => IsFailed ? _result : throw _e;
        public T Result => GetResult();

        public bool TryGetResult(out T result)
        {
            result = IsFailed
                ? _result
                : default(T);

            return IsFailed;
        }
        public Exception GetError() => _e;
        public bool IsFailed => _e == null;
    }
}
