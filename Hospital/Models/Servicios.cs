

namespace Hospital.Models
{
    public class Servicios 
    {
        public String Nombre { get; set; }


        public List<Paciente> paciente { get; set; }
        public List<Personal> personal { get; set; }

        public List<Hospital> hospital { get; set; }
        public List<Equipos> equipos { get; set; }
    }
}