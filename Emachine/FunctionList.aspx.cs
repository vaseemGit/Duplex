using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class FunctionList : System.Web.UI.Page
{
    private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
         {
             try
             {
              if (GetQueryString("ClientAddress") != null && GetQueryString("ClientAddress") != string.Empty)
                 {
                     string ClientAddress= GetQueryString("ClientAddress").Trim();
                     using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
                     {
                         var data = entities.MachineDetails.Where(m => m.ClientAddress == ClientAddress.Trim()).FirstOrDefault();
                         if (data != null)
                         {
                             string functionName = Convert.ToString(data.AvailableFunction);
                             machineNameLabel.Text = Convert.ToString(data.MachineKey);
                             if (!string.IsNullOrEmpty(functionName))
                             {
                                 string[] functionArray = functionName.Split(',');
                                 functionListGridView.DataSource = functionArray.Select(a => new { data = a });
                                 functionListGridView.DataBind();
                             }
                             else
                             {
                                 functionListGridView.DataSource = null;
                                 functionListGridView.DataBind();
                             }
                         }

                
                     }
                 }
             }
             catch (Exception ex)
             {
                 logger.Info(ex.Message.ToString());
             }


         }


    }

    #region Method GetQueryString
    public string GetQueryString(string key)
    {
        if (HttpContext.Current.Request.QueryString[key] != null)
        {
            return HttpContext.Current.Request.QueryString[key].ToString();
        }
        return null;
    }
    #endregion
    protected void functionNameButton_Click(object sender, EventArgs e)
    {
        try
        {

        Button functonNamebutton = (Button)sender;
        Service vs = new Service();
        vs.NotifyServer(GetQueryString("ClientAddress").Trim(), "ExecuteFunction", functonNamebutton.Text);
        if(vs.errorMsg!=string.Empty)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + vs.errorMsg + "');", true);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('"+functonNamebutton.Text.Trim()+" command executed on client machine.');", true);
        }
        }catch (Exception ex)
        {
            logger.Info(ex.Message.ToString());
        }

                

    }
}