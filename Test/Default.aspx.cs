using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Collections.Specialized;

namespace Test
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Crypto crypto = new Crypto(true);
            string plaintextToken = DateTime.Now.Ticks.ToString();
            plaintextToken += " " + tbTerritories.Text;
            string encryptedToken = Server.UrlEncode(crypto.Encrypt(plaintextToken));
            link.HRef = tbUrl.Text + "?token=" + encryptedToken;
            //Response.Redirect(tbUrl.Text + "?token=" + encryptedToken);
        }
    }
}