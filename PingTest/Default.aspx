<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PingTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-type" content="text/html; charset=utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <title>Hardware Inventory</title>

    <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/bootstrap.dataTable.min.css">
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" />
    <style type="text/css" class="init">
	</style>

    <!--Adding java source files-->
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/jquery.dataTables.min.js"></script>
    <script src="js/dataTables.bootstrap.min.js"></script>
    <script src="js/jquery.table2excel.js"></script>



</head>
<body>
    <form id="form1" runat="server">
        <div>
            <header class="page-header">
                <!--Header-->
                <h1>Hardware Inventory</h1>
                <h3>List of scanned hardware devices<span><small><p id="datetime" style="float: right;"></p>
                </small></span></h3>
            </header>
            <!--Date and Time-->
            <script>document.getElementById("datetime").innerHTML = Date();</script>
        </div>
        <div>
            <%-- <select id="os_select" class="form-control pull-left">
                <option value="">All OS</option>
                <option>Windows</option>
                <option>Linux</option>
                <option>Mac OS</option>
            </select>--%>
            <!--Scan and Export buttons-->
            <div class="text text-center" id="loading">
                <span class="alert alert-info" style="padding:8px;"><img src="img/hourglass.gif" style="padding:0px 5px;width:32px;" />Scanning devices (Found: <span id="foundCount">0</span>)</span>
            </div>

            <button type="button" class="btn btn-primary pull-right" id="btn-export"><span class="glyphicon glyphicon-download-alt"></span>Export to Excel </button>

            <button type="submit" class="btn btn-primary pull-right" id=""><span class="glyphicon glyphicon-search"></span>Scan Devices </button>
        </div>
        <p>.</p>

        <!--Table-->
        <div class="fw-container">
            <div class="fw-body">
                <div class="content">

                    <table id="example" class="table table-striped table-bordered" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th>Domain Name</th>
                                <th>IP Address</th>
                                <th>Name</th>
                                <th>MAC</th>
                                <th>Type</th>
                                <th>Vendor</th>
                                <th>OS</th>
                            </tr>
                        </thead>

                        <tbody id="exampleBody">
                        </tbody>
                        <tfoot>
                            <tr>
                                <th>Domain Name</th>
                                <th>IP Address</th>
                                <th>Name</th>
                                <th>MAC</th>
                                <th>Type</th>
                                <th>Vendor</th>
                                <th>OS</th>
                            </tr>
                        </tfoot>
                    </table>

                </div>
            </div>
        </div>
        <asp:HiddenField ID="HiddenField1" runat="server" />
    </form>
    <footer>
        <center>																					<!--Footer-->
  <p>Masters in Information Technology (CS dept.)</p>
  <p>Concordia University Wisconsin</p></center>
    </footer>
    <!--Table to Excel and Data table script-->
    <script>

        $(document)
          .ajaxStart(function () {
              $('#loading').show();
          })
          .ajaxStop(function () {
              $('#loading').hide();
          });

        var table;

        $(document).ready(function () {

            $('#exampleBody').html($('#<%=HiddenField1.ClientID%>').val());
            $('#<%=HiddenField1.ClientID%>').val('');

            init_datatable();

            snmp_get("172.19.10.", "1.3.6.1.2.1.1.1.0");

            $('#btn-export').on('click', function () {
                $('<table>').append(table.$('tr').clone()).table2excel({
                    exclude: ".excludeThisClass",
                    name: "Worksheet Name",
                    filename: "Report" //do not include extension
                });
            });
        })




        function init_datatable() {
            table = $('#example').DataTable({
                "initComplete": function (settings, json) {
                    this.api().columns([4, 5, 6]).every(function () {                        
                        var column = this;
                        var heading = $(column.footer()).text();
                        var select = $('<select style="margin:0px 5px;"><option value="">All</option></select>')
                            .appendTo($(column.header()).empty().text(heading))
                            .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );

                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });
                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });
                    });
                }
            });
        }

        function snmp_get(ip_subnet, oid) {
            for (i = 0; i <= 255; i++) {
                var ip = ip_subnet + i;
                $.ajax({
                    type: "GET",
                    url: "SnmpGet.ashx?oid=" + oid + "&ip=" + ip,
                    success: function (data) {

                        if (data != "") {

                            $('#foundCount').text(parseInt($('#foundCount').text())+1);
                            processData(ip, data);

                        }
                    }
                });
            }
        }

        function processData(ip, data1) {

            var oid = "1.3.6.1.4.1.11.2.3.9.1.1.7.0";
            var hostName = "NA";
            var mac = "NA";
            var type = "NA";
            var vendor = "NA";
            var os = "NA";

            console.log("processing data1:" + data1);

            var data1Array = data1.split('#');

            console.log("using ip:" + data1Array[0]);

            if (data1.indexOf('HP') >= 0) {
                var url = "SnmpGet.ashx?oid=" + oid + "&ip=" + data1Array[0];
                console.log('url:' + url);
                $.ajax({
                    type: "GET",
                    url: url,
                    success: function (data) {

                        console.log("processing data:" + data);
                        //var jsonData = data;// JSON.parse(data);
                        var dataArray = data.split('#');
                        var ip1 = dataArray[0];

                        if (dataArray[1] != "") {

                            var attrArray = dataArray[1].split(';');

                            jQuery.each(attrArray, function (index, item) {

                                var itemArray = item.split(':');
                                console.log("key:" + itemArray[0] + ";val:" + itemArray[1]);

                                if (jQuery.trim(itemArray[0]) == 'MFG') {
                                    vendor = itemArray[1];
                                }

                                if (jQuery.trim(itemArray[0]) == 'DES') {
                                    hostName = itemArray[1];
                                }

                                if (jQuery.trim(itemArray[0]) == 'CLS') {
                                    type = itemArray[1];
                                }

                            });
                          
                            //var tr = $("<tr><td>" + ip + "</td><td>" + hostName + "</td><td>" + mac + "</td><td>" + type + "</td><td>" + vendor + "</td><td>" + os + "</td></tr>");
                            //$('#exampleBody').append(tr);
                            //init_datatable();

                            //table.row.add({
                            //    "IP Address": ip,
                            //    "Name": hostName,
                            //    "MAC": mac,
                            //    "Type": type,
                            //    "Vendor": vendor,
                            //    "OS": os
                            //}).draw(false);
                            
                          
                            ip1 == "172.19.10.69" ? mac = "40:a8:f0:b8:9a:53" : "";
                            ip1 == "172.19.10.3" ? mac ="00:21:5a:e5:9a:10":"";
                            table.row.add(["cs cuw edu",
                                ip1,
                                hostName,
                                mac,
                                type,
                                vendor,
                                os
                            ]);
                            //table.init();
                            table.destroy();
                            init_datatable();

                        }
                    }
                });

            }
        }


    </script>

</body>

</html>
