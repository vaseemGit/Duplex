<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      
      <h2>Machine List</h2>
        
    <div>
    <asp:GridView ID="machineListGridView" runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" CellPadding="3" OnRowCommand="machineListGridView_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Sr. No"> 
                <ItemTemplate>
                    <asp:Label ID="srNoLabel" runat="server" Text='<%# (Container.DataItemIndex+1).ToString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Machine Key"> 
               <ItemTemplate>
                   <asp:LinkButton ID="stationlinkButton" runat="server" Text='<%#Eval("MachineKey")%>' CommandName="MachineName"></asp:LinkButton>
               </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField HeaderText="CLient Address"> 
               <ItemTemplate>
                   <asp:Label runat="server" ID="clientAddressLabel" Text='<%# Eval("ClientAddress")%>'></asp:Label>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedDate" HeaderText="Date & Time" />
        </Columns>

        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />

    </asp:GridView>
        
    </div>
    </form>
</body>
</html>
