<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SnmpTest.aspx.cs" Inherits="PingTest.SnmpTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="http://code.jquery.com/jquery-2.1.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <div><span>Scanned: </span><span id="scannedSpan">0</span></div>
            <div><span>Found: </span><span id="foundSpan">0</span></div>
        </div>
        <table>
            <tbody id="tableBody">
            </tbody>
        </table>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {

            snmp_get("172.19.10.", "1.3.6.1.4.1.11.2.3.9.1.1.7.0");


            //for (i = 0; i <= 255; i++) {
            //    var ip = "172.19.10." + i;
            //    $.ajax({
            //        type: "GET",
            //        url: "SnmpGet.ashx?ip=" + ip,
            //        success: function (data) {
            //            $('#scannedSpan').text(parseInt($('#scannedSpan').text()) + 1);
            //            if (data != "") {
            //                $('#foundSpan').text(parseInt($('#foundSpan').text()) + 1);
            //                $('#tableBody').append("<tr><td>" + data + "</td></tr>")
            //            }
            //        }
            //    });

            //    var ip = "172.19.11." + i;
            //    $.ajax({
            //        type: "GET",
            //        url: "SnmpGet.ashx?ip=" + ip,
            //        success: function (data) {
            //            $('#scannedSpan').text(parseInt($('#scannedSpan').text()) + 1);
            //            if (data != "") {
            //                $('#foundSpan').text(parseInt($('#foundSpan').text()) + 1);
            //                $('#tableBody').append("<tr><td>" + data + "</td></tr>")
            //            }
            //        }
            //    });

            //    var ip = "172.10.10." + i;
            //    $.ajax({
            //        type: "GET",
            //        url: "SnmpGet.ashx?ip=" + ip,
            //        success: function (data) {
            //            $('#scannedSpan').text(parseInt($('#scannedSpan').text()) + 1);
            //            if (data != "") {
            //                $('#foundSpan').text(parseInt($('#foundSpan').text()) + 1);
            //                $('#tableBody').append("<tr><td>" + data + "</td></tr>")
            //            }
            //        }
            //    });
            //}

        });


        function snmp_get(ip_subnet,oid) {
            for (i = 0; i <= 255; i++) {
                var ip = ip_subnet + i;
                $.ajax({
                    type: "GET",
                    url: "SnmpGet.ashx?oid="+oid+"&ip=" + ip,
                    success: function (data) {
                        $('#scannedSpan').text(parseInt($('#scannedSpan').text()) + 1);
                        if (data != "") {
                            $('#foundSpan').text(parseInt($('#foundSpan').text()) + 1);
                            $('#tableBody').append("<tr><td>" + data + "</td></tr>")
                        }
                    }
                });
            }
        }


    </script>
</body>
</html>
