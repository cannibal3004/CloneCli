using System;
using System.Collections.Generic;
using System.IO;
//using System.Timers;
//using System.Linq;
namespace CloneCli
{
	class MainClass
	{
		private static List<string> Drives = new List<string>();
		private static List<string> Partitions = new List<string>();
		//private static List<MenuItem> MenuItems = new List<MenuItem>();
		public static ConsoleMenu MyMenu = new ConsoleMenu();
		private const string DevicePath = "/dev/";
		private static FileSystemWatcher watcher = new FileSystemWatcher(DevicePath)
		{ 
			Filter = "sd*", 
			IncludeSubdirectories = false, 
			NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName, 
			EnableRaisingEvents = false
		};
		//private static Timer menuDrawTime = new Timer(100) { AutoReset = true, Enabled = false};
		
		public static void Main (string[] args)
		{
			watcher.Created += DeviceAdded;
			watcher.Deleted += DeviceRemoved;
			//menuDrawTime.Elapsed += DrawMenu;
			watcher.EnableRaisingEvents = true;
			//menuDrawTime.Enabled = false;
			GetDrives();
			foreach (string partition in Partitions)
			{
				MyMenu.Add(new ConsoleMenuItem(partition));
			}
			MyMenu.Add(new ConsoleMenuItem("Exit",true));
//			MyMenu.Add(new ConsoleMenuItem("Disk1"));
//			MyMenu.Add(new ConsoleMenuItem("Copy drive"));
//			MyMenu.Add(new ConsoleMenuItem("Secure erase drive", true));
//			MyMenu.Add(new ConsoleMenuItem("Menu item 3"));
			var input = new ConsoleKeyInfo();//Console.ReadKey(true);
			while(input.Key != ConsoleKey.Q )
			{
				switch (input.Key) {
				case ConsoleKey.UpArrow:
					MyMenu.SelectPrev();
					break;
				case ConsoleKey.DownArrow:
					MyMenu.SelectNext();
					break;
				default:
					break;
				}
				MyMenu.Draw();
				input = Console.ReadKey(true);
			}
		}
		
		private static void DeviceAdded(object sender, FileSystemEventArgs e)
		{
			DeviceAdded(e.Name);
		}
		private static void DeviceAdded(string name)
		{
			switch (name.Length) {
			case 3:
				AddDrive(name);
				break;
			case 4:
				AddPartition(name);
				break;
			default:
				break;
			}
		}
		
		private static void DeviceRemoved(object sender, FileSystemEventArgs e)
		{
			DeviceRemoved(e.Name);
		}
		
		private static void DeviceRemoved(string name)
		{
			switch (name.Length)
			{
			case 3:
				RemoveDrive(name);
				break;
			case 4:
				RemovePartition(name);
				break;
			default:
				break;
			}
		}
		
		private static void AddDrive(string name)
		{
			Drives.Add(name);
		}
		
		private static void RemoveDrive(string name)
		{
			Drives.Remove(name);
		}
		
		private static void AddPartition(string name)
		{
			Partitions.Add(name);
		}
		
		private static void RemovePartition(string name)
		{
			Partitions.Remove(name);
		}
		
		private static void GetDrives()
		{
			string[] files = Directory.GetFiles(DevicePath, "sd*");
			foreach (string file in files)
			{
				var fileName = file.Substring(file.LastIndexOf("/")+1,file.Length-file.LastIndexOf("/")-1);
				DeviceAdded (fileName);
			}
		}
		
//		public static void DrawMenu(object sender, ElapsedEventArgs e)
//		{
//
//		}
	}
	
}
