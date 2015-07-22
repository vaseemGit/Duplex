<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FunctionList.aspx.cs" Inherits="FunctionList" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Emachine:Function List</title>
</head>
<body>
    <form id="form1" runat="server">

    <h2 style="background-color: #006699;color: White;font-weight: bold;margin-top:0px;padding:5px;">Function List of the particular machines are :-</h2>

                <div style="width:100%;margin:5px;">
            <a href="Default.aspx" style="color:red;">Go To Machine List</a>

        </div>
    <div>

<table style="width:50%;" align="center">
    <tbody>
    <tr>
        <td>
       <b> Machine Name:</b>
        </td>
        <td>
                <asp:Label runat="server" ID="machineNameLabel"></asp:Label>
        </td>

    </tr>
        <tr>
            <td style="vertical-align:top;">
                <b>Available Function of the Machine are:</b>

            </td>
            <td>
                <asp:GridView ID="functionListGridView" runat="server" AutoGenerateColumns="False" EmptyDataText="Not any function available" EmptyDataRowStyle-ForeColor="Red" ShowHeader="False" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2">
                    <Columns>
                       <asp:TemplateField>
                           <ItemTemplate>
                             <asp:Button ID="functionNameButton" runat="server" Text='<%# Eval("Data") %>' OnClick="functionNameButton_Click"
                                  style="min-width:100px;height:30px"></asp:Button>
                           </ItemTemplate>

                       </asp:TemplateField>

                    </Columns>

<EmptyDataRowStyle ForeColor="Red"></EmptyDataRowStyle>
                    <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                    <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                    <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FFF1D4" />
                    <SortedAscendingHeaderStyle BackColor="#B95C30" />
                    <SortedDescendingCellStyle BackColor="#F1E5CE" />
                    <SortedDescendingHeaderStyle BackColor="#93451F" />

                </asp:GridView>
               <%-- <asp:Label runat="server" ID="functionListLabel" ></asp:Label>--%>
            </td>

        </tr>
        </tbody>
</table>

            


    </div>
    </form>
</body>
</html>
