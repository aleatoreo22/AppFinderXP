using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace WindowsApplication1
{
	/// <summary>
	/// Summary description for MainWindow.
	/// </summary>
	public class MainWindow : System.Windows.Forms.Form
	{
		#region Components
		private System.Windows.Forms.ListBox lsbPrograms;
		private System.Windows.Forms.TextBox txtInput;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		const int MAX_APPS = 5;

		Thread searchAppsThread;
		bool isTyping = false;
		bool isSearchingApps = false;
		ArrayList loadedApps = null;

		public MainWindow()
		{
			InitializeComponent();
			txtInput.Focus();
		}

		private void LaunchProgram(int index)
		{
			if(loadedApps == null || loadedApps.Count == 0)
				return;
			Process.Start(((AppDetails)loadedApps[index]).Path);
			Application.Exit();
		}

		public void SearchApps()
		{		
			isSearchingApps = true;
			while(isTyping) 
			{
				isTyping = false;
				Thread.Sleep(200);
			}
			AppFinder appFinder = new AppFinder(Environment.UserName);
			Console.WriteLine("Searching...");
			loadedApps = appFinder.SearchApps(txtInput.Text);
			Console.WriteLine(loadedApps.Count + " itens was founded...");
			lsbPrograms.Items.Clear();
			if(loadedApps != null && loadedApps.Count > 0)
			{
				foreach(AppDetails item in loadedApps)
				{
					lsbPrograms.Items.Add(item.Name);
					if(lsbPrograms.Items.Count >= MAX_APPS)
						break;
				}
				lsbPrograms.SelectedIndex = 0;
			}
			isSearchingApps = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtInput = new System.Windows.Forms.TextBox();
			this.lsbPrograms = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// txtInput
			// 
			this.txtInput.BackColor = System.Drawing.Color.Black;
			this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtInput.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.txtInput.Location = new System.Drawing.Point(8, 16);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(488, 29);
			this.txtInput.TabIndex = 0;
			this.txtInput.Text = "";
			this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
			this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
			// 
			// lsbPrograms
			// 
			this.lsbPrograms.BackColor = System.Drawing.Color.Black;
			this.lsbPrograms.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lsbPrograms.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.lsbPrograms.ItemHeight = 21;
			this.lsbPrograms.Location = new System.Drawing.Point(8, 48);
			this.lsbPrograms.Name = "lsbPrograms";
			this.lsbPrograms.Size = new System.Drawing.Size(488, 105);
			this.lsbPrograms.TabIndex = 1;
			this.lsbPrograms.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lsbPrograms_KeyDown);
			// 
			// MainWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(10, 22);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(504, 166);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lsbPrograms,
																		  this.txtInput});
			this.Font = new System.Drawing.Font("Cascadia Code", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);

		}
		#endregion

		#region Events

		private void txtInput_TextChanged(object sender, System.EventArgs e)
		{
			if(!isSearchingApps)
			{
				searchAppsThread = new Thread(new ThreadStart(SearchApps));
				searchAppsThread.Start();
			}
			isTyping = true;
		}

		private void txtInput_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Console.WriteLine(e.KeyCode.ToString());
			if(e.KeyCode ==  Keys.Down)
			{
				lsbPrograms.Focus();
				SendKeys.SendWait("{DOWN}");
			}
			else if(e.KeyCode == Keys.Return)
				LaunchProgram(lsbPrograms.SelectedIndex);
		}

		private void lsbPrograms_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
				LaunchProgram(lsbPrograms.SelectedIndex);
			else if(e.KeyCode.ToString().Length > 1)
				return;
			else
			{
				txtInput.Focus();
				SendKeys.SendWait(e.KeyCode.ToString().ToLower());
			}
		}

		#endregion
	}
}
