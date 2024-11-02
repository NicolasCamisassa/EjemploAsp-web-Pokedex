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
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            try
            {
                Trainee user = new Trainee();
                TraineeNegocio traineeNegocio = new TraineeNegocio();
                EmailService emailService = new EmailService();
                user.Email = txtEmail.Text;
                user.Pass = txtPassword.Text;
                int id = traineeNegocio.insertarNuevo(user);

                emailService.armarCorreo(user.Email, "Bienvenido Trainee", "Te damos la bienvenida a la aplicacion");
                emailService.enviarEmail();
                
                Response.Redirect("Default.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }
    }
}