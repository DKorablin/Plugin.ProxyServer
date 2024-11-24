using System;
using System.Windows.Forms;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer.UI
{
	internal class MessageListViewItem : ListViewItem
	{
		public new MessageListView ListView { get => (MessageListView)base.ListView; }
		public new UiProxyDto Tag
		{
			get => (UiProxyDto)base.Tag;
			private set => base.Tag = value;
		}

		public void Create(UiProxyDto dto)
		{
			String[] subItems = Array.ConvertAll<String, String>(new String[base.ListView.Columns.Count], delegate(String a) { return String.Empty; });
			base.SubItems.AddRange(subItems);
			this.Tag = dto;

			base.SubItems[this.ListView.ColumnRequestDate.Index].Text = dto.RequestDate.ToString();
			base.SubItems[this.ListView.ColumnMethod.Index].Text = dto.Request.Method;
			base.SubItems[this.ListView.ColumnUrl.Index].Text = dto.Request.Url;
			this.UpdateDto(dto);
		}

		public void UpdateDto(UiProxyDto dto)
			=> base.SubItems[this.ListView.ColumnStatus.Index].Text = dto.Response == null ? String.Empty : dto.Response.ToString();

		public void SetRequest(SessionEventArgs args)
		{
			String[] subItems = Array.ConvertAll<String, String>(new String[base.ListView.Columns.Count], delegate(String a) { return String.Empty; });
			base.SubItems.AddRange(subItems);
			UiProxyDto tag;
			this.Tag = tag = new UiProxyDto(args);

			base.SubItems[this.ListView.ColumnRequestDate.Index].Text = tag.RequestDate.ToString();
			base.SubItems[this.ListView.ColumnMethod.Index].Text = tag.Request.Method;
			base.SubItems[this.ListView.ColumnUrl.Index].Text = tag.Request.Url;
			base.SubItems[this.ListView.ColumnStatus.Index].Text = String.Empty;
		}

		public void SetResponse(SessionEventArgs args)
		{
			UiProxyDto tag = this.Tag;
			tag.SetResponsePayload(args);
			base.SubItems[this.ListView.ColumnStatus.Index].Text = tag.Response.ToString();
		}
	}
}