using static Interfaces.Enums.Common;

namespace Interfaces.ResultModel
{
    public class Result<T> where T: class
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public ResultType Type { get; set; }
        public bool IsSuccess { get; set; }

        public Result()
        {
            Data = default(T);
        }

        public Result(T Data)
            : this(true, ResultType.None, string.Empty, Data)
        {
        }

        public Result(bool IsSuccess, ResultType ResultType, string Message)
            : this(IsSuccess, ResultType, Message, default(T))
        {
        }

        public Result(bool IsSuccess, ResultType ResultType, string Message, T Data)
        {
            this.IsSuccess = IsSuccess;
            this.Type = ResultType;
            this.Message = Message;
            this.Data = Data;
        }
    }
}
