
namespace Hospital.Models
{
    public class Paciente 
    {
        public String? Nombre { get; set; }
        public String? Altura { get; set; }
        public String? Direccion { get; set; }
        public String? Genero { get; set; }
        public String? Identificacion { get; set; }
        public Enfermedad? Enfermedad { get; set; }
        public String? Peso { get; set; }
        public String? Telefono { get; set; }
        public List <Hospital>? hospital { get; set; }
        public List<Equipos>? equipos { get; set; }
        public List<Servicios>? servicios { get; set; }

    }
}