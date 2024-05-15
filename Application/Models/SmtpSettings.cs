using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }

		public SmtpSettings(string host, int port, string username, string password, bool enableSsl)
		{
			Host = host;
			Port = port;
			Username = username;
			Password = password;
			EnableSsl = enableSsl;
		}
	}
}