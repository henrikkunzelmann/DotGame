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

        private readonly Test2Component component;
        private ISound stream;
        private ISoundInstance streamInstance;
        private ISound threedee; // LOOL
        private ISoundInstance threedeeInstance;
        
        public Form1()
        {
            InitializeComponent();

            Engine = new Engine(new EngineSettings()
            {
                GraphicsAPI = GraphicsAPI.OpenGL4,
                AudioAPI = AudioAPI.OpenAL
            }, null);
            // splitContainer1.Panel1

            Engine.AddComponent(component = new Test2Component(Engine));

            Engine.AudioDevice.Listener.Gain = 0.1f;

            stream = Engine.AudioDevice.Factory.CreateSound("test.ogg", SoundFlags.Streamed | SoundFlags.AllowRead);
            streamInstance = stream.CreateInstance();
            //component.Visualize = streamInstance;

            threedee = Engine.AudioDevice.Factory.CreateSound("16-44100.wav", SoundFlags.Support3D);
            threedeeInstance = threedee.CreateInstance();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Engine.Stop();
            Engine.Dispose();
            base.OnFormClosing(e);
        }

        private void btnStreamPlay_Click(object sender, EventArgs e)
        {
            streamInstance.Play();
        }

        private void btnStreamPause_Click(object sender, EventArgs e)
        {
            streamInstance.Pause();
        }

        private void btnStreamStop_Click(object sender, EventArgs e)
        {
            streamInstance.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lbStreamQueued.Text = streamInstance.StreamBufferCount.ToString();
            lbStreamProcessed.Text = streamInstance.StreamBuffersProcessed.ToString();
        }

        private void tbStreamGain_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            streamInstance.Gain = (s.Value - s.Minimum) / (float)s.Maximum;
        }

        private void tbStreamPitch_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            streamInstance.Pitch = (s.Value - s.Minimum) / 25.0f;
        }

        private void cbStreamLoop_CheckedChanged(object sender, EventArgs e)
        {
            var s = sender as CheckBox;
            streamInstance.IsLooping = s.Checked;
        }

        private void btn3DPlay_Click(object sender, EventArgs e)
        {
            threedeeInstance.Play();
        }

        private void btn3DPause_Click(object sender, EventArgs e)
        {
            threedeeInstance.Pause();
        }

        private void btn3DStop_Click(object sender, EventArgs e)
        {
            threedeeInstance.Stop();
        }

        private void cb3DLoop_CheckedChanged(object sender, EventArgs e)
        {
            var s = sender as CheckBox;
            threedeeInstance.IsLooping = s.Checked;
        }

        private void tb3DGain_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            threedeeInstance.Gain = (s.Value - s.Minimum) / (float)s.Maximum;
        }

        private void tb3DPitch_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            threedeeInstance.Pitch = (s.Value - s.Minimum) / 25.0f;
        }

        private void tb3Dx_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            var pos = threedeeInstance.Position;
            pos.X = s.Value / 10f;
            threedeeInstance.Position = pos;
        }

        private void tb3Dy_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            var pos = threedeeInstance.Position;
            pos.Y = s.Value / 10f;
            threedeeInstance.Position = pos;
        }

        private void tb3Dz_Scroll(object sender, EventArgs e)
        {
            var s = sender as TrackBar;
            var pos = threedeeInstance.Position;
            pos.Z = s.Value / 10f;
            threedeeInstance.Position = pos;
        }
    }
}
