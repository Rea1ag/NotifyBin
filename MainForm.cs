﻿using Microsoft.Win32;
using NotifyBin.Properties;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
		//Начало - Не показывать программу в Alt-tab
		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr window, int index, int value);

		[DllImport("user32.dll")]
		private static extern int GetWindowLong(IntPtr window, int index);

		private const int GWL_EXSTYLE = -20;
		private const int WS_EX_TOOLWINDOW = 0x00000080;

		public static void HideFromAltTab(IntPtr Handle)
		{
			SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle,
				GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
		}
		//Конец - Не показывать программу в Alt-tab
		//Очистка корзины
		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				uint IsSuccess = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHRB_NOCONFIRMATION);//Очистка корзины
				GetNotifyData();
				notify.BalloonTipTitle = "Notify Bin";
				notify.BalloonTipText = Language.Translate("BinClear"); //"Recycle Bin is successfully cleared!";
				notify.BalloonTipIcon = ToolTipIcon.Info;
				notify.ShowBalloonTip(1000);
				
			}
			catch (Exception ex)
			{
				notify.BalloonTipTitle = "Notify Bin";
				notify.BalloonTipText = Language.Translate("BinNotClear"); //"Recycle Bin has not been cleared!";
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
			GetRegistryData();
			GetLanguage(); //Надо пробовать
			GetNotifyData();
			timer.Start();
			HideFromAltTab(this.Handle);//Запуск метода скрывает программу из alt-tab
		}

		//Локализация
		private void GetLanguage()
		{
			Language.lang = key.GetValue("Language").ToString();
			ControlsLang();

		}

		//Раз в минуту обновляем, информацию о корзине
		private void timer_Tick(object sender, EventArgs e)
		{
			GetNotifyData();
		}
		//Размер и количество файлов при наведении на значек в строке уведомлений
		private void GetNotifyData()
		{
			GetBinData databin = new GetBinData();
			//Сколько сейчас данных в корзине
			databin.GetSize();

			if (databin._file_sizeMB == 0 && databin._num_itemsMB == 0)
			{
			clearToolStripMenuItem.Enabled = false;
			}
			else { clearToolStripMenuItem.Enabled = true; }
			notify.Text = "Notify Bin v1.11\n\n" + databin._num_items + "\n" + databin._file_size;
			//Сколько всего данных в корзине
			double sum = databin.GetMaxSize();
			int iconpersent = 0;
			if (databin._file_sizeMB >= ((sum / 100) * 75) || databin._file_sizeMB <= ((sum / 100) * 75))
			{
				iconpersent = 75;
				if (databin._file_sizeMB <= ((sum / 100) * 50))
				{
					iconpersent = 50;
					if (databin._file_sizeMB <= ((sum / 100) * 25))
					{
						iconpersent = 25;

						if (databin._file_sizeMB == 0)
						{
							iconpersent = 0;
							if(databin._num_itemsMB >= 1)
							{
								iconpersent = 25;
							}
						}

					}
				}
			}
			if (databin._file_sizeMB >= ((sum / 100) * 99))
			{
				iconpersent = 100;
			}

			switch (iconpersent)
			{
				case 0:
					notify.Icon = Resources.OkIcon;//Иконка OK
					break;
				case 25:
					notify.Icon = Resources.NotIcon25;//Иконка 25%
					break;
				case 50:
					notify.Icon = Resources.NotIcon50;//Иконка 50%
					break;
				case 75:
					notify.Icon = Resources.NotIcon75;//Иконка 75%
					break;
				case 100:
					notify.Icon = Resources.NotIcon100;//Иконка 100%
					break;
				default:
					notify.Icon = Resources.DefaultIcon;
					break;
			}
		}
		//Все настройки записываються в реестр
		private void GetRegistryData()
		{
			try
			{
				var value = key.GetValue("Autostart");
				switch(value)
				{
					case 1:
						onToolStripMenuItem.Checked = true;
						offToolStripMenuItem.Checked = false;
						break;
					default :
						key.SetValue("Autostart", 0);
						onToolStripMenuItem.Checked = false;
						offToolStripMenuItem.Checked = true;
						break;
				}
			}
			catch
			{
				key.SetValue("Autostart", 0);
			}
			//Узнаем действие на дабл клик по иконке из реестра, если в реестре пусто создаем.
			//По умолчанию действие Открыть корзину
			try
			{
				var value = key.GetValue("DoubleClickAction");
				switch (value)
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
			//Определяем локализацию
			try
			{
				var value = key.GetValue("Language");
				switch (value)
				{
					case "ENG":
						englishToolStripMenuItem.Checked = true;
						russianToolStripMenuItem.Checked = false;
						break;
					case "RU":
						englishToolStripMenuItem.Checked = false;
						russianToolStripMenuItem.Checked = true;
						break;
					default:
						key.SetValue("Language", "ENG");
						englishToolStripMenuItem.Checked = true;
						russianToolStripMenuItem.Checked = false;
						break;
				}
			}
			catch
			{
				key.SetValue("Language", "ENG");
			}
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
			if (!Application.OpenForms.OfType<About>().Any())
				new About().Show();
		}
		//Закрыть программму
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Dispose();
			Application.Exit();
		}

		private void englishToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Language.lang = "ENG";
			key.SetValue("Language","ENG");
			ControlsLang();
			englishToolStripMenuItem.Checked = true;
			russianToolStripMenuItem.Checked = false;
		}

		private void russianToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Language.lang = "RU";
			key.SetValue("Language", "RU");
			ControlsLang();
			englishToolStripMenuItem.Checked = false;
			russianToolStripMenuItem.Checked = true;
		}
		//Интерфейс <-- Языковой словарь 
		//ENG
		void ControlsLang()
		{
			openToolStripMenuItem.Text = Language.Translate("Open");
			clearToolStripMenuItem.Text = Language.Translate("Clear");
			settingsToolStripMenuItem.Text = Language.Translate("Settings");
			autostartToolStripMenuItem.Text = Language.Translate("Autostart");
			onToolStripMenuItem.Text = Language.Translate("onAutostart");
			offToolStripMenuItem.Text = Language.Translate("offAutostart");
			DoubleClickAction.Text = Language.Translate("IconDblClikAction");
			openDoubleClickAction.Text = Language.Translate("openIconDblClikAction");
			clearDoubleClickAction.Text = Language.Translate("clearIconDblClikAction");
			languageToolStripMenuItem.Text = Language.Translate("Language");
			englishToolStripMenuItem.Text = Language.Translate("engLanguage");
			russianToolStripMenuItem.Text = Language.Translate("ruLanguage");
			aboutToolStripMenuItem.Text = Language.Translate("About");
			exitToolStripMenuItem.Text = Language.Translate("Exit");
		}

	}
}
