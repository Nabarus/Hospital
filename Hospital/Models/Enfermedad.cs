
namespace Hospital.Models
{
    public class Enfermedad 
    {
        public String? Gravedad { get; set; }
        public String? Nombre { get; set; }
        public List<Paciente> pacientes { get; set; }
        public List<Personal> personal { get; set; }
        public List<Equipos> equipos { get; set; }

    }
}