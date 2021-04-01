using System.ComponentModel;
using System.Windows.Forms;

namespace FRLG_RSE_PID_to_Frame.Controls
{
    public partial class _DataGridView : DataGridView
    {
        public _DataGridView()
        {
            InitializeComponent();
        }

        public _DataGridView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hti = HitTest(e.X, e.Y);

                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    if (!((Rows[hti.RowIndex])).Selected)
                    {
                        ClearSelection();

                        (Rows[hti.RowIndex].Cells[hti.ColumnIndex]).Selected = true;
                    }
                }
            }
            base.OnMouseDown(e);
        }
    }
}
