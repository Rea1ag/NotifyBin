using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace NotifyBin
{
	class GetBinData
	{
		public string _cb_size;
		public string _file_size;
		public string _num_items;
		public bool cleanstatus;



		// Создает структуру, которая будет хранить информацию повторного запроса. 
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		public struct SHQUERYRBINFO
		{
			public Int32 cbSize;
			public UInt64 i64Size;
			public UInt64 i64NumItems;
		}
		// Вызов метода SHQueryRecycleBin () библиотеки shell32 для запроса размера файла и номера файла в корзине.
		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern int SHQueryRecycleBin([MarshalAs(UnmanagedType.LPTStr)]String pszRootPath,ref SHQUERYRBINFO pSHQueryRBInfo);
		public void GetSize()
		{
			SHQUERYRBINFO bb_Query = new SHQUERYRBINFO();
			bb_Query.cbSize = Marshal.SizeOf(bb_Query.GetType());
			SHQueryRecycleBin(null, ref bb_Query);
			//Смена иконки если корзина пуста.
			if (bb_Query.i64Size == 0)
			{ 
				cleanstatus = true; //Состояние корзины пуста или нет.
			}
			_cb_size = "CB Size  :  " + bb_Query.cbSize;
			//Вызов элемента структуры i64NumItems, который вернет номер файла в корзине.
			_num_items = bb_Query.i64NumItems + " files";
			//Вызов элемента структуры i64Size, который вернет размер корзины.
			_file_size = bb_Query.i64Size + " byte";  
			if (bb_Query.i64Size >= 1024)
			{
				_file_size = bb_Query.i64Size / 1024 + " KB";
				if (bb_Query.i64Size >= 1048576)
				{
					_file_size = bb_Query.i64Size / 1048576 + " MB";
					if (bb_Query.i64Size >= 1073741824)
					{
						string sizeGB = (Convert.ToDouble(bb_Query.i64Size) / 1073741824).ToString();
						_file_size = sizeGB.Substring(0, sizeGB.IndexOf(",") + 3) + " GB"; // Для дробного представления ГБ (1,65)
					}
				}
			}
		}
	}
}
