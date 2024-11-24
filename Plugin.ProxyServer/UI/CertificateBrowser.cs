using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Plugin.ProxyServer.UI
{
	public class CertificateBrowser : UITypeEditor
	{
		public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
		{
			using(OpenFileDialog dlg = new OpenFileDialog() { Filter = "Security Certificate (*.crt)|*.crt|Personal Information Exchange (*.pfx)|*.pfx|All files (*.*)|*.* ", CheckFileExists = false, Multiselect = false, ShowReadOnly = false, })
				if(dlg.ShowDialog() == DialogResult.OK)
				{
					PluginSettings settings = (PluginSettings)context.Instance;
					settings.SetRootCertificate(dlg.FileName);
				}
			return null;
		}
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}