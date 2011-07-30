using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DashboardControls
{
    public partial class StatusLight : UserControl
    {
        private Pen forePen;

        public StatusLight()
        {
            InitializeComponent();
            forePen = new Pen(ForeColor);
        }

        private bool on;
        public bool On
        {
            get
            {
                return on;
            }
            set
            {
                on = value;
                Invalidate();
            }
        }

        private void StatusLight_Paint(object sender, PaintEventArgs e)
        {
            forePen.Color = ForeColor;
            
            Graphics g = e.Graphics;
            g.DrawEllipse(forePen, 0, 0, Size.Width, Size.Height);
        }
    }
}
