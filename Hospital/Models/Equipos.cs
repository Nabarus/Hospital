
namespace Hospital.Models
{
    public class Equipos 
    {
        public String Nombre { get; set; }
        public String Disponibilidad { get; set; }
        public List<Paciente> paciente { get; set; }
        public List<Hospital> hospital { get; set; }
        public List<Servicios> servicios { get; set; }
        public List<Enfermedad> enfermedad { get; set; }
    }
}