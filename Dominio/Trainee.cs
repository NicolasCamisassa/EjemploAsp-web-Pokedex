using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Trainee
    {
        public int ID {  get; set; }
        private string email;
        public string Email { get; set; }
        public string Pass {  get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ImagenPerfil { get; set; }
        public bool Admin {  get; set; }
    }
}
