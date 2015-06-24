using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TightlyCurly.Com.Framework.Web.Utilities
{
    /// <summary>
    /// Summary description for ScriptUtilities
    /// </summary>
    public static class ScriptUtilities
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static void ShowConfirmBox(Button button, string message)
        {
            button.Attributes.Add("onclick", "return confirm('" + message + "');");
        }

        public static void ShowAlertBox(Control control, string alertMessage, string scriptKey)
        {
            if (!String.IsNullOrEmpty(alertMessage) && !String.IsNullOrEmpty(alertMessage))
            {
                if (!control.Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
                {
                    control.Page.ClientScript.RegisterClientScriptBlock(control.Page.GetType(),
                        scriptKey,
                        "<script type=\"text/javascript\">alert('" +
                        alertMessage + "');</script>");
                }
            }
        }

        #endregion
    }

}
