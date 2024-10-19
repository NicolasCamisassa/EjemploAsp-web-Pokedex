using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // 5.INCLUIMOS UNA LIBRERIA PARA PODER DECLARAR OBJETOS PARA LEER EN SQL
using Dominio;
using System.Diagnostics.Eventing.Reader;
using System.Collections;


namespace Negocio // cambiar el namespace al nuevo proyecto
{
    // 1.CREAMOS UNA CLASE PARA PODER ACCEDER A LOS DATOS DESDE EL EXTERIOR
    public class PokemonNegocio // cambiar el namespace al nuevo proyecto
    {
        // 2.CREAMOS LA FUNCION O METODO PARA LEER ESOS DATOS POR MEDIO DE UNA LISTA
        public List<Pokemon> Listar()
        {
            // 6.CREAMOS OBJETOS PARA PODER LEER EN LA DB
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            /*SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;*/

            // 3.CREAMOS EL MANEJO DE EXCEPCIONES PARA PODER OBTENER DATOS DE LA LISTA
            try
            {
                // 4.POMENOS AQUI TODA LA FUNCIONALIDAD QUE PUEDA FALLAR 
                // 7.CONFIGURAMOS LA CADENA DE CONEXION A LA DB
                /*conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1";
                comando.Connection = conexion;
                */
                datos.SetConsulta("Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1");
                datos.EjecutarLectura();
                
               /* lector = comando.ExecuteReader();*/

                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    //1. FORMA DE VALIDAR UN NULL
                    //if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))
                    //    aux.UrlImagen = (string)lector["UrlImagen"];
                    
                    //2. 2DA FORMA DE VALIDAR UN NULL
                    if (!(datos.Lector["UrlImagen"] is DBNull)) //EL SIGNO ! ES PARA NEGAR
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento(); //creamos esta property para que no nos de nulla cuando lo instanciemos
                    aux.Tipo.ID = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.ID = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    
                    lista.Add(aux);
                }

                datos.CerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            // ESTE METODO YA DEVUELVE UNA LISTA
        }

        public void Agregar(Pokemon nuevo) // 6.CONSTRUIR LA LOGICA PARA QUE SE CONECTE A DB
        {
            AccesoDatos datos = new AccesoDatos(); //6.OBJETO DE LA CLASE "ACCESO A DATOS"

            try //SETEAR LA CONSULTA QUE QUIERO HACER
            {
                datos.SetConsulta("Insert into POKEMONS(Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" +nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @UrlImagen)");
                datos.SetearParametro("@idTipo", nuevo.Tipo.ID);
                datos.SetearParametro("@idDebilidad", nuevo.Debilidad.ID);
                datos.SetearParametro("@UrlImagen", nuevo.UrlImagen);
                datos.EjecutarAccion();
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

        public void Modificar(Pokemon modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad where Id = @Id");
                datos.SetearParametro("@numero", modificar.Numero);
                datos.SetearParametro("@nombre", modificar.Nombre);
                datos.SetearParametro("@desc", modificar.Descripcion);
                datos.SetearParametro("@img", modificar.UrlImagen);
                datos.SetearParametro("@idTipo", modificar.Tipo.ID);
                datos.SetearParametro("@idDebilidad", modificar.Debilidad.ID);
                datos.SetearParametro("@Id", modificar.Id);

                datos.EjecutarAccion();
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
        
        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1 And ";
                if (campo == "Numero")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con ":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con ":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con ":
                            consulta += "P.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con ":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                
                datos.SetConsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];                                     
                    if (!(datos.Lector["UrlImagen"] is DBNull)) //EL SIGNO ! ES PARA NEGAR
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    aux.Tipo = new Elemento(); //creamos esta property para que no nos de nulla cuando lo instanciemos
                    aux.Tipo.ID = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.ID = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public void Eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetConsulta("delete from POKEMONS where id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }

        public void EliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetConsulta("update POKEMONS set Activo = 0 where id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
    }
}
