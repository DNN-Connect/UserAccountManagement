<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.AccountManagement.View" Codebehind="View.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="connect_main">

    <div class="connect-accounts-select" id="ctlRolesContainer" runat="server">
    </div>

    <div class="processing">
        <div class="bubblingG">
            <span id="bubblingG_1"></span>
            <span id="bubblingG_2"></span>
            <span id="bubblingG_3"></span>
        </div>
    </div>

    <table border="0" class="display hover" id="grdAccounts">
        <thead>
            <tr>
                <%=GetColumnsHTML() %>
            </tr>
        </thead>
    </table>

    <ul class="dnnActions dnnClear" runat="server" id="UserActions">
        <li id="btnDelete" runat="server"><a href="#" class="cmdDelete dnnSecondaryAction">Delete selected</a></li>
        <li id="btnRemove" runat="server"><a href="#" class="cmdDelete dnnSecondaryAction">Remove selected</a></li>
    </ul>

</div>

<div class="connectRolesDialog dnnClear">

    <div class="connent-arrow-left"></div>
    <div class="connect-roles-content">

        <p class="connect-roles-heading"><%= Localization.GetString("lblSelectUserRoles", LocalResourceFile)%></p>
        <asp:CheckBoxList ID="chkUserRoles" runat="server" DataTextField="RoleName" DataValueField="RoleId"></asp:CheckBoxList>
        <p><a href="#" class="dnnPrimaryAction cmdUpdateUserRoles" data-userid=""><%= Localization.GetString("btnSaveUserRoles", LocalResourceFile)%></a></p>

    </div>

</div>

