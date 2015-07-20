using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ServiceModel;
public partial class _Default : System.Web.UI.Page
{
 
    #region  Mehod Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
        {
            LoadMachineData();
        }
    }
    #endregion
    #region LoadMachineData
    protected void LoadMachineData()
   {
       try
       {
           using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
           {
              
               var query = (from c in entities.MachineDetails.AsEnumerable()
                            select new { c.ID, c.MachineKey, c.ModifiedDate, c.CreatedDate, c.status,c.ClientAddress }).OrderByDescending(c => c.ID).ToList();
               if (query != null && query.Count > 0)
               {
                   machineListGridView.DataSource = query.ToList();
                   machineListGridView.DataBind();
               }
               else
               {
                   machineListGridView.DataSource = null;
                   machineListGridView.DataBind();
               }
           }
       }                                                                                                                                                                                                                                         
       catch(Exception ex)
       {
     
       }

   }
    #endregion
    #region machineListGridView_RowCommand
    protected  void machineListGridView_RowCommand(object sender, GridViewCommandEventArgs e)
   {
       try
       {
           if(e.CommandName=="MachineName")
           {
               LinkButton lnkButton = (LinkButton)e.CommandSource;
               using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
               {
                   var data = entities.MachineDetails.Where(m => m.MachineKey == lnkButton.Text.Trim()).FirstOrDefault();
                   if (data != null)
                   {
                       data.AvailableFunction = null;
                       entities.SaveChanges();
                   }
               }
               GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
               Label clientAddressLabel = (Label)machineListGridView.Rows[row.RowIndex].FindControl("clientAddressLabel");
               Service vs = new Service();
               vs.NotifyServer(clientAddressLabel.Text);
                   
               using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
               {
                   var data = entities.MachineDetails.Where(m => m.MachineKey == lnkButton.Text.Trim()).FirstOrDefault();
                   if (data != null)
                   {
                       functionListLabel.Text =Convert.ToString(data.AvailableFunction);
                       machineNameLabel.Text = Convert.ToString(lnkButton.Text.Trim());
                   }
                   if (functionListLabel.Text.Trim() == string.Empty)
                   {
                       functionListLabel.Text = "No function available";
                   }
                   availableFunctiondiv.Visible=true;
                  
                   
               }

              
              
           }
       }
       catch (Exception ex)
       {
            
       }


   }
    #endregion
}
