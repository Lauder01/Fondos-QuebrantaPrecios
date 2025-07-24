namespace FQPClassLibraryProject
{
    public class Building
    {
        public string Id {  get; set; } = "Empty";
        public string Name { get; set; } = "Empty";
        public Address BuildingAdress { get; set; } = new Address();
        public int FloorNumber { get; set; } = 0;
        public string Status { get; set; } = "Empty"; //Este despues va a ser un enum
        public string Description { get; set; } = "Empty";
        public bool HasLift { get; set; } = false;
        
        //en caso de tener urbanización , se pone el id de la urbanización
        
        public Building() { }

        
    }
}