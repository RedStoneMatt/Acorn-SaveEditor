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
    public partial class EditLocation : Form
    {
        public EditLocation(SaveCSLocation locationToEdit)
        {
            InitializeComponent();
            thisSaveCSLocation = locationToEdit;
            worldNumUpDown.Value = thisSaveCSLocation.world;
            sectionNumUpDown.Value = thisSaveCSLocation.section;
            nodeNumUpDown.Value = thisSaveCSLocation.node;
        }

        public SaveCSLocation thisSaveCSLocation;

        private void okButton_Click(object sender, EventArgs e)
        {
            thisSaveCSLocation.world = (byte)worldNumUpDown.Value;
            thisSaveCSLocation.section = (byte)sectionNumUpDown.Value;
            thisSaveCSLocation.node = (byte)nodeNumUpDown.Value;
            this.DialogResult = DialogResult.OK;
        }
    }
}
