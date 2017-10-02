using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KrisHemenway.AndroidApp
{
	public class Notification
	{
		public string Title { get; set; }
		public string Message { get; set; }
		public DateTime Received { get; set; }
		public int TypeId { get; set; }
	}

	public class NotificationsAdapter : BaseAdapter<Notification>
	{
		public NotificationsAdapter()
		{

		}

		public override Notification this[int position]
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override int Count
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override long GetItemId(int position)
		{
			throw new NotImplementedException();
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			throw new NotImplementedException();
		}
	}
}