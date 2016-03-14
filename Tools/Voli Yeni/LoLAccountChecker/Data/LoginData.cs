namespace LoLAccountChecker.Data
{
    using System;

    internal class LoginData
    {
        public bool Checked;
        public string Password;
        public string Username;

        public LoginData(string usr, string pass)
        {
            this.Username = usr;
            this.Password = pass;
            this.Checked = false;
        }
    }
}

