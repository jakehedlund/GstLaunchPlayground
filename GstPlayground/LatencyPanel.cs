using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace GstPlayground
{
    public class LatencyPanel : Panel
    {
        public bool FlashEnabled { get; set; } = false;
        public bool OnState { get; set; } = false;

        public uint FlashOnTime { get; set; } = 50; //ms
        public int FlashInterval { get { return timer?.Interval ?? 0; } set { timer.Interval = value; } } //ms

        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        private DateTime lastFlash = DateTime.Now;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private Thread tmrThread; 

        public Rectangle MeasureBox { get; set; }
        Rectangle _measBox = new Rectangle(50, 150, 50, 50);
        Pen rPen = new Pen(Color.Red, 2f);


        Control vidStream;

        public event EventHandler<Tuple<double,Color>> LatencyEvent;

        public LatencyPanel(Control c)
        {
            
            vidStream = c;
            this.Location = c.Location;
            this.Size = c.Size;
            this.Visible = false;
            this.BackColor = Color.Teal;
            base.Enabled = true;

            timer.Tick += Timer_Tick;

            tmrThread = new Thread(Watcher)
            {
                IsBackground = true,
                Name = "watcher thread",
            };

            c.LocationChanged += C_LocationChanged;
            c.ClientSizeChanged += C_ClientSizeChanged;
        }

        private void C_ClientSizeChanged(object sender, EventArgs e)
        {
            this.ClientSize = this.vidStream.ClientSize;
        }

        private void C_LocationChanged(object sender, EventArgs e)
        {
            this.Location = this.vidStream.Location;
        }

        public Point cursPoint = new Point(); 
        private void Watcher()
        {
            while (true)
            {
                var elapsed = DateTime.Now - lastFlash; 
                if (elapsed.TotalMilliseconds > FlashOnTime && OnState)
                {
                    Flash(false);
                    //Thread.Sleep((int)(this.FlashInterval - FlashOnTime - 50));
                }
                else if (FlashEnabled && !OnState)
                {
                    NativeMethods.GetCursorPos(ref cursPoint);
                    Color c = ExtensionMethods.GetColorAt(cursPoint); 
                    if(c.B == c.G  && c.R < 0x10 && c.B > 0x20)
                    {
                        LatencyEvent?.Invoke(this, new Tuple<double, Color>(sw.ElapsedMilliseconds, c));
                    }
                }
                else
                {
                    //Thread.Sleep(10); 
                }
                Thread.Sleep(1); 
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Flash(true); 
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //if (Enabled)
            //{
            //    Graphics g = e.Graphics;
            //    g.DrawRectangle(rPen, _measBox);
            //}
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        public void Start()
        {
            this.FlashEnabled = true;
            timer.Start();
            lastFlash = DateTime.Now;
            OnState = true;
            if (tmrThread.ThreadState.HasFlag(ThreadState.Unstarted)) 
            {
                tmrThread.Start(); 
            }
            this.BringToFront(); 
        }

        public void Stop()
        {
            this.FlashEnabled = false;
            timer.Stop();
            Flash(false); 
        }

        private void Flash(bool on)
        {
            if (on)
            {
                lastFlash = DateTime.Now;
                sw.Restart(); 
            }
            OnState = on;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(Flash), on);
                return;
            }
            this.Visible = on;
            this.Invalidate();
            this.Refresh();
        }

    }
}
