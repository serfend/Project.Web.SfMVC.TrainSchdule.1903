using BLL.Interfaces.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Common
{
	public class CipperServices : ICipperServices
	{
		public CipperServices(IConfiguration configuration)
		{
			var permission = configuration?.GetSection("Configuration")?.GetSection("Permission");
			PublicKey = permission?["public_key"];
			PrivateKey = permission?["private_key"];
			if ((PublicKey?.Length ?? 0) == 0 || (PrivateKey?.Length ?? 0) == 0)
			{
				PublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCz3lBSqu05sK6r3SDCr2z0lT19j4LBbWbapEvv37paxbwmkvA5E/nr/VD9Hw2jueBt9NyEdnzWEgN+WmRF1GUYBQFL6YWneFkovgpLA8tgXHEojePAMfgMb+hoYHoV90MUQwANDbt0gg4nnlRxZB+WtZc5CUQT5x7ckCs5+iQNTwIDAQAB";
				PrivateKey = "MIICXgIBAAKBgQCz3lBSqu05sK6r3SDCr2z0lT19j4LBbWbapEvv37paxbwmkvA5E/nr/VD9Hw2jueBt9NyEdnzWEgN+WmRF1GUYBQFL6YWneFkovgpLA8tgXHEojePAMfgMb+hoYHoV90MUQwANDbt0gg4nnlRxZB+WtZc5CUQT5x7ckCs5+iQNTwIDAQABAoGBAKSOgcH/6uTaxhMqTWyP/giN+SHEiAXaxzzFD0w3zVB6kzZfPDOcGQxURyIspNfjmHZAjPcLSA65kESrAg340Trs00k9i1JfzYp4hc/r85yBVTp5ljWp8kPWRpfJBK3yzBok4qvGbIpJHlLrENFnVUd0dkPXKaOXZs3+mZ1GWTIRAkEA5gFj/QkpGWa/PRLSJ55ptdiIVjxXDhdNVJsozs4UcbYr/CIEUiQA6OqYNOWr8shAdQM1g65PvDYWGFJQq42qvQJBAMgyVr5P1Vj2EwahnbDtD9Zzngchcv5sv9sVlI3NNhD4tkzxntc01ikOzzy9M+x3cP1tHavv8lxgNWnWAi6hnvsCQBprfnjKXJY2XzE8wDcc0ze4L7D4LWfI9XEKgZ1/volxS4wivCxTRmd6yxEIcL/qkLzgKX1+wFn2PIN+sRWDqGECQQCazbofvXHXMajypsReTGHDzXF0SBw4uvT8P0q4/+b/5qJpCyltdjDoXMhJSnC9OHsJrHeWPZvmbIrBBTh4wIdDAkEAirO5n1K88opa8chywQxfdnKqt0BJq/x+Xp2W9V4p61PucMKSDQQ3Ytf47JUfi/17WeQeTc5L6RBDxVF2Hqjk4Q==";
			}
		}

		public string PublicKey { get; set; }
		public string PrivateKey { get; set; }
	}
}