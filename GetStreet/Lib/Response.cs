using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Enum;

namespace Lib
{
    /// <summary>
    /// объект ответа
    /// </summary>
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
