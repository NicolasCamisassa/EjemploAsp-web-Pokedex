using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion; //1.DECLARACION DE VARIABLES PARA CONEXION, COMANDO Y LECTOR 
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector // 5.PROPERTY PARA LEER DESDE DEL EXTERIOR
        {
            get { return lector; }
        }

        public AccesoDatos() // 2.CREAMOS EL CONSTRUCTOR(SOBRECARGADO) PARA CREAR LA CONEXION
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            comando = new SqlCommand();
        }

        public void SetConsulta(string consulta) //3.METODO O FUNCION PARA REALIZAR LA CONSULTA
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void EjecutarLectura() // 4.METODO O FUNCION PARA EJECUTAR LA LECTURA
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EjecutarAccion() //CREAMOS UNA FUNCION o METODO PARA EJECUTAR UNA ACCION 
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        
        public void CerrarConexion() // FUNCION PARA CERRAR LA CONEXION
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }


    }
}
