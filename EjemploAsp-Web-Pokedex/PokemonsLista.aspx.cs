using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace EjemploAsp_Web_Pokedex
{
    public partial class PokemonsLista : System.Web.UI.Page
    {
        public bool FiltroAvanzado {  get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            FiltroAvanzado = CheckFiltroAvanzado.Checked;
            if (!IsPostBack)
            {
                PokemonNegocio negocio = new PokemonNegocio();
                Session.Add("ListaPokemons", negocio.ListarConSP());
                dgvPokemos.DataSource = Session["ListaPokemons"];
                dgvPokemos.DataBind();
            }
        }

        protected void dgvPokemos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Id = dgvPokemos.SelectedDataKey.Value.ToString();
            Response.Redirect("FormularioPokemons.aspx?Id=" + Id);
        }
        protected void dgvPokemos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPokemos.DataSource = Session["ListaPokemons"];
            dgvPokemos.PageIndex = e.NewPageIndex;
            dgvPokemos.DataBind();
        }

        protected void Filtro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> lista = (List<Pokemon>)Session["ListaPokemons"];
            List<Pokemon> listaFiltrada = lista.FindAll(x => x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()));
            dgvPokemos.DataSource= listaFiltrada;
            dgvPokemos.DataBind();
        }

        protected void CheckFiltroAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            FiltroAvanzado = CheckFiltroAvanzado.Checked;
            txtFiltro.Enabled = !FiltroAvanzado;
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCriterio.Items.Clear();
            if(ddlCampo.SelectedItem.ToString() == "Numero")
            {
                ddlCriterio.Items.Add("Igual a");
                ddlCriterio.Items.Add("Mayor a");
                ddlCriterio.Items.Add("Menor a");
            }
            else
            {
                ddlCriterio.Items.Add("Contiene");
                ddlCriterio.Items.Add("Comienza con");
                ddlCriterio.Items.Add("Termina con");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                dgvPokemos.DataSource = negocio.filtrar(
                    ddlCampo.SelectedItem.ToString(),
                    ddlCriterio.SelectedItem.ToString(),
                    txtFiltroAvanzado.Text,
                    ddlEstado.SelectedItem.ToString());
                dgvPokemos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex);
                throw;
            }
        }
    }
}