using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class TraineeNegocio
    {
        //id
        //email
        //pass
        //admin false

        //nombre, apellido, fecha, imagen 

        public int insertarNuevo(Trainee nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearProcedimiento("insertarNuevo");
                datos.SetearParametro("@email", nuevo.Email);
                datos.SetearParametro("@pass", nuevo.Pass);
                return datos.EjecutarAccionScalar();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


    }
}
