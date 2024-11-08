﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EjemploAsp_Web_Pokedex.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Pagina oficial Pokedex</h1>
    <div class="row row-cols-1 row-cols-md-3 g-4">
        <asp:Repeater ID="Repetidor" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card" style="width: 18rem;">
                        <img src="<%#Eval("UrlImagen") %>" class="card-img-top img-small" alt="Imagen">
                        <div class="card-body">
                            <h5 class="card-title"><%#Eval("Nombre")%></h5>
                            <p class="card-text"><%#Eval("Descripcion") %></p>
                            <a href="DetallePokemon.aspx?id=<%#Eval("Id")%>">Ver Detalle</a>
                            <asp:Button ID="btnEjemplo" runat="server" Text="Ejemplo" CssClass="btn btn-primary" CommandArgument='<%#Eval("Id")%>' CommandName="PokemonId" OnClick="btnEjemplo_Click" />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <%--       <%
            foreach (Dominio.Pokemon poke in ListaPokemons)
            {
        %>
        <div class="col">
            <div class="card">
                <img src="<%: poke.UrlImagen %>" class="card-img-top" alt="...">
                <div class="card-body">
                    <h5 class="card-title"><%: poke.Nombre %></h5>
                    <p class="card-text"><%: poke.Descripcion %></p>
                    <a href="DetallePokemon.aspx?id=<%: poke.Id %>">Ver Detalle</a>
                </div>
            </div>
        </div>
        <% } %>--%>
</asp:Content>
