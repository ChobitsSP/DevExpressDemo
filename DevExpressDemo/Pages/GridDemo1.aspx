<%@ Page Title="GridDemo1" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GridDemo1.aspx.cs" Inherits="DevExpressDemo.Pages.GridDemo1" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="GridDemo1.js"></script>
    <form runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <h3>GridDemo1</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <button runat="server" class="btn btn-success" onserverclick="Export_ServerClick">export xlsx</button>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server"
                        ClientInstanceName="grid1"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1"
                        KeyFieldName="Id">
                        <Columns>
                            <dx:GridViewCommandColumn
                                ShowClearFilterButton="true"
                                ShowNewButtonInHeader="true"
                                ShowEditButton="true"
                                ShowDeleteButton="true"
                                VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="1"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="CreateTime" VisibleIndex="2">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss"></PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataCheckColumn FieldName="IsDel" VisibleIndex="3">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataTextColumn FieldName="FileUrl" VisibleIndex="4">
                                <EditItemTemplate>
                                    <dx:ASPxTextBox ID="ASPxTextBox1" CssClass="hidden" runat="server" Text='<%# Eval("FileUrl") %>'>
                                        <ClientSideEvents Init="InitFileUrl" />
                                    </dx:ASPxTextBox>
                                </EditItemTemplate>
                                <DataItemTemplate>
                                    <img width="50" height="50" src="<%# Eval("FileUrl") %>" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource runat="server"
                        ID="SqlDataSource1"
                        ConnectionString='<%$ ConnectionStrings:ConnectionString %>'
                        SelectCommand="SELECT * FROM [Table1]"></asp:SqlDataSource>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
