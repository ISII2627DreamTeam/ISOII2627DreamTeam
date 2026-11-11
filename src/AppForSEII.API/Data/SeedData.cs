namespace AppForSEII.API.Data {
    public class SeedData {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

           try {
                seedImpresa3D(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar las impresoras 3D en la base de datos.");
            }
            try {
                seedModelos3D(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar los modelos 3D en la base de datos.");
            }
            try {
                seedLicenciasModelo3D(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar las licencias de los modelos 3D en la base de datos.");
            }

 

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {

            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                Cliente user = new ("1", "Elena", "Navarro Martínez", "elena@uclm.es","Calle de la Universidad, 1, 13071 Ciudad Real, España");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }


            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                Cliente user = new Cliente("3", "Peter", "Jackson", "peter@uclm.es","Calle de la Universidad, 1, 13071 Ciudad Real, España");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }

        }

        public static void seedImpresa3D(ApplicationDbContext dbContext) {
            if (!dbContext.Impresora3Ds.Any()) {
                var impresoras = new List<Impresora3D> {
                    new Impresora3D { Nombre = "Impresora 3D A", Modelo = "Modelo A" , Descripcion = "Impresora 3D de alta calidad para prototipos y producción en serie.", Tipo = TipoImpresora.Filamento, PrecioKilovatioHora = 0.15m, PrecioReserva = 10.00m },
                    new Impresora3D { Nombre = "Impresora 3D B", Modelo = "Modelo B", Descripcion = "Impresora 3D de resina para detalles finos y acabados suaves.", Tipo = TipoImpresora.Resina, PrecioKilovatioHora = 0.20m, PrecioReserva = 15.00m },
                    new Impresora3D { Nombre = "Impresora 3D C", Modelo = "Modelo C", Descripcion = "Impresora 3D de bajo costo para uso educativo.", Tipo = TipoImpresora.Filamento, PrecioKilovatioHora = 0.10m, PrecioReserva = 5.00m }
                };

                dbContext.Impresora3Ds.AddRange(impresoras);
                dbContext.SaveChanges();
            }
        }

        public static void seedModelos3D(ApplicationDbContext dbContext) {
            if (!dbContext.Modelos3D.Any()) {
                var modelos3D = new List<Modelo3D> {
                    new Modelo3D { Nombre = "Modelo 3D A", Categoría = "Juguetes", Formato = FormatoModelo3D.STL, Precio = 5.99m },
                    new Modelo3D { Nombre = "Modelo 3D B", Categoría = "Oficina", Formato = FormatoModelo3D.OBJ, Precio = 2.50m },
                    new Modelo3D { Nombre = "Modelo 3D C", Categoría = "Mecánica", Formato = FormatoModelo3D.TresMF, Precio = 12.00m }
                };

                dbContext.Modelos3D.AddRange(modelos3D);
                dbContext.SaveChanges();
            }
        }

        public static void seedLicenciasModelo3D(ApplicationDbContext dbContext) {
            if (!dbContext.LicenciasModelo3D.Any()) {
                
                var modelos = dbContext.Modelos3D.Select(a=> a).ToListAsync();
              

                if (modelos.Result.Count() >= 3) {
                    var licencias = new List<LicenciaModelo3D> {
                        new LicenciaModelo3D("Licencia 3D A", DateTime.Now.AddYears(1), modelos.Result[0]),
                        new LicenciaModelo3D("Licencia 3D B", DateTime.Now.AddYears(3), modelos.Result[1]),
                        new LicenciaModelo3D("Licencia 3D C", DateTime.Now.AddYears(5), modelos.Result[2])
                    };

                    dbContext.LicenciasModelo3D.AddRange(licencias);
                    dbContext.SaveChanges();
                }
            }
        }





    }
}