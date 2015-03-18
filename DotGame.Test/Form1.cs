using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame;
using DotGame.Graphics;
using Color = DotGame.Graphics.Color;

namespace DotGame.Test
{
    public partial class Form1 : Form
    {
        public Engine Engine { get; private set; }

        public Form1()
        {
            InitializeComponent();

            var props = typeof(Color).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var prop in props)
            {
                comboBox1.Items.Add(prop.GetValue(null));
                comboBox1.SelectedItem = Color.CornflowerBlue;
            }
            comboBox1.Update();

            Engine = new Engine(new EngineSettings()
            {
                GraphicsAPI = GraphicsAPI.DirectX11
            }, splitContainer1.Panel1);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Engine.Dispose();
            base.OnFormClosing(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Engine.GraphicsDevice.Clear((Color)comboBox1.SelectedItem);
            Engine.GraphicsDevice.SwapBuffers();
        }
    }
}
