using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using AudioSwitcher.AudioApi.CoreAudio;
using PInvoke;
using System.Runtime.InteropServices;
using baxp.Winamp;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        InputSimulator simulator = new InputSimulator();
        CoreAudioController controller;
        CoreAudioDevice device;

        public Form1()
        {
            InitializeComponent();
            //this.controller = new CoreAudioController();
            //int a = 5;
            //this.device = controller.DefaultPlaybackDevice;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            this.print(e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.print(e);
        }

        private void print(KeyEventArgs e)
        {
            this.textBox1.Text = e.KeyCode.ToString() + "   " + ((int)e.KeyCode).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WinampController.NextTrack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WinampController.PreviousTrack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WinampController.Pause();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //device.Volume++;
            //this.textBox1.Text = device.Volume.ToString();
            //this.simulator.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //device.Volume--;
            //this.textBox1.Text = device.Volume.ToString();
            //this.simulator.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.simulator.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // WM_WA_IPC = wm_user
            // 125 - current id
            // 3034 - current title 
            // IPC_GETPLAYLISTTITLEW 213
            // IPC_GET_PLAYING_TITLE 3034
            // IPC_GETINFO 126
            // IPC_GETMODULENAME 109
            // IPC_PLAYING_FILE 3003 

            this.textBox1.Text = baxp.Winamp.WinampController.GetCurrentTrack();
        }

    }
}