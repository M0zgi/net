using ServerConsole.Entities;

SocketServer server = new SocketServer(4041, "127.0.0.1");

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


server.StartAsync();

Console.WriteLine("IP: 127.0.0.1 \nPort: 4041");
// Delay.
Console.ReadKey();