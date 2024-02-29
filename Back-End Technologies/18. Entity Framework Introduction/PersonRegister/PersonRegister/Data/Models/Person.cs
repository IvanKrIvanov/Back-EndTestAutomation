namespace PersonRegister.Data.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City {  get; set; }
        public int Age { get; set; }
        public int RegionId { get; set;}

    }
}