<div class="dnnDialog dnnClear RoleMembershipSetRemoved" title='<%= Localization.GetString("lblRoleMembershipSetRemovedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblRoleMembershipSetRemovedNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear RoleMembershipSetApproved" title='<%= Localization.GetString("lblRoleMembershipSetApprovedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblRoleMembershipSetApprovedNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear RoleMembershipSetPending" title='<%= Localization.GetString("lblRoleMembershipSetPendingTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblRoleMembershipSetPendingNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetDeleted" title='<%= Localization.GetString("lblAccountSetDeletedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetDeletedNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetRemoved" title='<%= Localization.GetString("lblAccountSetRemovedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetRemovedNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetRestored " title='<%= Localization.GetString("lblAccountSetRestoredTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetRestoredNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetPending" title='<%= Localization.GetString("lblAccountSetPendingTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetPendingNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetApproved " title='<%= Localization.GetString("lblAccountSetApprovedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetApprovedNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetAuthorized " title='<%= Localization.GetString("lblAccountSetAuthorizedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetAuthorizedNote", LocalResourceFile)%></p>
</div>

<div class="dnnDialog dnnClear AccountSetUnAuthorized " title='<%= Localization.GetString("lblAccountSetAuthorizedTitle", LocalResourceFile)%>'>
    <p><%= Localization.GetString("lblAccountSetUnAuthorizedNote", LocalResourceFile)%></p>
</div>

<script language="javascript" type="text/javascript">
   
    var grid;
    var mid = <%=ModuleId%>;
    var userid = <%=UserId%>;
    var pageNo = 1;
    var recordsCount = 3;
    var roleId = <%=GetPreSelectedRole()%>;
    var portalId = <%=PortalId%>;
    var searchPattern = '';
    var searchColumns = <%=GetSearchColumns()%>;
    var users;
    var registeredRoleId = <%=PortalSettings.RegisteredRoleId%>;
    var unverifiedRoleId = <%=GetUnverifiedUsersRole()%>;
    var editUrl = '<%=GetEditUrl()%>';

    var cmdConfirmRestore = '<%= Localization.GetString("cmdConfirmRestore", LocalResourceFile)%>';
    var cmdConfirmDelete = '<%= Localization.GetString("cmdConfirmDelete", LocalResourceFile)%>';
    var cmdConfirmRemoval = '<%= Localization.GetString("cmdConfirmRemoval", LocalResourceFile)%>';    
    var cmdSetPendingAndNotify = '<%= Localization.GetString("cmdSetPendingAndNotify", LocalResourceFile)%>';
    var cmdSetPendingOnly = '<%= Localization.GetString("cmdSetPendingOnly", LocalResourceFile)%>';
    var cmdSetUnauthorizedAndNotify = '<%= Localization.GetString("cmdSetUnauthorizedAndNotify", LocalResourceFile)%>';
    var cmdSetUnauthorizedOnly = '<%= Localization.GetString("cmdSetUnauthorizedOnly", LocalResourceFile)%>';
    var cmdSetAuthorizedAndNotify = '<%= Localization.GetString("cmdSetAuthorizedAndNotify", LocalResourceFile)%>';
    var cmdSetAuthorizedOnly = '<%= Localization.GetString("cmdSetAuthorizedOnly", LocalResourceFile)%>';
    var cmdApproveAndNotify = '<%= Localization.GetString("cmdApproveAndNotify", LocalResourceFile)%>';
    var cmdApproveOnly = '<%= Localization.GetString("cmdApproveOnly", LocalResourceFile)%>';
    var cmdRemoveOnly = '<%= Localization.GetString("cmdRemoveOnly", LocalResourceFile)%>';
    var cmdRemoveAndNotify = '<%= Localization.GetString("cmdRemoveAndNotify", LocalResourceFile)%>';
    var cmdCancel = '<%= Localization.GetString("cmdCancel", LocalResourceFile)%>';
    var noRecordsString = '<%= Localization.GetString("NoRecords", LocalResourceFile)%>';
    var emptySearchBoxString = '<%= Localization.GetString("emptySearchBoxString", LocalResourceFile)%>';
    var paginationNextString = '<%= Localization.GetString("paginationNextString", LocalResourceFile)%>';
    var paginationPrevString = '<%= Localization.GetString("paginationPrevString", LocalResourceFile)%>';
    var tableInfoString = '<%= String.Format(Localization.GetString("tableInfoString", LocalResourceFile), "_START_", "_END_", "_TOTAL_")%>';



    $(".RoleMembershipSetRemoved").dialog({
        autoOpen: false,
        height: 350,
        width: 550,
        modal: true,
        dialogClass: 'dnnFormPopup',
        buttons: [ 
            {
                text: cmdRemoveOnly, class: 'dnnPrimaryAction', click: function () {
                    var userId = $(this).dialog("option", "userId");
                    var roleId = $(this).dialog("option", "roleId");
                    setRoleMembershipDeleted(userId,roleId,portalId);
                    $(this).dialog("close");
                    setTimeout(function(){ refreshGrid(); }, 1000); 
                }
            },
            { 
                text: cmdRemoveAndNotify, class: 'dnnSecondaryAction', click: function () {
                    var userId = $(this).dialog("option", "userId");
                    var roleId = $(this).dialog("option", "roleId");
                    var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=RoleMembershipDeleted", "UserId=' + userId + '", "RoleId=' + roleId + '")%>';
                    setRoleMembershipDeleted(userId,roleId,portalId);
                    window.location.href = url;
                }
            },
            {
                text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () {
            $(this).dialog("close");
        }
    });

        $(".RoleMembershipSetApproved").dialog({
            autoOpen: false,
            height: 350,
            width: 550,
            modal: true,
            dialogClass: 'dnnFormPopup',
            buttons: [ 
                { 
                    text: cmdApproveAndNotify, class: 'dnnPrimaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        var roleId = $(this).dialog("option", "roleId");
                        var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=RoleMembershipApproved", "UserId=' + userId + '", "RoleId=' + roleId + '")%>';
                        setRoleMembershipApproved(userId,roleId,portalId);
                        window.location.href = url;
                    }
                },
            {
                text: cmdApproveOnly, class: 'dnnSecondaryAction', click: function () {                    
                    var userId = $(this).dialog("option", "userId");
                    roleId = $(this).dialog("option", "roleId");                
                    setRoleMembershipApproved(userId,roleId,portalId);
                    $(this).dialog("close");
                    setTimeout(function(){ refreshGrid(); }, 1000);    
                }
            },
            {
                text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                    $(this).dialog("close");
                }
            }
            ],
        close: function () {
            $(this).dialog("close");
        }
    });

            $(".RoleMembershipSetPending").dialog({
                autoOpen: false,
                height: 350,
                width: 550,
                modal: true,
                dialogClass: 'dnnFormPopup',
                buttons: [ 
                    { 
                        text: cmdSetPendingOnly, class: 'dnnPrimaryAction', click: function () {
                            var userId = $(this).dialog("option", "userId");
                            roleId = $(this).dialog("option", "roleId");                    
                            setRoleMembershipPending(userId,roleId,portalId);
                            $(this).dialog("close");                            
                            setTimeout(function(){ refreshGrid(); }, 1000);
                        }
                    },
                {
                    text: cmdSetPendingAndNotify, class: 'dnnSecondaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        roleId = $(this).dialog("option", "roleId");
                        setRoleMembershipPending(userId,roleId,portalId);
                        var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=RoleMembershipPending", "UserId=' + userId + '", "RoleId=' + roleId + '")%>';
                    window.location.href = url;               
                }
            },
        {
            text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                $(this).dialog("close");
            }
        }
            ],
        close: function () {
            $(this).dialog("close");
        }
    });

        $(".AccountSetDeleted").dialog({
            autoOpen: false,
            height: 300,
            width: 550,
            modal: true,
            dialogClass: 'dnnFormPopup',
            buttons: [ 
                { 
                    text: cmdConfirmDelete, class: 'dnnPrimaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        setUserDeleted(userId,portalId);
                        $(this).dialog("close");  
                        setTimeout(function(){ refreshGrid(); }, 1000);                                      
                    }
                },
                {
                    text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function () {
                $(this).dialog("close");
            }
        });

        $(".AccountSetRemoved").dialog({
            autoOpen: false,
            height: 300,
            width: 550,
            modal: true,
            dialogClass: 'dnnFormPopup',
            buttons: [ 
                { 
                    text: cmdConfirmRemoval, class: 'dnnPrimaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        setUserRemoved(userId,portalId);
                        $(this).dialog("close");  
                        setTimeout(function(){ refreshGrid(); }, 1000);                  
                    }
                },
                {
                    text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function () {
                $(this).dialog("close");
            }
        });

        $(".AccountSetRestored").dialog({
            autoOpen: false,
            height: 300,
            width: 550,
            modal: true,
            dialogClass: 'dnnFormPopup',
            buttons: [ 
                { 
                    text: cmdConfirmRestore, class: 'dnnPrimaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        setUserRestored(userId,portalId);
                        $(this).dialog("close");  
                        setTimeout(function(){ refreshGrid(); }, 1000);                    
                    }
                },
                {
                    text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function () {
                $(this).dialog("close");
            }
        });

        $(".AccountSetPending").dialog({
            autoOpen: false,
            height: 350,
            width: 550,
            modal: true,
            dialogClass: 'dnnFormPopup',
            buttons: [ 
                { 
                    text: cmdSetPendingAndNotify, class: 'dnnPrimaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        setUserPending(userId,portalId);
                        var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=AccountPending", "UserId=' + userId + '")%>';
                    window.location.href = url;                  
                }
            },
            { 
                text:cmdSetPendingOnly, class: 'dnnSecondaryAction', click: function () {
                    var userId = $(this).dialog("option", "userId");
                    setUserPending(userId,portalId);
                    $(this).dialog("close");  
                    setTimeout(function(){ refreshGrid(); }, 1000);                  
                }
            },
            {
                text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        close: function () {
            $(this).dialog("close");
        }
    });

        $(".AccountSetUnAuthorized").dialog({
            autoOpen: false,
            height: 350,
            width: 550,
            modal: true,
            dialogClass: 'dnnFormPopup',
            buttons: [ 
                { 
                    text: cmdSetUnauthorizedAndNotify, class: 'dnnPrimaryAction', click: function () {
                        var userId = $(this).dialog("option", "userId");
                        setUserUnAuthorized(userId,portalId);
                        var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=AccountUnAuthorized", "UserId=' + userId + '")%>';
                        window.location.href = url;                  
                    }
                },
            { 
                text: cmdSetUnauthorizedOnly, class: 'dnnSecondaryAction', click: function () {
                    var userId = $(this).dialog("option", "userId");
                    setUserUnAuthorized(userId,portalId);
                    $(this).dialog("close");  
                    setTimeout(function(){ refreshGrid(); }, 1000);                  
                }
            },
            {
                text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                    $(this).dialog("close");
                }
            }
            ],
        close: function () {
            $(this).dialog("close");
        }
    });

            $(".AccountSetAuthorized").dialog({
                autoOpen: false,
                height: 350,
                width: 550,
                modal: true,
                dialogClass: 'dnnFormPopup',
                buttons: [ 
                    { 
                        text: cmdSetAuthorizedAndNotify, class: 'dnnPrimaryAction', click: function () {
                            var userId = $(this).dialog("option", "userId");
                            setUserAuthorized(userId,portalId);
                            var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=AccountAuthorized", "UserId=' + userId + '")%>';
                        window.location.href = url;                  
                    }
                },
            { 
                text: cmdSetAuthorizedOnly, class: 'dnnSecondaryAction', click: function () {
                    var userId = $(this).dialog("option", "userId");
                    setUserAuthorized(userId,portalId);
                    $(this).dialog("close");  
                    setTimeout(function(){ refreshGrid(); }, 1000);                  
                }
            },
            {
                text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                    $(this).dialog("close");
                }
            }
            ],
        close: function () {
            $(this).dialog("close");
        }
    });

            $(".AccountSetApproved").dialog({
                autoOpen: false,
                height: 350,
                width: 550,
                modal: true,
                dialogClass: 'dnnFormPopup',
                buttons: [ 
                    { 
                        text: cmdApproveAndNotify, class: 'dnnPrimaryAction', click: function () {
                            var userId = $(this).dialog("option", "userId");
                            setUserApproved(userId,portalId);
                            var url = '<%= NavigateUrl(TabId, "", "Action=Notify", "Reason=AccountApproved", "UserId=' + userId + '")%>';
                        window.location.href = url;                  
                    }
                },
            { 
                text:cmdApproveOnly, class: 'dnnSecondaryAction', click: function () {
                    var userId = $(this).dialog("option", "userId");
                    setUserApproved(userId,portalId);
                    $(this).dialog("close");  
                    setTimeout(function(){ refreshGrid(); }, 1000);                 
                }
            },
            {
                text: cmdCancel, class: 'dnnSecondaryAction', click: function () {
                    $(this).dialog("close");
                }
            }
            ],
        close: function () {
            $(this).dialog("close");
        }
    });

            function bindGridActions()
            {
                $('.cmdAccountSetDeleted').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetDeleted").dialog("option", "userId", userId);           
                    $(".AccountSetDeleted").dialog("open");
                });

                $('.cmdAccountSetRemoved').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetRemoved").dialog("option", "userId", userId);           
                    $(".AccountSetRemoved").dialog("open");
                });

                $('.cmdAccountSetRestored').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetRestored").dialog("option", "userId", userId);           
                    $(".AccountSetRestored").dialog("open");
                });

                $('.cmdAccountSetApproved').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetApproved").dialog("option", "userId", userId);           
                    $(".AccountSetApproved").dialog("open");
                });

                $('.cmdAccountSetPending').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetPending").dialog("option", "userId", userId);           
                    $(".AccountSetPending").dialog("open");
                });

                $('.cmdAccountSetUnAuthorized').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetUnAuthorized").dialog("option", "userId", userId);           
                    $(".AccountSetUnAuthorized").dialog("open");
                });

                $('.cmdAccountSetAuthorized').click(function () {
                    var userId = $(this).data("uid");
                    $(".AccountSetAuthorized").dialog("option", "userId", userId);           
                    $(".AccountSetAuthorized").dialog("open");
                });

                $('.cmdRoleMembershipSetRemoved').click(function () {
                    var userId = $(this).data("uid");
                    $(".RoleMembershipSetRemoved").dialog("option", "userId", userId);           
                    $(".RoleMembershipSetRemoved").dialog("option", "roleId", roleId);  
                    $(".RoleMembershipSetRemoved").dialog("open");
                });

                $('.cmdRoleMembershipSetApproved').click(function () {
                    var userId = $(this).data("uid");
                    $(".RoleMembershipSetApproved").dialog("option", "userId", userId);           
                    $(".RoleMembershipSetApproved").dialog("option", "roleId", roleId);  
                    $(".RoleMembershipSetApproved").dialog("open");
                });

                $('.cmdRoleMembershipSetPending').click(function () {
                    var userId = $(this).data("uid");
                    $(".RoleMembershipSetPending").dialog("option", "userId", userId);           
                    $(".RoleMembershipSetPending").dialog("option", "roleId", roleId);  
                    $(".RoleMembershipSetPending").dialog("open");
                });  

                $('.cmdAccountSetRoles').click(function (event) {    
                    event.preventDefault();
                    var el = $(this);
                    var userId = el.data("uid");
                    showUserRoles(userid, el);
                });  
        
            }

            function showUserRoles(userid, element)
            {
                var container = $(".connectRolesDialog");
                var pos = element.position();
                var width = element.outerWidth();
                container.css({
                    position: "absolute",
                    top: pos.top + "px",
                    left: (pos.left + width + 10) + "px"
                }).effect( "slide", 300 );

                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/GetUserRoles',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userid, PortalId: portalId },
                    success: function(data){ 
                        //
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }
            function setRoleMembershipApproved(userId,roleId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetRoleMembershipApproved',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, RoleId: roleId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setRoleMembershipPending(userId,roleId, portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetRoleMembershipPending',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, RoleId: roleId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setRoleMembershipDeleted(userId,roleId, portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetRoleMembershipDeleted',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, RoleId: roleId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserPending(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserPending',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserUnAuthorized(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserUnAuthorized',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserAuthorized(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserAuthorized',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserApproved(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserApproved',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserDeleted(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserDeleted',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserRemoved(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserRemoved',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function setUserRestored(userId,portalId)
            {
                var sf = $.ServicesFramework(mid);

                $.ajax({
                    type: "POST",
                    url: sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/SetUserRestored',
                    beforeSend: sf.setModuleHeaders,
                    data: { UserId: userId, PortalId: portalId },
                    success: function(data){ return data; },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

            }

            function getStatusLinks(user){    
        
                var uid = user["UserId"];        
                var status = user["Status"];
        
                var setAccountApprovedLink          = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetApproved connectaccounts-hourglass1"></a></li>';
                var setAccountPendingLink           = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetPending connectaccounts-check30"></a></li>';
                var setAccountAuthorizedLink        = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetAuthorized connectaccounts-hourglass1"></a></li>';
                var setAccountUnAuthorizedLink      = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetUnAuthorized connectaccounts-check30"></a></li>';
                var setRoleMembershipApprovedLink   = '<li><a href="#" data-uid="' + uid + '" data-roleid="' + roleId + '" class="cmdRoleMembershipSetApproved connectaccounts-hourglass1"></a></li>';
                var setRoleMembershipPendingLink    = '<li><a href="#" data-uid="' + uid + '" data-roleid="' + roleId + '" class="cmdRoleMembershipSetPending connectaccounts-check30"></a></li>';


                if (roleId == -2)
                {
                    setAccountApprovedLink = '';
                    setAccountPendingLink = '';
                    setRoleMembershipApprovedLink = '';
                    setRoleMembershipPendingLink = '';
                    setAccountAuthorizedLink = '';
                    setAccountUnAuthorizedLink = '';
                }

                if (roleId == registeredRoleId)
                {
            
                    status = user["Authorised"]; // we don't do any role status here, we only set the authorized flag

                    setRoleMembershipApprovedLink = '';
                    setRoleMembershipPendingLink = '';
                    setAccountApprovedLink = '';
                    setAccountPendingLink = '';

                    if (status == 1 || status == 'true' || status == 'True')
                    {
                        setAccountAuthorizedLink = '';
                    }
                    else{
                        setAccountUnAuthorizedLink = '';
                    }
                }
                else if(roleId == unverifiedRoleId)
                {
                    status = 0; // unverified users are always treated as not approved

                    // show only setApprovedLink
                    setRoleMembershipApprovedLink = '';
                    setRoleMembershipPendingLink = '';
                    setAccountAuthorizedLink = '';
                    setAccountUnAuthorizedLink = '';
                    setAccountPendingLink = '';
                }
                else
                {
                    setAccountApprovedLink = '';
                    setAccountPendingLink = '';
                    setAccountAuthorizedLink = '';
                    setAccountUnAuthorizedLink = '';

                    if (status == 1)
                    {
                        setRoleMembershipApprovedLink = '';
                    }
                    else
                    {
                        setRoleMembershipPendingLink = '';
                    }
                }

                var actions       = '<ul class="connect-gridactions">';
        
                actions          += setAccountApprovedLink;
                actions          += setAccountPendingLink;
                actions          += setRoleMembershipApprovedLink;
                actions          += setRoleMembershipPendingLink;
                actions          += setAccountAuthorizedLink;
                actions          += setAccountUnAuthorizedLink;

                actions          += '</ul>'  
                return actions;

            }

            function getActionLinks(user){        
                
                var uid = user["UserId"];       

                var editLink                        = '<li><a href="' + editUrl + '/UserId/' + uid + '"class="connectaccounts-pencil43"></a></li>';
        
                var setAccountDeletedLink           = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetDeleted connectaccounts-delete30"></a></li>';
                var setAccountRemovedLink           = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetRemoved connectaccounts-delete30"></a></li>';
                var setAccountRestoredLink          = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetRestored connectaccounts-undo9"></a></li>';
                var setRoleMembershipRemovedLink    = '<li><a href="#" data-uid="' + uid + '" data-roleid="' + roleId + '" class="cmdRoleMembershipSetRemoved connectaccounts-delete30"></a></li>';
                var setUserRolesLink                = '<li><a href="#" data-uid="' + uid + '" class="cmdAccountSetRoles connectaccounts-roles">Roles</a></li>';

                if (roleId == registeredRoleId)
                {
                    setAccountRemovedLink = '';
                    setAccountRestoredLink = '';
                    setRoleMembershipRemovedLink = '';
                }
                else if (roleId == -2)
                {
                    setAccountDeletedLink = '';
                    setRoleMembershipRemovedLink = '';
                    editLink = '';
                    setUserRolesLink = '';
                }
                else
                {
                    setAccountDeletedLink = '';
                    setAccountRemovedLink = '';
                    setAccountRestoredLink = '';
                }


                var actions       = '<ul class="connect-gridactions">';
        
                actions          += editLink;
                actions          += setAccountDeletedLink;
                actions          += setAccountRemovedLink;
                actions          += setAccountRestoredLink;
                actions          += setRoleMembershipRemovedLink;
                actions          += setUserRolesLink;

                actions          += '</ul>'  
                return actions;

            }

            function refreshGrid(){
                $('#grdAccounts').dataTable().api().ajax.reload();
            }

            function setProcessing(processing)
            {
                $(".processing").css( 'display', processing ? 'block' : 'none' );
            }

            function bindUserRolesUpdate()
            {

            }
            function initGrid() {

                var sf = $.ServicesFramework(mid);
                var serviceUrl = sf.getServiceRoot('Connect/UserAccounts') + 'UserAccounts/ProcessDataTablesRequest';
                var orderPattern = '';
                var defaultSort =<%=GetDefaultSort()%>;
            var defaultPageSize = <%=GetDefaultPageSize()%>;
        var defaultColumns = <%=GetDisplayColumns()%>;

        $('#grdAccounts')
            .on( 'draw.dt', function () {
                bindGridActions();
            })
            .on('processing.dt', function ( e, settings, processing ) {
                setProcessing(processing);
            })
            .dataTable({
                "language": {
                    "lengthMenu": "_MENU_",
                    "search": "",
                    "zeroRecords": noRecordsString,
                    "searchPlaceholder": emptySearchBoxString,
                    "emptyTable": noRecordsString,
                    "info": tableInfoString,
                    "paginate": {
                        "next": paginationNextString,
                        "previous": paginationPrevString
                    }
                },
                "processing": false,
                "serverSide": true,
                "columns": defaultColumns,
                "order": defaultSort,
                "pageLength": defaultPageSize,
                "ajax" : {
                    type: "POST",
                    beforeSend: sf.setModuleHeaders,
                    url: serviceUrl,
                    data: function(d) {
                        if (d.order.length > 0 ){
                            var i = d.order.length-1;
                            orderPattern =  d.columns[d.order[i].column].data;
                            orderPattern +=  '-';
                            orderPattern +=  d.order[i].dir;
                        }
                        var postData = {
                            "draw": d.draw, 
                            "start": d.start, 
                            "length": d.length, 
                            "roleId": roleId, 
                            "portalId": portalId,
                            "searchPattern": d.search.value, 
                            "searchColumns": searchColumns, 
                            "orderPattern": orderPattern
                        };
                        return postData;
                    },
                    dataType : "json",
                },
                dom: 'T<"clear">lfrtip',
                tableTools: {
                    "sRowSelect": "os",
                    "aButtons": [ "select_all", "select_none" ]
                }
            });

    }

    function arrangeElements()
    {
        $("#grdAccounts_wrapper > .DTTT_container").detach().insertAfter("#grdAccounts_filter");
        $(".connect-accounts-select").detach().insertAfter("#grdAccounts_length");
        
    }

    function setupDropdowns()
    {
        $('.connect-accounts-roleselect').chosen();
        $('.connect-accounts-roleselect').on('change', function(evt, params) {
            roleId = params.selected;
            refreshGrid();
        });

        $('[name="grdAccounts_length"]').chosen({
            disable_search: true,
            placeholder_text_single: 'Accounts'
        });

    }

    /*globals jQuery, window, Sys */
    (function ($, Sys)  {

        function setupPage() 
        {                       
            initGrid();
            arrangeElements();
            setupDropdowns();
        }

        $(document).ready(function () {
            setupPage();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupPage();
            });
        });

    }(jQuery, window.Sys));

</script>



