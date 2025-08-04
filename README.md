ejecute la migracion para modulo catalogo(aqui no se precisa el context de catalogo, porque en su momento solo existia un contexto) - se termina la configuracion para la migracion de ef, en Bootstrapper/api instalar Microsoft.entityframeworkcore.design 8.0.4
luego setear como Proyecto por defecto Bootstrapper/api y en package console situarte en catalogo y ejecutar el commando
Add-Migration InitialCreate -OutputDir Data/Migrations -Project Catalogo -StartupProject Api
luego Update-Database

ejecute la migracion para modulo carrito: primero agarro boostrapper/Api y lo pongo como set as startup project, luego abro package manager console y en default project selecciono Module/Carrito/Carrito , finalmente ejecuto esta linea de codigo:  Add-Migration InitialCreate -OutputDir Data/Migrations -Project Carrito -StartupProject Api -Context BasketDbContext

ahora levanto el conteneder docker de bd y ejecuto el comando en el package manager Update-Database -Context BasketDbContext
