﻿using MySql.Data.MySqlClient;
using System;
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
    public partial class FormClienteBorrar : Form
    {
        FormPantallaPrincipal formPantallaInicial;
        FormClienteBorrarAviso formClienteBorrarAviso;

        // La línea que guarda la IP del servidor, usuario y contraseña.
        String cadenaConexión;

        // Conector que almacena la conexión de la BBDD.
        MySqlConnection conexion;

        // Comando que quiero que se ejecute.
        MySqlCommand comando;

        // Consulta
        String sentenciaSQL;

        // Resultado de la consulta.
        MySqlDataReader resultado;

        ListViewItem item1;

        public FormClienteBorrar(FormPantallaPrincipal F)
        {
            InitializeComponent();
            formPantallaInicial = F;

            comboBox_titulacion.Items.Add("DISCOVERY SCUBA DIVER");
            comboBox_titulacion.Items.Add("OPEN WATER DIVER");
            comboBox_titulacion.Items.Add("ADVANCE OPEN WATER DIVER");
            comboBox_titulacion.Items.Add("RESCUE DIVER");
            comboBox_titulacion.Items.Add("DIVEMASTER");
            comboBox_titulacion.Items.Add("INSTRUCTOR");

            // Configuramos los eventos de teclado para poder utilizarlos.
            textBox_numCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_nombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_apellido1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_apellido2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_correoElectronico.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_telefonoFijo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            textBox_telefonoMovil.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            comboBox_titulacion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);

            try
            {
                cadenaConexión = "Server = " + formPantallaInicial.nombreServidor +
                                "; Database = " + formPantallaInicial.nombreBBDD +
                                "; Uid = " + formPantallaInicial.nombreUsuario +
                                "; Pwd = " + formPantallaInicial.contraseñaUsuario +
                                "; Port = " + formPantallaInicial.puertoConexion + ";";
                conexion = new MySqlConnection(cadenaConexión);
            }
            catch { }

            ActiveControl = textBox_numCliente;

        }


        /*
         * Este método nos va a permitir que cuando pulsemos el "Enter", sea lo mismo que pulsar el botón aceptar.
         */
        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Escape)
            {
                // Si hemos llegado aquí es que hemos pulsado esa tecla.
                Hide();
                borrarDatos();
                formPantallaInicial.Show();
            }
        }


        /*
         * Este método nos va a permitir dejar todos los canpos vacios.
         */
        private void borrarDatos()
        {
            // Procedemos a borrar todos los campos.
            textBox_numCliente.Text = "";
            textBox_nombre.Text = "";
            textBox_apellido1.Text = "";
            textBox_apellido2.Text = "";
            textBox_telefonoFijo.Text = "";
            textBox_telefonoMovil.Text = "";
            textBox_correoElectronico.Text = "";
            comboBox_titulacion.Text = "";
            label2.Text = "";


            pictureBox1.Image = new Bitmap(Application.StartupPath + "\\Fotos\\0.png");
        }


        private void boton_cancelar_Click(object sender, EventArgs e)
        {
            Close();
            borrarDatos();
            formPantallaInicial.Show();
            
        }

        private void textoAMayusculas()
        {
            // Lo primero que vamos a hacer es pasar todos los campos a mayusculas.
            textBox_nombre.Text = textBox_nombre.Text.ToUpper();
            textBox_apellido1.Text = textBox_apellido1.Text.ToUpper();
            textBox_apellido2.Text = textBox_apellido2.Text.ToUpper();
            textBox_correoElectronico.Text = textBox_correoElectronico.Text.ToUpper();
        }

        //metodo para obtener el valor de un campo del formulario de tipo textBox o comboBox
        public String obtenerValorCampo(Control x)
        {
            String campo;
            if (x is TextBox)
            {
                campo = ((TextBox)x).Text;
            }
            else
            {
                campo = ((ComboBox)x).SelectedIndex.ToString();
            }
            return campo;
        }

        private void insertarFoto()
        {
            int numeroClienteFoto;
            if (textBox_numCliente.Text == "")
            {
                numeroClienteFoto = 0;
            }
            else
            {
                numeroClienteFoto = Int32.Parse(textBox_numCliente.Text);
            }

            try
            {
                pictureBox1.Image = new Bitmap(Application.StartupPath + "\\Fotos\\" + numeroClienteFoto + ".png");

            }
            catch
            {
                pictureBox1.Image = new Bitmap(Application.StartupPath + "\\Fotos\\0.png");
            }
        }

        private void boton_Buscar_Click(object sender, EventArgs e)
        {
            // Solo vamos a lanzar la consulta si hay algun dato relleno
            if ((textBox_numCliente.Text != "") || (textBox_nombre.Text != "") || (textBox_apellido1.Text != "") || (textBox_apellido2.Text != "")
                || (textBox_telefonoFijo.Text != "") || (textBox_telefonoMovil.Text != "") || (textBox_correoElectronico.Text != "")
                || (comboBox_titulacion.Text != ""))
            {
                // Pasamos todos los campos a mayusculas.
                textoAMayusculas();

                try { pictureBox1.Image = new Bitmap(@"\Fotos\0.png"); }
                catch { }

                // Aqui hariamos la consulta.
                sentenciaSQL = "SELECT * FROM sql27652.clientes WHERE ";
                Boolean masDeUnCampo = false;
                foreach (Control x in this.Controls)
                {
                    if ((x is TextBox) || (x is ComboBox))
                    {
                        String nombreCajaTexto = x.Name;
                        String campo = obtenerValorCampo(x);
                        String columna = "";
                        Boolean esNumero = false;
                        Console.WriteLine(">> Leyendo el campo: " + campo);
                        if ((campo != "") && (campo != "-1"))
                        {
                            if (nombreCajaTexto.Equals("textBox_numCliente"))
                            {
                                columna = "id_cliente";
                                esNumero = true;
                            }
                            else if (nombreCajaTexto.Equals("textBox_nombre"))
                            {
                                columna = "nombre";
                            }
                            else if (nombreCajaTexto.Equals("textBox_apellido1"))
                            {
                                columna = "apellido1";
                            }
                            else if (nombreCajaTexto.Equals("textBox_apellido2"))
                            {
                                columna = "apellido2";
                            }
                            else if (nombreCajaTexto.Equals("textBox_telefonoFijo"))
                            {
                                columna = "telefono_fijo";
                            }
                            else if (nombreCajaTexto.Equals("textBox_telefonoMovil"))
                            {
                                columna = "telefono_movil";
                            }
                            else if (nombreCajaTexto.Equals("textBox_correoElectronico"))
                            {
                                columna = "correo_electronico";
                            }
                            else if (nombreCajaTexto.Equals("comboBox_titulacion"))
                            {
                                columna = "titulacion";
                            }

                            Console.WriteLine(">> Leyendo el campo: " + columna);
                            if (masDeUnCampo)
                            {
                                sentenciaSQL += " AND ";
                            }
                            else
                            {
                                masDeUnCampo = true;
                            }

                            if (esNumero)
                            {
                                sentenciaSQL += columna + " = " + campo;
                            }
                            else
                            {
                                sentenciaSQL += columna + " = '" + campo + "'";
                            }

                        }
                    }
                }

                try
                {
                    // Iniciamos la conexion.
                    conexion.Open();
                    comando = new MySqlCommand(sentenciaSQL, conexion);
                    resultado = comando.ExecuteReader();
                    Console.WriteLine(">> Realizando la consulta: " + sentenciaSQL);
                    listView1.Items.Clear();
                    int numResultados = 0;
                    while (resultado.Read())
                    {
                        item1 = new ListViewItem(resultado["id_cliente"].ToString());
                        item1.SubItems.Add(resultado["nombre"].ToString());
                        item1.SubItems.Add(resultado["apellido1"].ToString());
                        item1.SubItems.Add(resultado["apellido2"].ToString());
                        item1.SubItems.Add(resultado["telefono_fijo"].ToString());
                        item1.SubItems.Add(resultado["telefono_movil"].ToString());
                        item1.SubItems.Add(resultado["correo_electronico"].ToString());

                        int datoAuxiliar = Int32.Parse(resultado["titulacion"].ToString());
                        switch (datoAuxiliar)
                        {
                            case 0: item1.SubItems.Add("DISCOVERY SCUBA DIVER"); break;
                            case 1: item1.SubItems.Add("OPEN WATER DIVER"); break;
                            case 2: item1.SubItems.Add("ADVANCE OPEN WATER DIVER"); break;
                            case 3: item1.SubItems.Add("RESCUE DIVER"); break;
                            case 4: item1.SubItems.Add("DIVEMASTER"); break;
                            case 5: item1.SubItems.Add("INSTRUCTOR"); break;
                            default: item1.SubItems.Add("SIN TITULACION"); break;
                        }

                        listView1.Items.AddRange(new ListViewItem[] { item1 });
                        numResultados++;
                    }
                    if (numResultados > 0)
                    {
                        label2.Text = "Se han encontrado los siguientes resultados:";
                        if (numResultados == 1)
                        {
                            insertarFoto();
                        }
                    }
                    else
                    {
                        label2.Text = "No se ha encontrado ningún resultado";
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Se ha producido un error al realizar la consulta: " + sentenciaSQL);
                }
            
            
            }else
            {
                MessageBox.Show("Error: No se ha insertado ningun dato de busqueda");
            }
        }

        private void boton_darDeBaja_Click(object sender, EventArgs e)
        {
            // Solo vamos a lanzar la consulta si hay algun dato relleno
            if ((textBox_numCliente.Text != "") || (textBox_nombre.Text != "") || (textBox_apellido1.Text != "") || (textBox_apellido2.Text != "")
                || (textBox_telefonoFijo.Text != "") || (textBox_telefonoMovil.Text != "") || (textBox_correoElectronico.Text != "")
                || (comboBox_titulacion.Text != ""))
            {
                // Abrimos el formulario de advertencia
                formClienteBorrarAviso = new FormClienteBorrarAviso(this);
                formClienteBorrarAviso.StartPosition = FormStartPosition.CenterScreen;
                formClienteBorrarAviso.Show();
            }else
            {
                MessageBox.Show("Error: No se ha insertado ningun dato a eliminar");
            }
        }

        public void dardeBaja() 
        {
            // Lo primero que vamos a hacer es pasar todos los campos a mayusculas.
            textoAMayusculas();

            try { pictureBox1.Image = new Bitmap(@"\Fotos\0.png"); }
            catch { }

            // Aqui hariamos la consulta.
            sentenciaSQL = "DELETE FROM sql27652.clientes WHERE ";
            Boolean masDeUnCampo = false;
            foreach (Control x in this.Controls)
            {
                if ((x is TextBox) || (x is ComboBox))
                {
                    String nombreCajaTexto = x.Name;
                    String campo = obtenerValorCampo(x);
                    String columna = "";
                    Boolean esNumero = false;
                    Console.WriteLine(">> Leyendo el campo: " + campo);
                    if ((campo != "") && (campo != "-1"))
                    {
                        if (nombreCajaTexto.Equals("textBox_numCliente"))
                        {
                            columna = "id_cliente";
                            esNumero = true;
                        }
                        else if (nombreCajaTexto.Equals("textBox_nombre"))
                        {
                            columna = "nombre";
                        }
                        else if (nombreCajaTexto.Equals("textBox_apellido1"))
                        {
                            columna = "apellido1";
                        }
                        else if (nombreCajaTexto.Equals("textBox_apellido2"))
                        {
                            columna = "apellido2";
                        }
                        else if (nombreCajaTexto.Equals("textBox_telefonoFijo"))
                        {
                            columna = "telefono_fijo";
                        }
                        else if (nombreCajaTexto.Equals("textBox_telefonoMovil"))
                        {
                            columna = "telefono_movil";
                        }
                        else if (nombreCajaTexto.Equals("textBox_correoElectronico"))
                        {
                            columna = "correo_electronico";
                        }
                        else if (nombreCajaTexto.Equals("comboBox_titulacion"))
                        {
                            columna = "titulacion";
                        }

                        Console.WriteLine(">> Leyendo el campo: " + columna);
                        if (masDeUnCampo)
                        {
                            sentenciaSQL += " AND ";
                        }
                        else
                        {
                            masDeUnCampo = true;
                        }

                        if (esNumero)
                        {
                            sentenciaSQL += columna + " = " + campo;
                        }
                        else
                        {
                            sentenciaSQL += columna + " = '" + campo + "'";
                        }

                    }
                }
            }

            try
            {
                // Iniciamos la conexion.
                conexion.Open();
                comando = new MySqlCommand(sentenciaSQL, conexion);
                resultado = comando.ExecuteReader();
                Console.WriteLine(">> Realizando la consulta: " + sentenciaSQL);
                listView1.Items.Clear();
                label2.Text = "Usuario dado de baja correctamente";
                borrarDatos();
                conexion.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Se ha producido un error al realizar la consulta: " + sentenciaSQL);
            }
        
        }


        private void boton_Borrar_Click(object sender, EventArgs e)
        {
            borrarDatos();
        }
    }
}

