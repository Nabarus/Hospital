

namespace Hospital.Models
{
    public class Hospital 
    {
        public String Identificador { get; set; }

        public String Nombre { get; set; }
        public String Direccion { get; set; }

        public String Telefono { get; set; }


        public  String Equipos { get; set; }
        
        public String Paciente { get; set; }

        public String Personal { get; set; }


        public List <Servicios> servicios { get; set; }
        public List<Enfermedad> enfermedades { get; set; }
    }
}