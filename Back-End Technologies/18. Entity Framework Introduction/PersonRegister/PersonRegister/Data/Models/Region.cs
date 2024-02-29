using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonRegister.Data.Models
{
    public class Region
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public Icollection<Person> Perso { get; set; }
    }
}
