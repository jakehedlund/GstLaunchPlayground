using Gst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using sDbg = System.Diagnostics.Debug;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Diagnostics;
using System.Reflection;
using Gst.Video;
using GstPlayground.Properties;
using System.Text.RegularExpressions;

namespace GstPlayground
{
    public partial class Form1 : Form
    {
        Pipeline mpipe, convPipe;
        Thread glibThread;
        static GLib.MainLoop glibLoop;
        IntPtr hPnl = IntPtr.Zero;

        SerializableStringDictionary launchDict = new SerializableStringDictionary(); 
        LatencyPanel latencyPanel1;

        string launchDelim = ";";

        public Form1()
        {
            InitializeComponent();
            


            latencyPanel1 = new LatencyPanel(pnlGst);
            this.Controls.Add(latencyPanel1);
            latencyPanel1.BringToFront();
            latencyPanel1.FlashInterval = 1000;

            latencyPanel1.LatencyEvent += LatencyPanel1_LatencyEvent;
        }

        private void LatencyPanel1_LatencyEvent(object sender, Tuple<double,Color> e)
        {
            if (lblLatency.InvokeRequired)
            {
                lblLatency.Invoke(new Action<object, Tuple<double,Color>>(LatencyPanel1_LatencyEvent), sender, e);
                return;
            }

            Color c = e.Item2; 
            lblLatency.Text = e.Item1.ToString("0.00") + " ms";
            lblColor.Text = $"(R:{c.R},G:{c.G},B:{c.B})";

            //Point curs = new Point();
            //NativeMethods.GetCursorPos(ref curs);
            //lblColor.Text = ExtensionMethods.GetColorAt(curs).ToString();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hPnl = pnlGst.Handle;

            if (Settings.Default.upgradeNeeded)
            {
                Settings.Default.Upgrade();
                Settings.Default.Save();
                Settings.Default.Reload();
                Settings.Default.upgradeNeeded = false;
                Settings.Default.Save(); 
            }

            launchDict = Settings.Default.launchDict;

            int idx = Settings.Default.lastIdx;
            if (idx < 0 || idx >= launchDict.Count)
                idx = 0;

            cmbLaunch.BeginUpdate();
            cmbLaunch.Items.AddRange(launchDict.Keys.Cast<string>().ToArray());
            cmbLaunch.SelectedIndex = idx;
            cmbLaunch.EndUpdate(); 
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            resetGst();
            if (mpipe != null)
            {
                sDbg.WriteLine("------- START PREVIEW -------");
                using(var elt = mpipe.GetByName("recSink"))
                {
                    if(elt != null)
                        elt["location"] = @"C:\gstreamer\recording\test12345678.mp4";
                }

                mpipe.SetState(State.Playing); 
            }
        }

