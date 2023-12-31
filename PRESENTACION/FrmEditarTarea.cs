﻿using ENTIDADES;
using LOGICA_NEGOCIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRESENTACION
{
    public partial class FrmEditarTarea : Form
    {
        LogicaTareas logicaTareas = new LogicaTareas();
        LogicaEstados logicaEstados = new LogicaEstados();
        LogicaCategorias logicaCategorias = new LogicaCategorias();
        public int idTarea;

        public FrmEditarTarea()
        {
            InitializeComponent();
        }

        // Validar campos 
        public bool ValidarCampos()
        {
            bool errorFlag = false;
            errorProvider1.Clear();

            if (txtTitulo.Text.Equals(""))
            {
                errorProvider1.SetError(txtTitulo, "Campo requerido");
                errorFlag = true;
            }

            if (cbEstado.Text.Equals(""))
            {
                errorProvider1.SetError(cbEstado, "Campo requerido");
                errorFlag = true;
            }

            if (cbCategoria.Text.Equals(""))
            {
                errorProvider1.SetError(cbCategoria, "Campos requerido");
                errorFlag = true;
            }

            if (errorFlag) { return true; } else { return false; }
        }

        // Cargar datos
        public void CargarDatos()
        {
            // Obtner la tarea
            Tarea tarea = logicaTareas.ObtenerTarea(idTarea);

            // Validar si la tarea existe
            if (tarea == null)
            {
                MessageBox.Show("No se ha podido cargar la tarea", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cargar los datos de la tarea
            txtTitulo.Text = tarea.Titulo;
            dtpFecha.Value = Convert.ToDateTime(tarea.Fecha);
            txtApuntes.Text = tarea.Apuntes;

            // seleccionar el estado
            List<Estado> listaEstados = logicaEstados.ObtenerEstados();
            foreach (Estado estado in listaEstados)
            {
                if (estado.IdEstado == tarea.IdEstado)
                {
                    cbEstado.SelectedIndex = listaEstados.IndexOf(estado);
                }
            }

            // seleccionar el estado
            List<Categoria> listaCategorias = logicaCategorias.ObtenerCategorias();
            foreach (Categoria categoria in listaCategorias)
            {
                if (categoria.IdCategoria == tarea.IdCategoria)
                {
                    cbCategoria.SelectedIndex = listaCategorias.IndexOf(categoria);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar campos
            if (ValidarCampos()) return;

            // Obtener el ID del estado
            List<Estado> listaEstados = logicaEstados.ObtenerEstados();
            int idEstado = listaEstados[cbEstado.SelectedIndex].IdEstado;

            // Obtener el ID de la categoría
            List<Categoria> listaCategorias = logicaCategorias.ObtenerCategorias();
            int idCategoria = listaCategorias[cbCategoria.SelectedIndex].IdCategoria;

            // Crear una nueva tarea
            if (!logicaTareas.ActualizarTarea(new Tarea(idTarea, txtTitulo.Text, dtpFecha.Value.ToShortDateString(),
                idEstado, idCategoria, txtApuntes.Text)))
            {
                MessageBox.Show("No se han podido guardar los cambios de la tarea", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!logicaTareas.EliminarTarea(idTarea))
            {
                MessageBox.Show("No se ha podido eliminar la tarea", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Close();
        }

        private void btnDescartar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmEditarTarea_Load(object sender, EventArgs e)
        {


            cbEstado.Items.Clear();
            foreach (Estado estado in logicaEstados.ObtenerEstados())
                cbEstado.Items.Add(estado.Nombre);

            cbCategoria.Items.Clear();
            foreach (Categoria categoria in logicaCategorias.ObtenerCategorias())
                cbCategoria.Items.Add(categoria.Nombre);

            CargarDatos();
        }
    }
}
