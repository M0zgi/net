using Lib.Enum;

namespace Lib
{
    [Serializable]
    public class Response
    {
        /// <summary>
        /// Данные результат обработки сервера
        /// </summary>
        public ResponseStatus Status;
        public string? StatusText = "";

        /// <summary>
        /// Данные ответа сервера
        /// </summary>
        public object Body;
    }
}
