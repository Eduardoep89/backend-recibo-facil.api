using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;



public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ReciboFacilContexto>
{
 public ReciboFacilContexto CreateDbContext(string[] args)
{
    var optionsBuilder = new DbContextOptionsBuilder<ReciboFacilContexto>();
    optionsBuilder.UseSqlServer("Server=DESKTOP-39BNEP0\\SQLEXPRESS;Database=ReciboFacil;Trusted_Connection=True;TrustServerCertificate=True;");

    return new ReciboFacilContexto(optionsBuilder.Options);
}
}