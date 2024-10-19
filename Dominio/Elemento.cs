using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio // cambiar el namespace al nuevo proyecto
{
    public class Elemento //agregar "public' a las clases para que sean vistas
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }

        public override string ToString() // creamos esta property para sobreescribir el dato que queremos que traiga
        {
            return Descripcion;
        }
    }
}


// ESTA CLASE VA A SERVIR PARA MODELAR LA INFORMACION DE ELEMENTOS EN EL PROGRAMA 