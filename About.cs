using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotifyBin
{
	public partial class About : Form
	{
		public About()
		{
			InitializeComponent();
			Descriptionlabel.Text = "Notify Bin v1.11\nRecycle bin for your Microsoft Windows system tray area. \n © 2021 by Realag \n github.com/Rea1ag/NotifyBin ";
		}
		private void Closebutton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
