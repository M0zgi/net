using Client.Models;


ClientConnect client = new ClientConnect(4000, "127.0.0.1");

#region Task1 TODO раскоментировать для проверки первого задания (остальные заккоментировать)

//client.Connect(); 

#endregion

#region Task2 TODO раскоментировать для проверки второго задания (остальные заккоментировать)

//while (true)
//{
//    Console.WriteLine("1 - для получения текущего времени\n" +
//                      "2 - для получения даты\n" +
//                      "Введите цифру: ");
//    string? data = Console.ReadLine();

//    client.ConnectData(data);
//}

#endregion

#region Task3 TODO раскоментировать для проверки третьего задания (остальные заккоментировать)
//client.ConnectAsync();
#endregion

#region Task4 TODO раскоментировать для проверки четвертого задания (остальные заккоментировать)

client.ConnectAsyncData();

#endregion



// Delay.
Console.ReadKey();