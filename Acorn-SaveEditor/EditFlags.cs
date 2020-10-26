using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acorn_SaveEditor
{
    public partial class EditFlags : Form
    {
        public EditFlags(ushort flag, string[] texts)
        {
            InitializeComponent();

            thisFlag = flag;
            CheckBox[] allCheckBoxes = this.Controls.OfType<CheckBox>().ToArray();
            for(int i = 0; i < allCheckBoxes.Length; i++)
            {
                allCheckBoxes[i].Checked = Convert.ToBoolean(thisFlag & (ushort)Math.Pow(2.0, i));
                if(i < texts.Length) { allCheckBoxes[i].Text = texts[i]; }
                else { 
                    allCheckBoxes[i].Visible = false;
                    this.Size = new Size(this.Size.Width, this.Size.Height - 23);
                    okButton.Location = new Point(okButton.Location.X, okButton.Location.Y - 23);
                }
            }
        }

        public ushort thisFlag = new ushort();

        private void okButton_Click(object sender, EventArgs e)
        {
            CheckBox[] allCheckBoxes = this.Controls.OfType<CheckBox>().ToArray();
            thisFlag = 0;
            foreach(CheckBox thisCheckBox in allCheckBoxes)
            {
                int flagValue = Convert.ToInt32(thisCheckBox.Name.Substring(4, thisCheckBox.Name.Length - 12), 16);
                thisFlag |= (ushort)((thisCheckBox.Checked) ? flagValue : 0);
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
