

namespace Hospital.Models
{
    public class Personal 
    {
        public String Nombre { get; set; }
        public String Disponibilidad { get; set; }
        public String Identificacion { get; set; }

        public String Telefono { get; set; }

        public List<Hospital> hospital { get; set; }
        public List<Servicios> servicios { get; set; }
        public List<Enfermedad> enfermedad { get; set; }


    }
}