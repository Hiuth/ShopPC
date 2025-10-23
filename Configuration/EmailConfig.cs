﻿namespace ShopPC.Configuration
{
    public class EmailConfig
    {
        public string From { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
