using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Enum;

namespace Lib
{
    /// <summary>
    /// объект который отсылается в момент запроса
    ///
    /// Протокол построения запроса к серверу
    /// 1. Что нужно сделать
    /// 2 С чем это делать
    /// </summary>
    [Serializable]
    public class Request
    {
        /// <summary>
        /// Команда которую должен выполнить сервер
        /// </summary>
        //public RequestCommands Command;

        public RequestCommands Command { get; set; }

        /// <summary>
        /// Данные для работы сервера
        /// </summary>
        public object? Body;
    }
}
