﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tienda_Buceo_v1
{
    public partial class Form1 : Form
    {

        // Con este nombre llamaremos al formulario Pantalla Inicial.
        Form2 formPantallaInicial;

        public Form1()
        {
            InitializeComponent();

            // Creamos el formulario principal.
            formPantallaInicial = new Form2(this);

            // Configuramos los eventos de teclado para poder utilizarlos.
            textBox_contrasena.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_usuario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            
        }

        private void boton_entrar_Click(object sender, EventArgs e)
        {
            entrar();
        }

        private void boton_salir_Click(object sender, EventArgs e)
        {
            Close();
        }

        /*
         * Este método nos va a permitir que cuando pulsemos el "Enter", sea lo mismo que pulsar el botón aceptar.
         */
        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // char 13 representa a la tecla "Enter" (Intro).
            if (e.KeyChar == (char)13)
            {
                // Si hemos llegado aquí es que hemos pulsado esa tecla.
                entrar();
            }
        }

        private void entrar() 
        {
            if (("root" == textBox_usuario.Text) && ("root" == textBox_contrasena.Text))
            {
                Hide();
                formPantallaInicial.StartPosition = FormStartPosition.CenterScreen;
                formPantallaInicial.label_usuario.Text = "Usuario: " + textBox_usuario.Text.ToString();
                formPantallaInicial.Show();
            }
            else
            {
                label2.Text = "Usuario y/o Contraseña incorrecta";

            }
            textBox_usuario.Text = "";
            textBox_contrasena.Text = "";
        }
    }
}