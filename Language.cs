using System.Collections.Generic;

namespace NotifyBin
{
	public static class Language
	{
		public static string lang;

		public static string Translate(string line)
		{
			string i = "";
			if (lang == "RU")
			{
				i = russian[line];
			}
			if (lang == "ENG")
			{
				i = english[line];
			}
			return i;
		}
		//ENG
		public static Dictionary<string, string> english = new Dictionary<string, string>//
		{
			//Controls
			["Open"] = "Open",
			["Clear"] = "Clear",
			["Settings"] = "Settings",
			["Autostart"] = "Autostart",
			["onAutostart"] = "On",
			["offAutostart"] = "Off",
			["IconDblClikAction"] = "Icon Double-Click Action",
			["openIconDblClikAction"] = "Open",
			["clearIconDblClikAction"] = "Clear",
			["Language"] = "Language",
			["engLanguage"] = "English",
			["ruLanguage"] = "Russian",
			["About"] = "About",
			["Exit"] = "Exit",
			//Numbers of files anf bytes (GetBinData)
			["files"] = " files",
			["byte"] = " byte",
			["KB"] = " KB",
			["MB"] = " MB",
			["GB"] = " GB",
			//Notify if recycle bin has been clearened or not
			["BinClear"] = "Recycle Bin is successfully cleared!",
			["BinNotClear"] = "Recycle Bin has not been cleared!",
		};
		//RUS
		public static Dictionary<string, string> russian = new Dictionary<string, string>
		{
			//Controls
			["Open"] = "Открыть",
			["Clear"] = "Очистить",
			["Settings"] = "Настройки",
				["Autostart"] = "Автозапуск",
					["onAutostart"] = "Вкл",
					["offAutostart"] = "Выкл",
				["IconDblClikAction"] = "Действие при двойном клике на иконку",
					["openIconDblClikAction"] = "Открыть",
					["clearIconDblClikAction"] = "Очистить",
				["Language"] = "Язык",
					["engLanguage"] = "Английский",
					["ruLanguage"] = "Русский",
			["About"] = "О программе",
			["Exit"] = "Выйти",
			//Numbers of files anf bytes (GetBinData)
			["files"] = " файлов",
			["byte"] = " байт",
			["KB"] = " КБ",
			["MB"] = " МБ",
			["GB"] = " ГБ",
			//Notify if recycle bin has been clearened or not
			["BinClear"] = "Корзина успешно очищена!",
			["BinNotClear"] = "Корзина не очищена! ",
		};
	}
}
