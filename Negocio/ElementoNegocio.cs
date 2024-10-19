using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ElementoNegocio
    {
        public List<Elemento> Listar()
        {
            List<Elemento> lista = new List<Elemento>();  // 1.CREAMOS UNA LISTA PARA ACCEDER A LOS DATOS
            AccesoDatos datos = new AccesoDatos(); // 2.ACCESO A DATOS DE LA CONEXION
            
            
            try
            {
                datos.SetConsulta("Select Id, descripcion From ELEMENTOS"); // 3.SETEAR CONSULTA
                datos.EjecutarLectura(); // 4.EJECUTAR LECTURA

                while(datos.Lector.Read()) //5. WHILE PARA LEER TODOS LOS DATOS
                {
                    Elemento aux = new Elemento();
                    aux.ID = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }
                
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion(); //6. CERRAR CONEXION
            }
            
        }
    }
}
