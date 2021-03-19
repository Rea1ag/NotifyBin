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
		Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
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
		RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\NotifyBin");
		//Очистка корзины
		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				uint IsSuccess = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHRB_NOCONFIRMATION);//Очистка корзины
				GetNotifyData();
				MessageBox.Show("Корзина успешно очищена!", "Очистка корзины", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: Корзина не была очищенна" + ex.Message, "Очистка корзины", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
		//Закрыть программму
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Dispose();
			Application.Exit();
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
		//О программе
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Программа для очистки корзины. ", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

	}
}
