using System;
using System.IO;
using System.Windows.Forms;
using Plugin.ProxyServer.Plugins;
using Plugin.ProxyServer.UI;
using SAL.Windows;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer
{
	public partial class PanelProxyLog : UserControl
	{
		private PluginHost PluginInstance { get => (PluginHost)this.Window.Plugin; }

		private IWindow Window { get => (IWindow)base.Parent; }

		public PanelProxyLog()
		{
			this.InitializeComponent();
			lvData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			splitMain.Panel2Collapsed = true;
		}

		protected override void OnCreateControl()
		{
			this.lvData.GroupType = this.PluginInstance.Settings.LogGroup;
			tsmiDataGroup.DropDownItems.AddRange(Array.ConvertAll<MessageGroupType, ToolStripMenuItem>((MessageGroupType[])Enum.GetValues(typeof(MessageGroupType)), delegate(MessageGroupType type) { return new ToolStripMenuItem(type.ToString()) { Tag = type, Checked = this.PluginInstance.Settings.LogGroup == type, }; }));
			if(this.PluginInstance.PluginCompiler.PluginCompiler == null)
			{
				tsddlPluginCompiler.Enabled = false;
				tsddlPluginCompiler.ToolTipText = $"Plugin ID={PluginCompilerWrapper.Name} not installed";
			}
			if(this.PluginInstance.PluginCryptoUI.PluginCryptoUI == null)
			{
				tsddlPluginCryptoUI.Enabled = false;
				tsddlPluginCryptoUI.ToolTipText = $"Plugin ID={PluginCryptoUIWrapper.Name} not installed";
			}

			this.ProxyWrapper_ProxyStateChanged(this, EventArgs.Empty);//Меняю кнопку
			this.Window.Caption = "Proxy Log";
			this.Window.SetDockAreas(DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom);

			this.Window.Closed += this.Window_Closed;

			this.PluginInstance.Settings.PropertyChanged += this.Settings_PropertyChanged;
			this.PluginInstance.ProxyWrapper.ProxyStateChanged += this.ProxyWrapper_ProxyStateChanged;
			this.PluginInstance.ProxyWrapper.Request += this.Proxy_Request;
			this.PluginInstance.ProxyWrapper.Response += this.Proxy_Response;
			base.OnCreateControl();
		}

		private void Window_Closed(Object sender, EventArgs e)
		{
			this.PluginInstance.Settings.PropertyChanged -= this.Settings_PropertyChanged;
			this.PluginInstance.ProxyWrapper.ProxyStateChanged -= this.ProxyWrapper_ProxyStateChanged;
			this.PluginInstance.ProxyWrapper.Request -= this.Proxy_Request;
			this.PluginInstance.ProxyWrapper.Response -= this.Proxy_Response;
		}

		private void Settings_PropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
			case nameof(PluginSettings.LogGroup):
				lvData.GroupType = this.PluginInstance.Settings.LogGroup;
				foreach(ToolStripMenuItem groupItem in tsmiDataGroup.DropDownItems)
					groupItem.Checked = ((MessageGroupType)groupItem.Tag) == this.PluginInstance.Settings.LogGroup;
				break;
			}
		}

		private void splitMain_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if(splitMain.SplitterRectangle.Contains(e.Location))
			{
				splitMain.Panel2Collapsed = true;
				lvData.Focus();
			}
		}

		private void lvData_DoubleClick(Object sender, EventArgs e)
		{
			MessageListViewItem item = lvData.SelectedItem;
			if(item != null)
			{
				splitMain.Panel2Collapsed = false;
				this.lvData_SelectedIndexChanged(sender, e);
			}
		}

		private void lvData_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if(!splitMain.Panel2Collapsed)
			{
				MessageListViewItem item = lvData.SelectedItem;
				if(item == null)
					splitMain.Panel2Collapsed = true;
				else
				{
					propHeaders.SelectedObject = item.Tag;
					txtRequest.Text = item.Tag.Request?.BodyString;
					txtResponse.Text = item.Tag.Response?.BodyString;

					if(txtRequest.Text.Length == 0)
						tabPayload.TabPages.Remove(tabRequest);
					else if(!tabPayload.TabPages.Contains(tabRequest))
						tabPayload.TabPages.Insert(1, tabRequest);

					if(txtResponse.Text.Length == 0)
						tabPayload.TabPages.Remove(tabResponse);
					else if(!tabPayload.TabPages.Contains(tabResponse))
						tabPayload.TabPages.Add(tabResponse);
				}
			}
		}

		private void ProxyWrapper_ProxyStateChanged(Object sender, EventArgs e)
		{
			tsbnProxyChangeState.Checked = this.PluginInstance.IsStarted;
			tsbnProxyChangeState.Text = this.PluginInstance.IsStarted
				? "Stop proxy"
				: "Start proxy";
		}

		private void Proxy_Request(Object sender, SessionEventArgs e)
			=> lvData.AddRequest(e);

		private void Proxy_Response(Object sender, SessionEventArgs e)
			=> lvData.AddResponse(e);

		private void tsMain_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsbnProxyChangeState)
			{
				this.PluginInstance.Settings.IsStartProxy = !tsbnProxyChangeState.Checked;
				this.PluginInstance.Host.Plugins.Settings(this.PluginInstance).SaveAssemblyParameters();
			}
		}

		private void cmsData_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiDataClear)
			{
				/*if(lvData.SelectedItems.Count > 0)
					while(lvData.SelectedItems.Count > 0)
						lvData.SelectedItems[0].Remove();
				else*/
				lvData.ClearCacheAndList();
			}
		}

		private void tsmiDataGroup_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
			=> this.PluginInstance.Settings.LogGroup = (MessageGroupType)e.ClickedItem.Tag;

		private void tsddlPluginCompiler_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiPluginCompilerRequest)
				this.PluginInstance.PluginCompiler.CreateCompilerWindow(Constants.InterceptRequest, null);
			else if(e.ClickedItem == tsmiPluginCompilerResponse)
				this.PluginInstance.PluginCompiler.CreateCompilerWindow(Constants.InterceptResponse, null);
		}

		private void tsddlPluginCryptoUI_DropDownOpening(Object sender, EventArgs e)
		{
			Boolean isCertExists=this.PluginInstance.Settings.GetRootCertificate() != null;
			tsmiPluginCryptoUISave.Enabled = isCertExists;
			tsmiPluginCryptoUIDelete.Enabled = isCertExists;
		}

		private void tsddlPluginCryptoUI_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiPluginCryptoUIGenerate)
			{
				String subject = String.Format("CN=Plugin.ProxyServer, C=RU, O=Danila Korablin");
				String password = this.PluginInstance.Settings.GetRootCertificatePassword();
				Byte[] payload = this.PluginInstance.PluginCryptoUI.GenerateCertificate(subject, password, 2048, "SHA256WITHRSA", DateTime.Today, DateTime.Today.AddYears(1));
				this.PluginInstance.Settings.SetRootCertificate(payload);
			} else if(e.ClickedItem == tsmiPluginCryptoUISave)
			{
				String defaultExt;
				String description;
				if(String.IsNullOrEmpty(this.PluginInstance.Settings.RootCertificatePassword))
				{
					defaultExt = "crt";
					description = "Certificate file (*.crt)|*.crt";
				} else
				{
					defaultExt = "pfx";
					description = "Personal Information Exchange file (*.pfx)|*.pfx";
				}

				Byte[] payload = this.PluginInstance.Settings.GetRootCertificate();
				using(SaveFileDialog dlg = new SaveFileDialog() { OverwritePrompt = true, AddExtension = true, DefaultExt = defaultExt, Filter = description })
					if(dlg.ShowDialog() == DialogResult.OK)
						File.WriteAllBytes(dlg.FileName, payload);
			} else if(e.ClickedItem == tsmiPluginCryptoUIDelete)
			{
				if(MessageBox.Show("Are you shure you want to delete root certificate?", "Certificate", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					this.PluginInstance.Settings.DeleteRootCertificate();
			}
		}
	}
}