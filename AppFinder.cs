using System;
using System.Collections;
using System.IO;

namespace WindowsApplication1
{
	/// <summary>
	/// Summary description for AppFinder.
	/// </summary>
	public class AppFinder
	{
		ArrayList paths;

		public AppFinder(string windowsUserName)
		{
			paths = new ArrayList();
			paths.Add(@"C:\Documents and Settings\All Users\Menu Iniciar");
			paths.Add(@"C:\Documents and Settings\" + windowsUserName + @"\Menu Iniciar");
		}

		public ArrayList SearchApps(string input)
		{
			input = input.ToLower();
			ArrayList apps = new ArrayList();
			foreach(string item in paths)
			{
				apps.AddRange(SearchApps(input, item));
			}
			return apps;
		}

		public ArrayList SearchApps(string input, string path)
		{
			ArrayList apps = new ArrayList();
			string[] files = Directory.GetFiles(path);
			foreach(string item in files)
			{
				if(item.ToLower().IndexOf(input) > 0)
				{
					string name = item.Substring(item.LastIndexOf(@"\") + 1);
					name = name.Substring(0, name.Length - 4);
					AppDetails app = new AppDetails();
					app.Name = name;
					app.Path = item;
					apps.Add(app);
				}
			}
			string[] paths = Directory.GetDirectories(path);
			foreach(string item in paths) 
			{								
				apps.AddRange(SearchApps(input, item));
			}
			return apps;
		}
	}
}
