using Microsoft.Win32;
using NotifyBin.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotifyBin
{
	public partial class MainForm : Form
	{
		//Автозапуск приложения
		readonly RegistryKey myKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
		//Папка в реестре где хранятся параметры программы
		readonly RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\NotifyBin");
		enum RecycleFlags : uint
		{
			SHRB_NOCONFIRMATION = 0x00000001, // Don't ask confirmation
			SHRB_NOPROGRESSUI = 0x00000002, // Don't show any windows dialog
			SHRB_NOSOUND = 0x00000004 // Don't make sound, ninja mode enabled :v
		}
		[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
		static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

		public MainForm()
		{
			InitializeComponent();
		}

		//Не показывать программу в Alt-tab
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams pm = base.CreateParams;
				pm.ExStyle |= 0x80;
				return pm;
			}
		}
		//Очистка корзины
		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				uint IsSuccess = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHRB_NOCONFIRMATION);//Очистка корзины
				GetNotifyData();
				notify.BalloonTipTitle = "Notify Bin";
				notify.BalloonTipText = "Recycle Bin is successfully cleared!";
				notify.BalloonTipIcon = ToolTipIcon.Info;
				notify.ShowBalloonTip(1000);
			}
			catch (Exception ex)
			{
				notify.BalloonTipTitle = "Notify Bin";
				notify.BalloonTipText = "Recycle Bin has not been cleared!";
				notify.BalloonTipIcon = ToolTipIcon.Error;
				notify.ShowBalloonTip(1000);
				
			}
		}
		//Открываем папку корзины
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("explorer", "shell:RecycleBinFolder"); // Открыть корзину
		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			//Делаем запись в реестр и сразу проверяем
			if (Convert.ToInt32(key.GetValue("Autostart")) == 1)
			{
				onToolStripMenuItem.Checked = true;
				offToolStripMenuItem.Checked = false;
			}
			else
			{
				key.SetValue("Autostart", 0);
				onToolStripMenuItem.Checked = false;
				offToolStripMenuItem.Checked = true;
			}
			GetNotifyData();
			timer.Start();

			try
			{
				var k = key.GetValue("DoubleClickAction");
				switch (k)
				{
					case "Open":
						openDoubleClickAction.Checked = true;
						clearDoubleClickAction.Checked = false;
						break;
					case "Clear":
						openDoubleClickAction.Checked = false;
						clearDoubleClickAction.Checked = true;
						break;
					default:
						key.SetValue("DoubleClickAction", "Open");
						openDoubleClickAction.Checked = true;
						clearDoubleClickAction.Checked = false;
						break;
				}
			}
			catch
			{
				key.SetValue("DoubleClickAction", "Open");
			}
		}
		//Раз в минуту обновляем, информацию о корзине
		private void timer_Tick(object sender, EventArgs e)
		{
			GetNotifyData();
		}
		//Размер и количество файлов при наведении на значек в строке уведомлений
		private void GetNotifyData()
		{
			GetBinData sizebin = new GetBinData();
			sizebin.GetSize();
			notify.Text = "Notify Bin v1.00\n\n" + sizebin._num_items + "\n" + sizebin._file_size;
			if(sizebin.cleanstatus == true)
			{
			notify.Icon = Resources.OkIcon;
			}
			else { notify.Icon = Resources.NotOkIcon; }
		}
		//Включить автозапуск при старте Windows
		private void onToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (myKey.GetValue("NotifyBin").ToString() != Application.ExecutablePath)
					myKey.SetValue("NotifyBin", Application.ExecutablePath);
			}
			catch (NullReferenceException ex)
			{
				myKey.SetValue("NotifyBin", Application.ExecutablePath);
			}
			key.SetValue("Autostart", 1);
			onToolStripMenuItem.Checked = true;
			offToolStripMenuItem.Checked = false;
		}
		//Выключить автозапуск при старте Windows
		public void offToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (myKey.GetValue("NotifyBin").ToString() == Application.ExecutablePath)
					myKey.DeleteValue("NotifyBin");
			}
			catch (NullReferenceException ex)
			{
				myKey.DeleteValue("NotifyBin");
			}
			key.SetValue("Autostart", 0);
			onToolStripMenuItem.Checked = false;
			offToolStripMenuItem.Checked = true;
		}
		//Действие при двойном клике по иконке
		private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (key.GetValue("DoubleClickAction").ToString() == "Open")
			{
				Process.Start("explorer", "shell:RecycleBinFolder");

			}
			else
			{
				clearToolStripMenuItem_Click(null, null);
			}
		}
		//Очистить - Действие при двойном клике по иконке
		private void clearDoubleClickAction_Click(object sender, EventArgs e)
		{
			key.SetValue("DoubleClickAction", "Clear");
			clearDoubleClickAction.Checked = true;
			openDoubleClickAction.Checked = false;
		}
		//Открыть - Действие при двойном клике по иконке
		private void openDoubleClickAction_Click(object sender, EventArgs e)
		{
			key.SetValue("DoubleClickAction", "Open");
			clearDoubleClickAction.Checked = false;
			openDoubleClickAction.Checked = true;
		}
		//О программе
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Notify Bin v1.00\nRecycle bin for your Microsoft Windows system tray area. \n © 2021 by Realag \n github.com/Realags/NotifyBin ", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		//Закрыть программму
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Dispose();
			Application.Exit();
		}

	}
}
