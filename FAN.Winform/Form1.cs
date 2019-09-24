using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAN.Winform
{
    public partial class Form1 : Form
    {
        Thread t = null;
        Thread t2 = null;
        int counter = 0;
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            t = new Thread(() => {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(3000);
                        textBox1.Invoke(new EventHandler(delegate
                        {
                            textBox1.Text += this.counter++ + ",";
                        }));
                    }
                    catch (ThreadInterruptedException interruptEx)
                    {
                        
                    }
                    catch (ThreadAbortException abortEx)
                    {

                    }
                }                
            });
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //t2.Start();
            if (t.ThreadState!=ThreadState.Aborted&& t.ThreadState != ThreadState.Running)
            {
                t.Start();
            }
        }

        private void buttonSuspend_Click(object sender, EventArgs e)
        {
            if (t.ThreadState != ThreadState.Suspended)
            {
                t.Suspend();
            }
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {

            t.Resume();
        }

        private void buttonInterrupt_Click(object sender, EventArgs e)
        {
            //中断线程，抛出一个异常，相当于continue
            t.Interrupt();
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            //中止线程，抛出一个异常并结束线程，相当于break
            t.Abort();
        }

    }
}
