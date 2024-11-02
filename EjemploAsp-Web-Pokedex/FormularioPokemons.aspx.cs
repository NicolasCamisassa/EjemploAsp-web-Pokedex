using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace EjemploAsp_Web_Pokedex
{
    public partial class FormularioPokemons : System.Web.UI.Page
    {
        public bool ConfirmarEliminacion {  get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            ConfirmarEliminacion = false;
            try
            {
                //Configuracion inicial de pantalla
                if (!IsPostBack)
                {
                    ElementoNegocio negocio = new ElementoNegocio();
                    List<Elemento> lista = negocio.Listar();

                    ddlTipo.DataSource = lista;
                    ddlTipo.DataValueField = "Id";
                    ddlTipo.DataTextField = "Descripcion";
                    ddlTipo.DataBind();

                    ddlDebilidad.DataSource = lista;
                    ddlDebilidad.DataValueField = "Id";
                    ddlDebilidad.DataTextField = "Descripcion";
                    ddlDebilidad.DataBind();
                }

                //Configuracion si Estamos modificando
                string id = Request.QueryString["Id"] != null ? Request.QueryString["Id"].ToString() : "";
                if (id != "" && !IsPostBack)
                {
                    PokemonNegocio negocio = new PokemonNegocio();
                    //List<Pokemon> lista = negocio.Listar(id);
                    //Pokemon seleccionado = lista[0];
                    Pokemon seleccionado = (negocio.Listar(id))[0];

                    //guardo pokemon seleccionado en session
                    Session.Add("PokeSeleccionado", seleccionado);

                    //pre cargar todos los campos
                    txtId.Text = id;
                    txtNumero.Text = seleccionado.Numero.ToString();
                    txtNombre.Text = seleccionado.Nombre;
                    txtDescripcion.Text = seleccionado.Descripcion;
                    txtImagenUrl.Text = seleccionado.UrlImagen;

                    ddlTipo.SelectedValue = seleccionado.Tipo.ID.ToString();
                    ddlDebilidad.SelectedValue = seleccionado.Debilidad.ID.ToString();
                    txtImagenUrl_TextChanged(sender, e);

                    // Configurar Acciones
                    if (!seleccionado.Activo)
                        btnInactivar.Text = "Reactivar";
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex);
            }
        }

        protected void btnAcpetar_Click(object sender, EventArgs e)
        {
            try
            {
                Pokemon nuevo = new Pokemon();
                PokemonNegocio negocio = new PokemonNegocio();

                nuevo.Numero = int.Parse(txtNumero.Text);
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.UrlImagen = txtImagenUrl.Text;

                nuevo.Tipo = new Elemento();
                nuevo.Tipo.ID = int.Parse(ddlTipo.SelectedValue);
                nuevo.Debilidad = new Elemento();
                nuevo.Debilidad.ID = int.Parse(ddlDebilidad.SelectedValue);

                if (Request.QueryString["id"] != null)
                {
                    nuevo.Id = int.Parse(txtId.Text);    
                    negocio.ModificarConSP(nuevo);
                }
                else
                    negocio.AgregarConSP(nuevo);
                
                
                Response.Redirect("PokemonsLista.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex);
                throw;
            }
        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            ImgPokemon.ImageUrl = txtImagenUrl.Text;
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            ConfirmarEliminacion = true;
        }

        protected void btnConfirmEliminacion_Click(object sender, EventArgs e)
        {
            try
            {
                if(checkConfirmEliminacion.Checked)
                {
                    PokemonNegocio negocio = new PokemonNegocio();
                    negocio.Eliminar(int.Parse(txtId.Text));
                    Response.Redirect("PokemonsLista.aspx");
                }
            }
            catch (Exception ex)
            {

                Session.Add("error", ex);
            }
        }

        protected void btnInactivar_Click(object sender, EventArgs e)
        {
            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                Pokemon seleccionado = (Pokemon)Session["PokeSeleccionado"];

                negocio.EliminarLogico(seleccionado.Id, !seleccionado.Activo);
                Response.Redirect("PokemonsLista.aspx");
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
            }
        }
    }
}