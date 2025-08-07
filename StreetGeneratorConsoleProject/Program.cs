using System;
using System.Linq;
using ClassLibraryProject.Entities;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Extensions;
using RepositoryLibraryProject.Data;
using Microsoft.EntityFrameworkCore;

namespace StreetGeneratorConsoleProject
{
    public class Program
    {

        public const string CONSTRING = "Server=serverdevdemo.database.windows.net,1433;Database=devdemobbdd;User Id= admsql;Password= P@ssw0rd;";
        static void Main(string[] args)
        {
            Console.WriteLine("=== Gestor de Calles ===\n");
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(GetConnectionString())
                .Options;

            using var context = new AppDbContext(options);
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nOpciones:");
                Console.WriteLine("1. Insertar nueva calle");
                Console.WriteLine("2. Listar calles");
                Console.WriteLine("3. Buscar calle por nombre");
                Console.WriteLine("0. Salir");
                Console.Write("Selecciona una opción: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        InsertStreet(context);
                        break;
                    case "2":
                        ListStreets(context);
                        break;
                    case "3":
                        SearchStreet(context);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            Console.WriteLine("\n¡Hasta pronto!");
        }

        static string GetConnectionString()
        {
            return CONSTRING;
        }

        static void InsertStreet(AppDbContext context)
        {
            Console.Write("Nombre base de la calle: ");
            var baseName = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(baseName) || baseName.Length < 2 || baseName.Length > 255)
            {
                Console.WriteLine("Nombre no válido.");
                return;
            }
            Console.WriteLine("Tipo de calle:");
            foreach (var value in Enum.GetValues(typeof(StreetTypeEnum)))
            {
                Console.WriteLine($"{(int)value} - {value}");
            }
            Console.Write("Tipo: ");
            if (!int.TryParse(Console.ReadLine(), out int typeInt) || !Enum.IsDefined(typeof(StreetTypeEnum), typeInt))
            {
                Console.WriteLine("Tipo no válido.");
                return;
            }
            var streetType = (StreetTypeEnum)typeInt;
            var street = new Street(baseName, streetType);
            if (context.Streets.Any(s => s.Name == street.Name))
            {
                Console.WriteLine($"Ya existe una calle con ese nombre compuesto: {street.Name}");
                return;
            }
            if (context.Streets.Any(s => s.Code == street.Code))
            {
                Console.WriteLine($"Ya existe una calle con ese código: {street.Code}");
                return;
            }
            context.Streets.Add(street);
            context.SaveChanges();
            Console.WriteLine($"Calle insertada correctamente: {street.Name} (Código: {street.Code})");
        }

        static void ListStreets(AppDbContext context)
        {
            var streets = context.Streets.OrderBy(s => s.Name).ToList();
            if (!streets.Any())
            {
                Console.WriteLine("No hay calles registradas.");
                return;
            }
            Console.WriteLine("\nListado de calles:");
            foreach (var s in streets)
            {
                Console.WriteLine($"- {s.Name} (Código: {s.Code})");
            }
        }

        static void SearchStreet(AppDbContext context)
        {
            Console.Write("Introduce parte del nombre a buscar: ");
            var query = Console.ReadLine()?.Trim() ?? string.Empty;
            var results = context.Streets
                .Where(s => s.Name.Contains(query))
                .OrderBy(s => s.Name)
                .ToList();
            if (!results.Any())
            {
                Console.WriteLine("No se encontraron calles con ese criterio.");
                return;
            }
            Console.WriteLine("\nResultados:");
            foreach (var s in results)
            {
                Console.WriteLine($"- {s.Name} (Código: {s.Code})");
            }
        }
    }
}
