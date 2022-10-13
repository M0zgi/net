using System.Text;
using Server.Models;


SocketServer server = new SocketServer(4000, "127.0.0.1");

Console.WriteLine("Версия Windows: {0}",
    Environment.OSVersion);
Console.WriteLine("64 Bit операционная система? : {0}",
    Environment.Is64BitOperatingSystem ? "Да" : "Нет");
Console.WriteLine("Имя компьютера : {0}",
    Environment.MachineName);
Console.WriteLine("Число процессоров : {0}",
    Environment.ProcessorCount);
Console.WriteLine("Системная папка : {0}",
    Environment.SystemDirectory);
Console.WriteLine("Логические диски : {0}",
    String.Join(", ", Environment.GetLogicalDrives())
        .TrimEnd(',', ' ')
        .Replace("\\", String.Empty));
Console.WriteLine("\n\n");

#region Task1 TODO раскоментировать для проверки первого задания (остальные заккоментировать)

//server.Start();

#endregion

#region Task2 TODO раскоментировать для проверки второго задания (остальные заккоментировать)

//server.StartData();

#endregion

#region Task3 TODO раскоментировать для проверки третьего задания (остальные заккоментировать)

//server.StartAsync();

#endregion

#region Task4 TODO раскоментировать для проверки четвертого задания (остальные заккоментировать)

server.StartAsyncData();

#endregion

// Delay.
Console.ReadKey();