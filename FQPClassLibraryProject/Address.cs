using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQPClassLibraryProject
{
    public class Address
    {
        public string Id { get; set; } = string.Empty;
        public District AddressDistrict { get; set; } = new District();
        public string Street { get; set; } = string.Empty;
        public string Doorway { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Door { get; set; } = string.Empty;

        public Address() { }

        public override string ToString()
        {
            return $"{Street} {Doorway}, {Floor}º {Door}, {AddressDistrict.ZipCode} {AddressDistrict.City}, {AddressDistrict.Country}";
        }
    }
}
