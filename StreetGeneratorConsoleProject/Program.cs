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
        static void WriteTitle(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n{text}");
            Console.ResetColor();
        }

        static void WriteMenuOption(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static void WriteSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static void WriteWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static void WriteError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static void WriteSeparator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 40));
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            WriteTitle("=== Gestor de Calles y Distritos ===");
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(GetConnectionString())
                .Options;

            using var context = new AppDbContext(options);
            bool exit = false;
            while (!exit)
            {
                WriteSeparator();
                WriteMenuOption("¿Qué deseas gestionar?");
                WriteMenuOption("1. Calles");
                WriteMenuOption("2. Distritos");
                WriteMenuOption("0. Salir");
                Console.Write("Selecciona una opción: ");
                var mainInput = Console.ReadLine();
                switch (mainInput)
                {
                    case "1":
                        ManageStreets(context);
                        break;
                    case "2":
                        ManageDistricts(context);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        WriteError("Opción no válida.");
                        break;
                }
            }
            WriteTitle("¡Hasta pronto!");
        }

        static void ManageStreets(AppDbContext context)
        {
            bool back = false;
            while (!back)
            {
                WriteSeparator();
                WriteTitle("Opciones de gestión de calles:");
                WriteMenuOption("1. Insertar nueva calle");
                WriteMenuOption("2. Listar calles");
                WriteMenuOption("3. Buscar calle por nombre");
                WriteMenuOption("0. Volver");
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
                        back = true;
                        break;
                    default:
                        WriteError("Opción no válida.");
                        break;
                }
            }
        }

        static void ManageDistricts(AppDbContext context)
        {
            bool back = false;
            while (!back)
            {
                WriteSeparator();
                WriteTitle("Opciones de gestión de distritos:");
                WriteMenuOption("1. Insertar nuevo distrito");
                WriteMenuOption("2. Listar distritos");
                WriteMenuOption("0. Volver");
                Console.Write("Selecciona una opción: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        InsertDistrict(context);
                        break;
                    case "2":
                        ListDistricts(context);
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        WriteError("Opción no válida.");
                        break;
                }
            }
        }

        static string GetConnectionString()
        {
            return CONSTRING;
        }

        static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var c in normalized)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        static void InsertStreet(AppDbContext context)
        {
            WriteSeparator();
            Console.Write("Nombre base de la calle: ");
            var baseName = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(baseName) || baseName.Length < 2 || baseName.Length > 255)
            {
                WriteError("Nombre no válido.");
                return;
            }
            WriteMenuOption("Tipo de calle:");
            foreach (var value in Enum.GetValues(typeof(StreetTypeEnum)))
            {
                WriteMenuOption($"{(int)value} - {value}");
            }
            Console.Write("Tipo: ");
            if (!int.TryParse(Console.ReadLine(), out int typeInt) || !Enum.IsDefined(typeof(StreetTypeEnum), typeInt))
            {
                WriteError("Tipo no válido.");
                return;
            }
            var streetType = (StreetTypeEnum)typeInt;

            // Selección de distritos
            var districts = context.Districts.OrderBy(d => d.Name).ToList();
            if (!districts.Any())
            {
                WriteError("No hay distritos registrados. Inserta al menos uno antes de crear calles.");
                return;
            }
            WriteTitle("Selecciona el/los distrito(s) para la calle:");
            for (int i = 0; i < districts.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{i + 1}. {districts[i].Name} (Código postal: {districts[i].Zipcode})");
                Console.ResetColor();
            }
            int[] selectedIndexes;
            if (streetType == StreetTypeEnum.Avenida)
            {
                Console.Write("Introduce los números de los distritos separados por coma (ej: 1,3): ");
                var input = Console.ReadLine();
                selectedIndexes = input?.Split(',').Select(s => int.TryParse(s.Trim(), out int idx) ? idx - 1 : -1).Where(idx => idx >= 0 && idx < districts.Count).ToArray() ?? Array.Empty<int>();
                if (selectedIndexes.Length == 0)
                {
                    WriteError("No se seleccionó ningún distrito válido.");
                    return;
                }
            }
            else
            {
                Console.Write("Introduce el número del distrito: ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out int idx) || idx < 1 || idx > districts.Count)
                {
                    WriteError("Distrito no válido.");
                    return;
                }
                selectedIndexes = new int[] { idx - 1 };
            }
            var selectedDistricts = selectedIndexes.Select(i => districts[i]).ToList();

            // Generación de código único para la calle
            var existingCodes = context.Streets.Select(s => s.Code).ToHashSet();
            var words = baseName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int[] lengths = Enumerable.Repeat(2, words.Length).ToArray();
            string uniqueCode = null;
            string code;
            while (true)
            {
                var initials = string.Join("-", words.Select((word, idx) =>
                {
                    int len = Math.Min(lengths[idx], word.Length);
                    var part = word.Substring(0, len).ToUpper();
                    return RemoveDiacritics(part);
                }));
                var acronym = streetType.GetAcronym();
                code = string.IsNullOrEmpty(acronym) ? initials : $"{acronym}-{initials}";
                if (!existingCodes.Contains(code))
                {
                    uniqueCode = code;
                    break;
                }
                int idx = words.Length - 1;
                while (idx >= 0 && lengths[idx] >= words[idx].Length)
                    idx--;
                if (idx < 0)
                    break;
                lengths[idx]++;
            }
            if (uniqueCode == null)
            {
                WriteError("No se pudo generar un código único para esta calle.");
                return;
            }
            var composedName = Street.GetComposedName(baseName, streetType);
            var street = new Street(baseName, streetType, uniqueCode);
            if (context.Streets.Any(s => s.Name == street.Name))
            {
                WriteWarning($"Ya existe una calle con ese nombre compuesto: {street.Name}");
                return;
            }
            if (context.Streets.Any(s => s.Code == street.Code))
            {
                WriteWarning($"Ya existe una calle con ese código: {street.Code}");
                return;
            }
            context.Streets.Add(street);
            context.SaveChanges();

            // Relacionar con distritos en DistrictStreet
            foreach (var district in selectedDistricts)
            {
                context.Database.ExecuteSql($"INSERT INTO DistrictStreet (DistrictId, StreetId) VALUES ('{district.Id}', '{street.Id}')");
            }
            WriteSuccess($"Calle insertada correctamente: {street.Name} (Código: {street.Code}) y relacionada con distrito(s).");
        }

        static void ListStreets(AppDbContext context)
        {
            WriteSeparator();
            var streets = context.Streets.OrderBy(s => s.Name).ToList();
            if (!streets.Any())
            {
                WriteWarning("No hay calles registradas.");
                return;
            }
            WriteTitle("Listado de calles:");
            foreach (var s in streets)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"- {s.Name} (Código: {s.Code})");
                Console.ResetColor();
            }
        }

        static void SearchStreet(AppDbContext context)
        {
            WriteSeparator();
            Console.Write("Introduce parte del nombre a buscar: ");
            var query = Console.ReadLine()?.Trim() ?? string.Empty;
            var results = context.Streets
                .Where(s => s.Name.Contains(query))
                .OrderBy(s => s.Name)
                .ToList();
            if (!results.Any())
            {
                WriteWarning("No se encontraron calles con ese criterio.");
                return;
            }
            WriteTitle("Resultados:");
            foreach (var s in results)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"- {s.Name} (Código: {s.Code})");
                Console.ResetColor();
            }
        }

        static void InsertDistrict(AppDbContext context)
        {
            WriteSeparator();
            Console.Write("Nombre del distrito: ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 255)
            {
                WriteError("Nombre no válido.");
                return;
            }
            Console.Write("Código postal (Zipcode): ");
            var zipcode = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(zipcode) || zipcode.Length < 2 || zipcode.Length > 20)
            {
                WriteError("Código postal no válido.");
                return;
            }
            Console.Write("Ciudad: ");
            var city = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(city) || city.Length < 2 || city.Length > 255)
            {
                WriteError("Ciudad no válida.");
                return;
            }
            if (context.Districts.Any(d => d.Name == name))
            {
                WriteWarning($"Ya existe un distrito con ese nombre: {name}");
                return;
            }
            if (context.Districts.Any(d => d.Zipcode == zipcode))
            {
                WriteWarning($"Ya existe un distrito con ese código postal: {zipcode}");
                return;
            }
            var district = new District
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Zipcode = zipcode,
                Country = "España",
                City = city,
                BuildingCount = 0
            };
            context.Districts.Add(district);
            context.SaveChanges();
            WriteSuccess($"Distrito insertado correctamente: {district.Name} (Código postal: {district.Zipcode})");
        }

        static void ListDistricts(AppDbContext context)
        {
            WriteSeparator();
            var districts = context.Districts.OrderBy(d => d.Name).ToList();
            if (!districts.Any())
            {
                WriteWarning("No hay distritos registrados.");
                return;
            }
            WriteTitle("Listado de distritos:");
            foreach (var d in districts)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"- {d.Name} (Código: {d.Zipcode})");
                Console.ResetColor();
            }
        }
    }
}
