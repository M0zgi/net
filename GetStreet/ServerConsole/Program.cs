﻿using ServerConsole.Entities;

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

Lib.Data.ApplicationDbContext dbContext = null;

try
{
    if (dbContext == null)
    {
        dbContext = new Lib.Data.ApplicationDbContext();
    }

    var Data = new Lib.DemoData.LoadInfo();

    var zip1 = Data.LoadZip1();
    dbContext.Add(zip1);

    var street = Data.LoadStreet1();
    dbContext.Add(street);

    var street2 = Data.LoadStreet2();
    dbContext.Add(street2);

    var street3 = Data.LoadStreet3();
    dbContext.Add(street3);

    var street4 = Data.LoadStreet4();
    dbContext.Add(street4);

    var zip2 = Data.LoadZip2();
    dbContext.Add(zip2);

    var street5 = Data.LoadStreet5();
    dbContext.Add(street5);

    var street6 = Data.LoadStreet6();
    dbContext.Add(street6);

    var street7 = Data.LoadStreet7();
    dbContext.Add(street7);

    var zip3 = Data.LoadZip3();
    dbContext.Add(zip3);


    dbContext.SaveChanges();
}
catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
{
    Console.WriteLine(ex.Message, "Create Error DB");

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message, "Create Error Other");
}

server.StartAsync();

Console.WriteLine("IP: 127.0.0.1 \nPort: 4041");
// Delay.
Console.ReadKey();