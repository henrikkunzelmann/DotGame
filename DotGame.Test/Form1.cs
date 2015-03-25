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
using DotGame.Audio;
using Color = DotGame.Graphics.Color;

namespace DotGame.Test
{
    public partial class Form1 : Form
    {
        public Engine Engine { get; private set; }

        ISoundInstance instance;
        IEffectReverb reverb;
        float t;
        
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
                GraphicsAPI = GraphicsAPI.OpenGL4,
                AudioAPI = AudioAPI.OpenAL
            }, splitContainer1.Panel1);

            Engine.AudioDevice.Listener.Gain = 0.1f;

            var sound = Engine.AudioDevice.Factory.CreateSound("test.ogg", true);
            reverb = Engine.AudioDevice.Factory.CreateReverb();
            reverb.Density = 0.2f;
            reverb.Gain = 0.05f;
            Engine.AudioDevice.MasterChannel.Effect = reverb;
            propertyGrid1.SelectedObject = reverb;

            instance = sound.CreateInstance(true);
            instance.Pitch = 1f;
            instance.IsLooping = true;
            instance.Position = new Vector3(0, 8, 20);
            instance.Play(); // Uncommenten zum Abspielen 
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Engine.Stop();
            Engine.Dispose();
            base.OnFormClosing(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Engine.AudioDevice.MasterChannel.Effect = reverb;
            var newPos = new Vector3((float)Math.Cos(t) * 4, (float)Math.Sin(t) * 4, 0);
            var velocity = newPos - instance.Position;
            instance.Position = newPos;
            instance.Velocity = velocity;
            t += 0.03f;
        }
    }
}
