using System;
using System.Globalization;
using System.Windows.Forms;

namespace FRLG_RSE_PID_to_Frame
{
    public partial class PidToFrame : Form
    {
        public PidToFrame()
        {
            InitializeComponent();

            _DataGridIV.Columns.Remove(secondColumn);
            _DataGridIV.Columns.Remove(delayColumn);
            delayColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            datePicker.Value = DateTime.Now;
            hourPicker.SelectedItem = "00";
            minutePicker.SelectedItem = "00";
        }

        private void glassButton1_Click(object sender, EventArgs e)
        {
            Search_III_Gen(initialSeedTextBox, frameTextBox, delayTextBox, _DataGridIII, label1, 9999999);
        }

        private void maskedTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_III_Gen(initialSeedTextBox, frameTextBox, delayTextBox, _DataGridIII, label1, 9999999);
            }
        }

        public void Search_III_Gen(MaskedTextBox _seed, MaskedTextBox _frame, MaskedTextBox _delay, DataGridView _dataGrid, Label _label, uint _maxFrame)
        {
            _label.Focus();

            if (_seed.Text == "" || _delay.Text == "" || _frame.Text == "")
            {
                MessageBox.Show("Please fill all the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _dataGrid.Rows.Clear();

                uint seed = uint.Parse(_seed.Text, NumberStyles.HexNumber);
                short delay = short.Parse(_delay.Text);
                uint frame = uint.Parse(_frame.Text);
                uint maxframes = _maxFrame;
                uint temp;
                
                for (uint i = 0; i < maxframes; i++)
                {
                    temp = 0x41c64e6d * seed + 0x00006073;

                    if ((temp >> 16 == 16384 || temp >> 16 == 32768 || temp >> 16 == 49152) && i - delay >= frame)
                    {
                        int n = _dataGrid.Rows.Add();

                        _dataGrid.Rows[n].Cells[0].Value = Convert.ToDecimal(i - delay);
                        _dataGrid.Rows[n].Cells[1].Value = seed.ToString("X");
                    }
                    seed = temp;
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_DataGridIII.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                Clipboard.SetDataObject(_DataGridIII.GetClipboardContent());
            }
            else
            {
                MessageBox.Show("Please select something", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void glassButton2_Click(object sender, EventArgs e)
        {
            if (DPradioButton.Checked == true)
            {
                Search_III_Gen(_InitialSeedTextBox, _FrameTextBox, _DelayTextBox, _DataGridIV, label8, 99999);
            }
            else
            {
                Search_IV_Gen();
            }
        }

        private void _DelayTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_III_Gen(_InitialSeedTextBox, _FrameTextBox, _DelayTextBox, _DataGridIV, label8, 99999);
            }
        }

        private void minutePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_IV_Gen();
            }
        }

        private void Search_IV_Gen()
        {
            label5.Focus();

            if (hourPicker.Text == "" || minutePicker.Text == "")
            {
                MessageBox.Show("Please fill all the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _DataGridIV.Rows.Clear();

                int day = datePicker.Value.Day;
                int month = datePicker.Value.Month;
                int year = datePicker.Value.Year;
                int minute = int.Parse(minutePicker.Text);
                int hour = int.Parse(hourPicker.Text);
                string ab = "", cd = "", efgh = "";
                int cgd;
                uint rusSeed = 0, temp = 0;
                string initSeed;

                for (int second = 0; second < 60; second++)
                {
                    for (int delay = -1400; delay <= -1000; delay++)
                    {
                        ab = ((month * day + minute + second) % 256).ToString("X2");
                        cd = hour.ToString("X2");
                        cgd = delay % 65536 + 1;
                        efgh = ((year + cgd) % 10000).ToString("X4");
                        initSeed = ab + cd + efgh;
                        rusSeed = uint.Parse(initSeed, NumberStyles.HexNumber);

                        for (int frame = 0; frame <= 100; frame++)
                        {
                            temp = 0x41c64e6d * rusSeed + 0x00006073;

                            if ((temp >> 16 == 16384 || temp >> 16 == 32768 || temp >> 16 == 49152) && frame >= 24)
                            {
                                int m = _DataGridIV.Rows.Add();
                                _DataGridIV.Rows[m].Cells[0].Value = frame;
                                _DataGridIV.Rows[m].Cells[1].Value = initSeed;
                                _DataGridIV.Rows[m].Cells[2].Value = delay + 2001;
                                _DataGridIV.Rows[m].Cells[3].Value = second;
                            }
                            rusSeed = temp;
                        }
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_DataGridIV.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                Clipboard.SetDataObject(_DataGridIV.GetClipboardContent());
            }
            else
            {
                MessageBox.Show("Please select something", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void hourPicker_TextChanged(object sender, EventArgs e)
        {
            if (hourPicker.Text != "" && int.Parse(hourPicker.Text) > 23)
            {
                hourPicker.Text = "23";
            }
            else if (hourPicker.Text.Length > 2)
            {
                hourPicker.Text = hourPicker.Text.Substring(hourPicker.Text.Length - 2, 2);
            }
        }

        private void minutePicker_TextChanged(object sender, EventArgs e)
        {
            if (minutePicker.Text != "" && int.Parse(minutePicker.Text) > 59)
            {
                minutePicker.Text = "59";
            }
            else if (minutePicker.Text.Length > 2)
            {
                minutePicker.Text = minutePicker.Text.Substring(minutePicker.Text.Length - 2, 2);
            }
        }

        private void PtHGSSradioButton_CheckedChanged(object sender, EventArgs e)
        {
            _DataGridIV.Rows.Clear();

            if (PtHGSSradioButton.Checked == true)
            {
                datePicker.Enabled = hourPicker.Enabled = minutePicker.Enabled = true;
                _InitialSeedTextBox.Enabled = _DelayTextBox.Enabled = false;
                seedColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                seedColumn.Width = 60;
                frameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                frameColumn.Width = 40;
                frameColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                _DataGridIV.Columns.Insert(2, delayColumn);
                _DataGridIV.Columns.Insert(3, secondColumn);

            }
            else
            {
                datePicker.Enabled = hourPicker.Enabled = minutePicker.Enabled = false;
                _InitialSeedTextBox.Enabled = _DelayTextBox.Enabled = true;
                seedColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                frameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                frameColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.NotSet;
                _DataGridIV.Columns.Remove(secondColumn);
                _DataGridIV.Columns.Remove(delayColumn);
            }
        }

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("New Order Of Breeding © \nAssembly studied by /u/zep715 \nDeveloped by Real.96 & Signum21",
                            "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}