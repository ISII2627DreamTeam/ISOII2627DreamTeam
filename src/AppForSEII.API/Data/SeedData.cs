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
                seedImpresoras3D(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar las impresoras 3D en la base de datos.");
            }

            try {
                seedReservasImpresora(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar las reservas de impresoras 3D en la base de datos.");
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

            try {
                seedAccesorios(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar los accesorios en la base de datos.");
            }

            try {
                seedComprasAccesorios(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar las compras de accesorios en la base de datos.");
            }

            try {
                seedMateriales(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar los materiales en la base de datos.");
            }
            try {
                seedPiezas3D(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Ha ocurrido un error al sembrar las piezas 3D en la base de datos.");
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

        public static void seedImpresoras3D(ApplicationDbContext dbContext) {
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

        public static void seedReservasImpresora(ApplicationDbContext dbContext) {
            if (!dbContext.ReservaImpresoras.Any()) {

                var impresoras = dbContext.Impresora3Ds.Select(i => i).ToListAsync();

                if (impresoras.Result.Count >= 3) {
                    var reserva = new ReservaImpresora(DateTime.Now, "Peter", "Jackson", "Calle de la Universidad, 1, 13071 Ciudad Real, España", 0m, MetodoPago.PayPal);

                    var linea1 = new LineaReserva(TiempoReserva.DosHoras, impresoras.Result[0].PrecioReserva * 2, impresoras.Result[0].Id, 0);
                    linea1.Impresora = impresoras.Result[0];
                    linea1.ReservaImpresora = reserva;

                    var linea2 = new LineaReserva(TiempoReserva.UnDia, impresoras.Result[1].PrecioReserva * 24, impresoras.Result[1].Id, 1);
                    linea2.Impresora = impresoras.Result[1];
                    linea2.ReservaImpresora = reserva;

                    reserva.LineasReserva.Add(linea1);
                    reserva.LineasReserva.Add(linea2);
                    reserva.PrecioTotal = linea1.PrecioSubtotal + linea2.PrecioSubtotal;
                    var reservas = new List<ReservaImpresora> { reserva };

                    dbContext.LineaReservas.AddRange(linea1, linea2);
                    dbContext.ReservaImpresoras.AddRange(reservas);
                    dbContext.SaveChanges();
                }
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
                
                var modelos = dbContext.Modelos3D.Select(m => m).ToListAsync();

                if (modelos.Result.Count >= 3) {
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

        public static void seedAccesorios(ApplicationDbContext dbContext) {
            if (!dbContext.Accesorios.Any()) {
                var accesorios = new List<Accesorio> {
                    new Accesorio("Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m),
                    new Accesorio("Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m),
                    new Accesorio("Alicates", CategoriaAccesorio.Herramientas, "Universal", 30, 8.00m)
                };

                dbContext.Accesorios.AddRange(accesorios);
                dbContext.SaveChanges();
            }
        }

        public static void seedComprasAccesorios(ApplicationDbContext dbContext) {
            if (!dbContext.ComprasAccesorios.Any()) {
                
                var accesorios = dbContext.Accesorios.Select(a => a).ToListAsync();

                if (accesorios.Result.Count >= 3) {
                    var compra = new CompraAccesorios("Jose Miguel", "Martinez", "Calle Falsa 123", "777888999", DateTime.Now, MetodoPago.TarjetaCredito, 0m);
                    var linea = new LineaCompraAccesorio(accesorios.Result[0], compra, 2, accesorios.Result[0].Precio);
                    
                    compra.LineasCompra.Add(linea);
                    compra.PrecioTotal = linea.Subtotal;

                    dbContext.ComprasAccesorios.Add(compra);
                    dbContext.SaveChanges();
                }
            }
        }

        public static void seedMateriales(ApplicationDbContext dbContext) {
            if (!dbContext.Materiales.Any()) {
                var materiales = new List<Material> {
                    new Material("PLA Blanco", 0.05m, 10.0m),
                    new Material("PETG Negro", 0.08m, 50.0m),
                    new Material("ABS Rojo", 0.06m, 50.0m)
                };

                dbContext.Materiales.Add(materiales[0]);
                dbContext.Materiales.Add(materiales[1]);
                dbContext.Materiales.Add(materiales[2]);
                dbContext.SaveChanges();
            }
        }

        public static void seedPiezas3D(ApplicationDbContext dbContext) {
            if (!dbContext.Piezas3D.Any()) {
                var mats = dbContext.Materiales.Select(p => p).ToList();

                if (mats.Count >= 3) {
                    var pieza1 = new Pieza3D("Engranaje Helicoidal", 25.5m, (CategoriaPieza)0);
                    pieza1.MaterialesValidos.Add(mats[0]);
                    pieza1.MaterialesValidos.Add(mats[1]);

                    var pieza2 = new Pieza3D("Soporte Pared", 50.0m, (CategoriaPieza)1);
                    pieza2.MaterialesValidos.Add(mats[1]);
                    pieza2.MaterialesValidos.Add(mats[2]);

                    var pieza3 = new Pieza3D("Figura articulada", 15.0m, (CategoriaPieza)2);
                    pieza3.MaterialesValidos.Add(mats[0]);

                    var piezas = new List<Pieza3D> { pieza1, pieza2, pieza3 };

                    dbContext.Piezas3D.AddRange(piezas);
                    dbContext.SaveChanges();
                }
            }
        }





    }
}