        private void resetGst()
        {
            if (mpipe != null)
            {
                mpipe.SendEvent(Event.NewEos());
                mpipe.Bus?.TimedPopFiltered((ulong)1e9, MessageType.Eos);
                mpipe.SetState(State.Null);
                mpipe.Dispose();
                //glibLoop.Quit(); 
            }

            try
            {
                if(!Gst.Global.IsInitialized)
                {
                    Gst.Application.Init();
                    GtkSharp.GstreamerSharp.ObjectManager.Initialize();
                    
                }

                var glibInfo = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(GLib.MainLoop)).Location);
                var gtkInfo = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(GtkSharp.GstreamerSharp.ObjectManager)).Location);

                Gst.Application.Version(out uint a, out uint b, out uint c, out uint d);
                Console.WriteLine($"GST version: {a}.{b}.{c}.{d}");
                Console.WriteLine($"GLib version: {glibInfo.FileVersion} ({glibInfo.FileName})" );
                Console.WriteLine($"GTK version: {gtkInfo.FileVersion} ({gtkInfo.FileName})" );

                if (glibThread != null && glibThread.IsAlive)
                {
                    glibLoop.Quit(); 
                    //glibThread.Abort();
                    glibThread = null;
                }

                if(glibLoop is null)
                    glibLoop = new GLib.MainLoop();

                glibThread = new Thread(glibLoop.Run)
                {
                    IsBackground = true,
                    Name = "GlibThread " + System.DateTime.Now.ToString("s"),
                    //Priority = ThreadPriority.AboveNormal
                };

                //glibThread.Start();
            }
            catch (BadImageFormatException bad)
            {
                sDbg.WriteLine("Failed to load: " + bad.FileName);
                sDbg.WriteLine(bad.Message); 
            }
            catch (Exception ex)
            {
                sDbg.WriteLine("Error resetting: " + ex.Message); 
                sDbg.WriteLine(ex.StackTrace);
                throw;
            }
            
            try
            {
                glibThread.Start();
                mpipe = Gst.Parse.Launch(SanitizeLaunchLine(txtPipe.Text)) as Pipeline;

                var bus = mpipe.Bus;

                //GstUtil.DumpGraph(mPipeline, "after_parse.dot");
                //GLib.Timeout.Add(100, queryStats);
                GLib.ExceptionManager.UnhandledException -= this.exceptionManager_UnhandledException;
                GLib.ExceptionManager.UnhandledException += this.exceptionManager_UnhandledException;
                //GLib.Timeout.Add(100, SetOverlayGst); 

                bus.EnableSyncMessageEmission();
                bus.SyncMessage += Bus_SyncMessage;
                bus.AddSignalWatch();
                bus.Message += HandleMessage;
                sDbg.WriteLine("reset gst."); 
                //bus.Dispose(); 
            }
            catch (BadImageFormatException bad)
            {
                sDbg.WriteLine("Failed to load: " + bad.FileName);
                sDbg.WriteLine(bad.Message);

                MessageBox.Show(this, "Failed to load Gstreamer binaries. Check x64/x86 compatibility.", "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                sDbg.WriteLine("Failed to create pipeline: " + ex.Message);
                MessageBox.Show(this, "Failed to launch, check your pipeline. " + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DumpGraph(mpipe, "failtolink.dot"); 
            }
        }

        private void exceptionManager_UnhandledException(GLib.UnhandledExceptionArgs args)
        {
            var ex = args.ExceptionObject as Exception; 

            sDbg.WriteLine("--- UNHANDLED EXCEPTION --- ");
            //sDbg.WriteLine(args.ExceptionObject);

            if (ex != null)
            {
                sDbg.WriteLine(ex.Message); 
                sDbg.WriteLine(ex.StackTrace); 
            }
        }

        //Gst.Message m; 
        private void HandleMessage(object o, MessageArgs args)
        {
            //sDbg.WriteLine("msg refcount: " + args.Message.Refcount);
            var m = args.Message;
            var oo = o as Bus;

            try
            {
                switch(m.Type)
                {
                    case MessageType.Error:
                        m.ParseError(out GLib.GException gex, out string dbg);
                        
                        sDbg.WriteLine("Error from: " + m.Src + ". -- " + gex.Message + " " + dbg);
                        break;
                    case MessageType.Warning:
                        m.ParseWarning(out IntPtr gg, out string dWarn);
                        var st = m.ParseWarningDetails();
                        sDbg.WriteLine("Warning from: " + m.Src + ". -- " + gg + " " + dWarn);
                        sDbg.WriteLine("Deets: " + st?.ToString()); 
                        break;
                    case MessageType.Qos:
                    case MessageType.Tag:
                        break;
                    case MessageType.Element:
                        if (m.Structure.HasField("event"))
                        {
                            var ev = m.Structure.GetValue("event").Val as Gst.Event;
                            //sDbg.WriteLine("Event message: " + ev.Structure.ToString());
                            switch (ev.Structure.GetString("event"))
                            {
                                case "mouse-move":
                                    ev.Structure.GetDouble("pointer_x", out double mx);
                                    ev.Structure.GetDouble("pointer_y", out double my);
                                    //sDbg.WriteLine($"({mx},{my})");
                                    break;
                                case "mouse-button-press":
                                    ev.Structure.GetInt("button", out int btnval);
                                    sDbg.WriteLine("Mouse pressed (BUS): " + btnval);
                                    break;
                                default:
                                    sDbg.WriteLine("Other event (BUS): " + ev.Structure.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            sDbg.WriteLine("Other element message: " + m.Structure.ToString()); 
                        }
                        break;
                    default: 
                        sDbg.WriteLine($"{m.Type} message received from {m.Src?.Name} ({oo?.Name}).");
                        break;
                }
            }
            catch (Exception ex)
            {
                sDbg.WriteLine("Failed to get message: " + ex.Message); 
            }

            //sDbg.WriteLine("msg refcount: " + m.Refcount);
            //m.Dispose();
            //oo?.Dispose();  
        }

        private void Bus_SyncMessage(object o, SyncMessageArgs args)
        {
            if (Gst.Video.Global.IsVideoOverlayPrepareWindowHandleMessage(args.Message))
            //if(false)
            {
                Gst.Video.VideoOverlayAdapter adapter;
                Element src = args.Message.Src as Element;

                Element overlay;

                if (src is Gst.Bin)
                {
                    overlay = (src as Gst.Bin).GetByInterface(Gst.Video.VideoOverlayAdapter.GType);
                }
                else 
                    overlay = src; 
                
                adapter = new Gst.Video.VideoOverlayAdapter(overlay.Handle);
                adapter.WindowHandle = hPnl;
                adapter.HandleEvents(true);
                sDbg.WriteLine("Bus Sync complete! ");
                DumpGraph(mpipe, "AfterSync.dot");

                //src.Dispose();
                //overlay.Dispose(); 
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetGst();
            chkLatencyEnable2.Checked = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.launchDict = launchDict;
            Settings.Default.lastIdx = cmbLaunch.SelectedIndex; 

            Settings.Default.Save(); 

            if (mpipe != null)
            {
                mpipe.SetState(State.Null);
            }

            if (glibLoop != null)
            {
                glibLoop.Quit(); 
            }

            Gst.Global.Deinit(); 
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                mpipe?.Dispose();
                mpipe = null; 
            }
            base.Dispose(disposing);
        }

        public static void DumpGraph(Pipeline pipe, string fname)
        {
            try
            {
                string path = @"C:\gstreamer\dotfiles";
                string dotdata = Gst.Debug.BinToDotData(pipe, DebugGraphDetails.All);
                if (!string.IsNullOrWhiteSpace(dotdata) && Directory.Exists(path))
                    File.WriteAllText(Path.Combine(path, "playground_" + fname), dotdata);
            }
            catch (Exception ex)
            {
                sDbg.WriteLine("Couldn't dump graph: " + ex.Message);
            }
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            var sink = mpipe?.GetByName("prevSink") as Element;
            if (sink != null)
            {
                
                sDbg.WriteLine("Sink got: " + sink.Name);
                VideoSink actualSink = sink as VideoSink;

                if (sink is Bin)
                {
                    actualSink = (sink as Bin).GetChildByIndex(0) as VideoSink;
                }

                //var sample = (mpipe.GetByName("vsink") as VideoSink).LastSample;
                //var jpgSample = Gst.Video.Global.VideoConvertSample(sample, Caps.FromString("image/jpeg"), 1000000000);

                var samp = actualSink.LastSample;
                var meta = samp.Buffer.GetMeta(VideoOverlayComposition.GType);
                //samp.Caps = Caps.FromString("video/x-raw,format=I420");
                //samp.MakeWritable(); 

                //var s = new Sample(samp.Buffer, Caps.FromString("video/x-raw"), samp.Segment, Structure.NewFromString(""));
                
                sDbg.WriteLine($"Sample size: {samp.Buffer.Size / 1024} KB"); 
                sDbg.WriteLine("Sample meta: " + meta.info.ToString());

                try
                {

                    if (samp != null)
                    {
                        var jpg2 = Gst.Video.Global.VideoConvertSample(samp, Caps.FromString("image/jpeg"), 1000000000);
                        //var jpg2 = myConvertSample(samp, Caps.FromString("image/jpeg"), (ulong)(2 * 1E9));
                        //var j3 = myConvertSample(s, Caps.FromString("image/jpeg"), (ulong)(2 * 1e9));

                        if (jpg2 != null)
                        {
                            Gst.Buffer imgBuf = jpg2.Buffer;

                            byte[] bytes = new byte[imgBuf.Size];
                            imgBuf.Extract(0, ref bytes);

                            // Create a new image, set the preview window, then save the JPG. 
                            MemoryStream ms = new MemoryStream(bytes);
                            Image img = Image.FromStream(ms);

                            pbxSnap.Image = img;
                            pbxSnap.Refresh(); 

                            ms.Dispose(); 
                            //img.Dispose();
                            imgBuf.Dispose();

                            toolTip1.SetToolTip(pbxSnap, "Last capture: " + System.DateTime.Now.ToString("HH:mm:ss"));
                        }
                    }
                }
                catch (GLib.GException gex)
                {
                    
                    sDbg.WriteLine("GException: " + gex.Message);
                }
                catch (Exception ex)
                {
                    sDbg.WriteLine("Error converting sample. " + ex.Message); 
                }
                finally
                {
                    //samp.Buffer.Unlock(LockFlags.Read);
                    samp.Dispose(); 
                }
            }
        }

        [Flags]
        public enum FactoryTypes {
            GST_ELEMENT_FACTORY_TYPE_ENCODER       =  1 << 1,
            GST_ELEMENT_FACTORY_TYPE_MEDIA_VIDEO   =  1 << 49,
            GST_ELEMENT_FACTORY_TYPE_MEDIA_AUDIO   =  1 << 50,
            GST_ELEMENT_FACTORY_TYPE_MEDIA_IMAGE   =  1 << 51,
            GST_ELEMENT_FACTORY_TYPE_MEDIA_SUBTITLE=  1 << 52,
            GST_ELEMENT_FACTORY_TYPE_MEDIA_METADATA=  1 << 53,
            GST_ELEMENT_FACTORY_TYPE_ANY           = (1 << 49) - 1, 
            GST_ELEMENT_FACTORY_TYPE_VIDEO_ENCODER = (GST_ELEMENT_FACTORY_TYPE_ENCODER | GST_ELEMENT_FACTORY_TYPE_MEDIA_VIDEO | GST_ELEMENT_FACTORY_TYPE_MEDIA_IMAGE),
            GST_ELEMENT_FACTORY_TYPE_AUDIO_ENCODER = GST_ELEMENT_FACTORY_TYPE_ENCODER | GST_ELEMENT_FACTORY_TYPE_MEDIA_AUDIO, 
        }

        private void pnlGst_DoubleClick(object sender, EventArgs e)
        {
            sDbg.WriteLine("Double click."); 
        }

        private void pnlGst_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            sDbg.WriteLine("Mouse double click."); 
        }

        private void cmbLaunch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = cmbLaunch.SelectedIndex;
            if (idx >= 0)
            {
                string key = cmbLaunch.Items[idx] as string;
                if (launchDict.ContainsKey(key))
                {
                    txtPipe.Text = launchDict[key];
                }
                else
                {
                    launchDict.Add(key, txtPipe.Text);
                }
            }
            else
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int idx = cmbLaunch.SelectedIndex;
            string key = string.Empty;

            if (idx >= 0)
            {
                key = cmbLaunch.Items[idx] as string;
            }
            else
            {
                key = cmbLaunch.Text;
            }

            if (launchDict.ContainsKey(key))
            {
                launchDict[key] = txtPipe.Text;
            }
            else
            {
                launchDict.Add(key, txtPipe.Text);
                cmbLaunch.Items.Add(key);
            }

            Settings.Default.launchDict = launchDict;
            Settings.Default.lastIdx = cmbLaunch.SelectedIndex;

            Settings.Default.Save();
        }

        private void cmbLaunch_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyCode;
            var item = cmbLaunch.SelectedItem as string;
            var idx = cmbLaunch.SelectedIndex;
            if (idx < 0 || item == null) return;

            switch (k)
            {
                case Keys.Delete:
                    {
                        try
                        {
                            if (launchDict.ContainsKey(item))
                            {
                                launchDict.Remove(item);
                                cmbLaunch.Items.Remove(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error removing key " + item + " " + ex.Message);
                        }
                    }
                    break;
                case Keys.Enter:
                    {
                        var txt = cmbLaunch.Text; 
                        if (txt != item && !string.IsNullOrWhiteSpace(item) && idx >= 0)
                        {
                            launchDict.ChangeKey(item, txt);
                            cmbLaunch.Items[idx] = txt;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public Sample myConvertSample(Sample sample, Caps to_caps, ulong timeout)
        {
            Element appSrc, vidConv, jpegEnc, appSink, dl;
            //Element vidScale;

            Sample ret = null;

            convPipe = new Pipeline();

            appSrc = ElementFactory.Make("appsrc");
            dl = ElementFactory.Make("d3d11download"); 
            vidConv = ElementFactory.Make("videoconvert");
            jpegEnc = ElementFactory.Make("jpegenc");
            appSink = ElementFactory.Make("appsink");

            sDbg.WriteLine("Converting to: " + to_caps.ToString());
            sDbg.WriteLine("\tFrom: " + sample.Caps);

            Gst.App.AppSrc src = appSrc as Gst.App.AppSrc;
            Gst.App.AppSink sink = appSink as Gst.App.AppSink;

            src.EmitSignals = true;
            sink.EmitSignals = true;
            sink["caps"] = to_caps;
            src["caps"] = sample.Caps;

            convPipe.Add(appSrc, dl, vidConv, jpegEnc, appSink);
            jpegEnc["idct-method"] = 1;

            if (!Element.Link(appSrc, dl, vidConv, jpegEnc, appSink)) 
            {
                sDbg.WriteLine("Failed to link.");
                return null; 
            }

            var r = convPipe.SetState(State.Paused);
            if (r == StateChangeReturn.Failure)
            {
                sDbg.WriteLine("Failed to change state: " + r);
                return ret;
            }
            DumpGraph(convPipe, "jpgConv.dot");

            var fr = src.PushBuffer(sample.Buffer);
            if (fr != FlowReturn.Ok)
            {
                sDbg.WriteLine("Failed to push buffer! " + fr.ToString());
                return ret;
            }

            var msg = convPipe.Bus.TimedPopFiltered(timeout, MessageType.Error | MessageType.AsyncDone);

            do
            {
                if (msg != null)
                {
                    switch (msg.Type)
                    {
                        case MessageType.AsyncDone:
                            var res = sink.PullPreroll();
                            //sink.PullSample();
                            if (res != null)
                            {
                                sDbg.WriteLine("Conversion successful: " + res.ToString());
                                ret = res;
                                DumpGraph(convPipe, "convAsyncDone.dot");
                            }
                            else
                            {
                                sDbg.WriteLine("Failed...");
                            }
                            break;
                        case MessageType.Error:
                            msg.ParseError(out GLib.GException err, out string dd);
                            var deets = msg.ParseErrorDetails();

                            sDbg.WriteLine("Failed to convert. " + err + dd);
                            DumpGraph(convPipe, "convError.dot");
                            break;
                        default:
                            sDbg.WriteLine("Other message: " + msg.Type);
                            break;
                    }
                }
                else
                {
                    sDbg.WriteLine("Timeout during conversion");
                }
                msg = convPipe.Bus.TimedPop((ulong)1E8);
            }
            while (msg != null);

            convPipe.SetState(State.Null);
            return ret;
        }

        private void btnDumpGraph_Click(object sender, EventArgs e)
        {
            DumpGraph(mpipe, "manualDump_" + System.DateTime.Now.ToString("s").Replace(":", "") + ".dot"); 
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            sDbg.WriteLine("Panel1 double-click."); 
        }

        private void txtPipe_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyCode; 
            if (k == Keys.Enter && e.Control)
            {
                e.SuppressKeyPress = true;
                btnSave.PerformClick();
                btnStart.PerformClick();
            }
            if (k == Keys.ControlKey || k == Keys.Control)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pnlGst_MouseMove(object sender, MouseEventArgs e)
        {
            lblSsCursor.Text = $"({e.Location.X},{e.Location.Y}) - [{e.Button}]";
            lblSsColor.Text = latencyPanel1.ObservedColor.ToString(); 
        }

        private void chkLatencyEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkLatencyEnable2.Checked)
                latencyPanel1.Stop();
            else
                latencyPanel1.Start();
        }

        private void pnlGst_MouseDown(object sender, MouseEventArgs e)
        {
            sDbg.WriteLine("Mouse down: " + e.Button);
        }

        private void pnlGst_MouseClick(object sender, MouseEventArgs e)
        {
            sDbg.WriteLine("Mouse click: " + e.Button); 
        }

        public static string SanitizeLaunchLine(string ll)
        {
            ll = ll.Trim();
            string exePattern = @"gst-launch-1.0(\.exe)?";
            string pattern = @"(\\\s+)|[\r\n]+";
            ll = Regex.Replace(ll, exePattern, ""); 
            ll = Regex.Replace(ll, pattern, " ");
            return ll;
        }

        private void listDetectedPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form() { Size = new Size(500, 400), StartPosition = FormStartPosition.CenterParent};
            Panel p = new Panel() { AutoScroll = true, Dock = DockStyle.Fill, Padding = new Padding(10) };
            TextBox t = new TextBox() { 
                Dock = DockStyle.Fill, 
                Multiline = true, 
                ReadOnly = true, 
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(15) }; 
            f.Controls.Add(p);
            p.Controls.Add(t);
            string str = ""; 

            if (Gst.Global.IsInitialized)
            {
                var elts = ElementFactory.ListGetElements((ulong)FactoryTypes.GST_ELEMENT_FACTORY_TYPE_ANY, Rank.None);
                str = string.Join(", ", elts.Select(elt => elt.Name));
                f.Text = $"All {elts.Length} factories";
            }
            else
            {
                str = "Please first initialize GStreamer by running a launch line.";
                f.Text = "Uninitialized"; 
            }
            

            t.Text = str;
            f.ShowDialog(this);

            t.Dispose();
            p.Dispose();
            f.Dispose();
        }

        private void exportAllLaunchLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var s = new SaveFileDialog())
            {
                s.InitialDirectory = Directory.GetCurrentDirectory();
                s.DefaultExt = ".txt";
                s.FileName = "my-launch-lines.txt";

                if (s.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string k in launchDict.Keys)
                        {
                            sb.AppendLine(k + launchDelim + " " + SanitizeLaunchLine(launchDict[k]));
                        }

                        using (var fs = new FileStream(s.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.Write(sb.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        sDbg.WriteLine("Failed to save file! " + ex.Message); 
                    }
                }
            }
        }
        private void importLaunchLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog fd = new OpenFileDialog())
                {
                    fd.InitialDirectory = Directory.GetCurrentDirectory();
                    fd.DefaultExt = ".txt";
                    fd.FileName = "my-launch-lines.txt";

                    if (fd.ShowDialog(this) == DialogResult.OK)
                    {
                        foreach (var line in File.ReadLines(fd.FileName))
                        {
                            var split = line.Split(launchDelim.ToCharArray(), 2, StringSplitOptions.None);
                            var key = split[0];
                            var val = split[1].TrimStart();

                            if (!launchDict.ContainsKey(key))
                            {
                                launchDict.Add(key, val);
                                cmbLaunch.Items.Add(key);
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file: " + ex.Message); 
            }
        }

        private void nudFlashOnTime_ValueChanged(object sender, EventArgs e)
        {
            latencyPanel1.FlashOnTime = (uint)nudFlashOnTime.Value;
        }

        private void latencyTesterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Use this tool to test a video camera's glass-to-glass (realtime) latency. " + Environment.NewLine);
            sb.AppendLine("0. Start a pipeline/launchline for a video camera in your posession.");
            sb.AppendLine("1. Point camera at your screen and move the window so it can be seen by the camera.");
            sb.AppendLine("2. Move cursor to hover over an area that changes color on the streamed image of the stream (first nested video image).");
            sb.AppendLine("3. Observe reported latency on the right side of the app.");

            var ret = MessageBox.Show(this, sb.ToString(), "Latency test", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            chkLatencyEnable2.Checked = DialogResult.OK == ret;

        }

        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Process.Start("cmd.exe", @"/k cd " + Environment.GetEnvironmentVariable(Program.gstPathVar));
        }


        public static ElementFactory[] GetElementFactories(Caps caps, FactoryTypes types, PadDirection dir = PadDirection.Src)
        {
            var elts = ElementFactory.ListGetElements((ulong)
                (FactoryTypes.GST_ELEMENT_FACTORY_TYPE_ENCODER | FactoryTypes.GST_ELEMENT_FACTORY_TYPE_MEDIA_IMAGE), Rank.None);
            GLib.List l = new GLib.List(elts, typeof(Element), true, true);
            var filtered = ElementFactory.ListFilter(l, caps, dir, false);

            sDbg.Write($"Compatible with {caps} encoder factories: ");

            foreach (var f in filtered)
            {
                sDbg.Write(f.Name + ", ");
            }
            sDbg.WriteLine("");
            l.Dispose();

            return filtered;
        }
    }

}
