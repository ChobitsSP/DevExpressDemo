<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridDemo1.aspx.cs" Inherits="DevExpressDemo.GridDemo1" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>GridDemo1</title>
    <link href="//lib.baomitu.com/twitter-bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <script src="//lib.baomitu.com/jquery/3.2.1/jquery.min.js"></script>
    <script src="//lib.baomitu.com/twitter-bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="GridDemo1.js"></script>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h3>GridDemo1</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <button runat="server" onserverclick="Export_ServerClick">export xlsx</button>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <form id="form1" runat="server">
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server"
                        ClientInstanceName="grid1"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1"
                        KeyFieldName="Id">
                        <Columns>
                            <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true" VisibleIndex="0"></dx:GridViewCommandColumn>
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
                                    <div>
                                        <div class="form-group">
                                            <label>
                                                Upload
                                            </label>
                                            <dx:ASPxUploadControl ID="ASPxUploadControl1"
                                                FileInputCount="1"
                                                runat="server"
                                                UploadMode="Auto"
                                                Width="280px"
                                                OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete"
                                                ShowUploadButton="True"
                                                ShowProgressPanel="True">
                                                <ClientSideEvents FileUploadComplete="FileUploadComplete"></ClientSideEvents>
                                            </dx:ASPxUploadControl>
                                        </div>
                                        <div class="form-group">
                                            <label>
                                                FileUrl
                                            </label>
                                            <dx:ASPxTextBox ID="ASPxTextBox1" CssClass="form-control" runat="server" ReadOnly="true" Text='<%# Eval("FileUrl") %>'></dx:ASPxTextBox>
                                        </div>
                                    </div>
                                </EditItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource runat="server"
                        ID="SqlDataSource1"
                        ConnectionString='<%$ ConnectionStrings:ConnectionString %>'
                        SelectCommand="SELECT * FROM [Table1]"></asp:SqlDataSource>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
