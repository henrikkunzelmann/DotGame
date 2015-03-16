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

namespace DotGame.Test
{
    public partial class Form1 : Form
    {
        public Engine Engine { get; private set; }

        public Form1()
        {
            InitializeComponent();

            Engine = new Engine(new DotGame.Windows.GameWindow(this));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Engine.Dispose();
            base.OnFormClosing(e);
        }
    }
}
