using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.Common
{
	public interface ICipperServices
	{
		string PublicKey { get; set; }
		string PrivateKey { get; set; }
	}
}