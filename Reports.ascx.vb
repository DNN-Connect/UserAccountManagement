Imports DotNetNuke.Entities.Modules
Imports Connect.Libraries.UserManagement
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership
Imports Telerik.Web.UI
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace Connect.Modules.UserManagement.AccountManagement

    Partial Class Reports
        Inherits ConnectUsersModuleBase


        Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

            JavaScript.RequestRegistration(CommonJs.DnnPlugins)

        End Sub

        Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then

                BindReports()

            End If

        End Sub

        Private Sub cmdNewReport_Click(sender As Object, e As EventArgs) Handles cmdNewReport.Click

            BindEditForm(Null.NullInteger)

        End Sub

        Private Sub cmdAddReport_Click(sender As Object, e As EventArgs) Handles cmdAddReport.Click

            AddReport()
            pnlReportForm.Visible = False
            BindReports()

        End Sub

        Private Sub cmdUpdateReport_Click(sender As Object, e As EventArgs) Handles cmdUpdateReport.Click

            Dim ReportId As Integer = Convert.ToInt32(cmdUpdateReport.CommandArgument)
            UpdateReport(ReportId)
            pnlReportForm.Visible = False
            BindReports()

        End Sub

        Private Sub cmdCancelReport_Click(sender As Object, e As EventArgs) Handles cmdCancelReport.Click

            pnlReportForm.Visible = False
            txtReportName.Text = ""
            txtReportSql.Text = ""

        End Sub

        Private Sub cmdDeleteReport_Click(sender As Object, e As EventArgs) Handles cmdDeleteReport.Click

            Dim ReportId As Integer = Convert.ToInt32(cmdDeleteReport.CommandArgument)
            UserReportsController.DeleteReport(ReportId)
            BindReports()

        End Sub

        Public Sub cmdEditReportFromList_Click(sender As Object, e As EventArgs)

            Dim ReportId As Integer = Convert.ToInt32(CType(sender, LinkButton).CommandArgument)
            BindEditForm(ReportId)

        End Sub

        Public Sub cmdDeleteReportFromList_Click(sender As Object, e As EventArgs)

            Dim ReportId As Integer = Convert.ToInt32(CType(sender, LinkButton).CommandArgument)
            UserReportsController.DeleteReport(ReportId)
            BindReports()

        End Sub

        Private Sub cmdBack_Click(sender As Object, e As EventArgs) Handles cmdBack.Click
            Response.Redirect(NavigateURL(TabId))
        End Sub


#Region "Private Methods"

        Private Sub AddReport()

            Dim objReport As New UserReportInfo
            objReport.FriendlyName = txtReportName.Text
            objReport.PortalId = PortalId
            objReport.Sql = txtReportSql.Text
            objReport.NeedsParameters = False
            UserReportsController.AddReport(objReport)

        End Sub

        Private Sub UpdateReport(ReportId As Integer)

            Dim objReport As UserReportInfo = UserReportsController.GetReport(ReportId)
            If Not objReport Is Nothing Then
                objReport.Sql = txtReportSql.Text
                objReport.FriendlyName = txtReportName.Text
                UserReportsController.UpdateReport(objReport)
            End If

        End Sub

        Private Sub BindReports()

            Dim reports As New List(Of UserReportInfo)
            reports = UserReportsController.GetReports(PortalId)
            rptReports.DataSource = reports
            rptReports.DataBind()

        End Sub

        Private Sub BindEditForm(ReportId As Integer)

            pnlReportForm.Visible = True
            txtReportName.Text = ""
            txtReportSql.Text = ""

            cmdUpdateReport.Visible = False
            cmdDeleteReport.Visible = False
            cmdAddReport.Visible = True

            If ReportId <> Null.NullInteger Then

                Dim objReport As UserReportInfo = UserReportsController.GetReport(ReportId)
                If Not objReport Is Nothing Then

                    txtReportName.Text = objReport.FriendlyName
                    txtReportSql.Text = objReport.Sql

                    cmdAddReport.Visible = False
                    cmdUpdateReport.Visible = True
                    cmdUpdateReport.CommandArgument = objReport.ReportId.ToString
                    cmdDeleteReport.Visible = True
                    cmdDeleteReport.CommandArgument = objReport.ReportId.ToString

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace


