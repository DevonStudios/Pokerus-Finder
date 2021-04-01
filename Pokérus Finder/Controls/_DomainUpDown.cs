using System;
using System.ComponentModel;
using System.Media;
using System.Windows.Forms;

namespace Pokerus_Finder.Controls
{
    public partial class _DomainUpDown : DomainUpDown
    {
        MyWindoHelper wh;

        public _DomainUpDown()
        {
            InitializeComponent();
        }

        public _DomainUpDown(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            wh = new MyWindoHelper(Controls[1]);
        }

        class MyWindoHelper : NativeWindow
        {
            Control c; //For future reference if needed

            public MyWindoHelper(Control control)
            {
                c = control;
                this.AssignHandle(c.Handle);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x0302)  //WM_PASTE
                {
                    SystemSounds.Beep.Play();
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            Select(0, 2);
            base.OnEnter(e);
        }
    }
}