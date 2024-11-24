using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AlphaOmega.Windows.Forms;
using Titanium.Web.Proxy.EventArguments;

namespace Plugin.ProxyServer.UI
{
	internal class MessageListView : DbListView
	{
		private MessageGroupType _groupType;

		private List<UiProxyDto> _list = new List<UiProxyDto>();
		private Timer _tUpdateList = new Timer() { Enabled = false, Interval = 1000, };
		private Object _syncObj = new Object();

		public MessageGroupType GroupType
		{
			get { return this._groupType; }
			set
			{
				if(this._groupType != value)
				{
					this._groupType = value;
					this.ChangeGrouping();
				}
			}
		}

		public ColumnHeader ColumnRequestDate { get; }

		public ColumnHeader ColumnMethod { get; }

		public ColumnHeader ColumnUrl { get; }

		public ColumnHeader ColumnStatus { get; }

		public MessageListViewItem SelectedItem
		{
			get
			{
				return base.SelectedItems.Count == 1
					? (MessageListViewItem)base.SelectedItems[0]
					: null;
			}
		}

		public MessageListView()
		{
			this.ColumnRequestDate = new ColumnHeader() { Text = "Date", };
			this.ColumnMethod = new ColumnHeader() { Text = "Method", };
			this.ColumnUrl = new ColumnHeader() { Text = "Url", };
			this.ColumnStatus = new ColumnHeader() { Text = "Code", };

			base.Columns.AddRange(new ColumnHeader[] { this.ColumnRequestDate, this.ColumnMethod, this.ColumnUrl, this.ColumnStatus, });
			this._tUpdateList.Tick += tUpdateList_Tick;
			this._tUpdateList.Start();
		}

		public void ClearCacheAndList()
		{
			lock(_syncObj)
			{
				this._list.Clear();
				base.Items.Clear();
			}
		}

		public void AddRequest(SessionEventArgs args)
		{
			lock(_syncObj)
			{
				Int32 index = this._list.Count;
				this._list.Add(new UiProxyDto(args));

				/*MessageListViewItem item = new MessageListViewItem();
				base.Items.Add(item);
				item.SetRequest(args);
				item.Group = this.GetGroup(item.Tag);
				index = item.Index;*/
				args.UserData = index;//Сохраняю индекс элемента
				//this._tUpdateList.Start();
			}
		}

		public void AddResponse(SessionEventArgs args)
		{
			Int32 index = (Int32)args.UserData;//Получаю индекс элемента. TODO: Тут надо проверить, что элемент не удалён
			if(this._list.Count > index)//Иначе список может быть почищен, а ответ только дошёл
				this._list[index].SetResponsePayload(args);

			/*MessageListViewItem item = (MessageListViewItem)base.Items[index];
			item.SetResponse(args);*/
			//this._tUpdateList.Start();
		}

		private void tUpdateList_Tick(Object sender, EventArgs e)
		{
			for(Int32 loop = 0; loop < this._list.Count; loop++)
			{
				UiProxyDto dto = this._list[loop];
				if(loop >= base.Items.Count)
				{
					MessageListViewItem item = new MessageListViewItem();
					base.Items.Add(item);
					item.Create(dto);
					item.Group = this.GetGroup(dto);
				} else
					((MessageListViewItem)base.Items[loop]).UpdateDto(dto);
			}
			//base.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			//this._tUpdateList.Stop();
		}

		private void ChangeGrouping()
		{
			base.Groups.Clear();
			foreach(MessageListViewItem item in base.Items)
				item.Group = this.GetGroup(item.Tag);
		}

		private ListViewGroup GetGroup(UiProxyDto tag)
		{
			String headerName;
			switch(this.GroupType)
			{
			case MessageGroupType.None:
				return null;
			case MessageGroupType.Method:
				headerName = tag.Request.Method;
				break;
			case MessageGroupType.ProxyEndpoint:
				headerName = tag.ProxyEndpoint;
				break;
			case MessageGroupType.ClientEndpoint:
				headerName = tag.ClientEndpoint;
				break;
			case MessageGroupType.Host:
				headerName = tag.Request.Host;
				break;
			default:
				throw new NotSupportedException();
			}

			foreach(ListViewGroup item in base.Groups)
				if(item.Header == headerName)
					return item;

			ListViewGroup group = new ListViewGroup(headerName);
			base.Groups.Add(group);
			return group;
		}
	}
}