using Lib.Data;

using var dbContext = new ApplicationDbContext();

dbContext.Database.EnsureCreated();

try
{
    //if (dbContext == null)
    //{
    //    dbContext = new ApplicationDbContext();
    //}

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