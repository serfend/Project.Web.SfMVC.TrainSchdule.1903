using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;

namespace TrainSchdule.ViewModels.Account
{
	public class QRCodeSettingViewModel
	{
		public int Size { get; set; }
		public string Icon { get; set; }
		public int IconSize { get; set; }
		public Color LightColor { get; set; }
		public Color DarkColor { get; set; }
		public QRCodeGenerator.ECCLevel EccLevel { get; set; }
	}
}
