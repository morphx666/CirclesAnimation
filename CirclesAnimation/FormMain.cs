using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CirclesAnimation {
    public partial class FormMain : Form {
        private double angle = 0.0f;
        private const int cr = 10;
        private const double ToRad = Math.PI / 180.0;
        private const double ToDeg = 180.0 / Math.PI;

        private float n = 1;

        private bool showLines = true;
        private bool showMainPoint = true;
        private bool showCircle = true;
        private bool showDandingCircles = true;
        private bool showHelp = true;

        public FormMain() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);

            this.KeyDown += (object s, KeyEventArgs e) => {
                switch(e.KeyCode) {
                    case Keys.F1:
                        showHelp = !showHelp;
                        break;
                    case Keys.F11:
                        showLines = !showLines;
                        break;
                    case Keys.Add:
                        n += 1;
                        break;
                    case Keys.Subtract:
                        n -= 1;
                        break;
                    case Keys.Space:
                        showCircle = !showCircle;
                        break;
                    case Keys.Enter:
                        showMainPoint = !showMainPoint;
                        break;
                    case Keys.F12:
                        showDandingCircles = !showDandingCircles;
                        break;
                }
            };

            Task.Run(() => {
                while(true) {
                    this.Invalidate();
                    Thread.Sleep(33);
                }
            });
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;

            float w2 = this.DisplayRectangle.Width / 2.0f;
            float h2 = this.DisplayRectangle.Height / 2.0f;
            float cr2 = cr / 2.0f;

            float r = (w2 < h2 ? w2 : h2) - cr2;

            float a;
            float x;
            float y;

            if(showHelp) {
                int lh = this.Font.Height;
                g.DrawString($"[F1]     = Toggle Help\t\t{(showHelp ? "ON" : "OFF")}", this.Font, Brushes.White, 0, 0 * lh);
                g.DrawString($"[+/-]    = Add/Remove Lines\t\t{n}", this.Font, Brushes.White, 0, 1 * lh);
                g.DrawString($"[SPACE]  = Toggle Circle\t\t{(showCircle ? "ON" : "OFF")}", this.Font, Brushes.White, 0, 2 * lh);
                g.DrawString($"[ENTER]  = Toggle Main Point\t\t{(showMainPoint ? "ON" : "OFF")}", this.Font, Brushes.White, 0, 3 * lh);
                g.DrawString($"[F11]    = Toggle Lines\t\t{(showLines ? "ON" : "OFF")}", this.Font, Brushes.White, 0, 4 * lh);
                g.DrawString($"[F12]    = Toggle Dancing Circles\t{(showDandingCircles ? "ON" : "OFF")}", this.Font, Brushes.White, 0, 5 * lh);
            }

            if(showLines) {
                for(int i = 0; i < n; i++) {
                    a = (float)((i / n) * Math.PI + Math.PI / 2.0);
                    float x1 = (float)(w2 + r * Math.Cos(a));
                    float y1 = (float)(h2 - r * Math.Sin(a));

                    float x2 = (float)(w2 + r * Math.Cos(a + Math.PI));
                    float y2 = (float)(h2 - r * Math.Sin(a + Math.PI));

                    g.DrawLine(Pens.DarkGray, x1, y1, x2, y2);
                }
            }

            g.TranslateTransform(w2, h2);

            if(showMainPoint) {
                x = (float)(r * Math.Cos(angle));
                y = (float)(-r * Math.Sin(angle));
                g.FillEllipse(Brushes.Yellow, x - cr2, y - cr2, cr, cr);
            }

            if(showCircle) g.DrawEllipse(Pens.Green, -r, -r, 2 * r, 2 * r);

            if(showDandingCircles) {
                for(int i = 0; i < n; i++) {
                    a = (float)(Math.PI * (i / n));
                    y = (float)(-r * Math.Sin(angle + a));

                    g.RotateTransform((float)(a * ToDeg));
                    g.FillEllipse(Brushes.White, 0 - cr2, y - cr2, cr, cr);
                    g.RotateTransform(-(float)(a * ToDeg));
                }
            }

            angle += 2.5 * ToRad;
        }
    }
}