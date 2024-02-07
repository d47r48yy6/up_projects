$(document).ready(function () {
    BindData();
});
function BindData() {
    var url = "/Master/GetSurveyorDetails";
    $('#loader').css("display", "block");

    $.getJSON(url, function (data) {
        BindDataTable(data);
        $('#loader').css("display", "none");

    });
}
function BindDataTable(data) {

    $("#tblData tbody").empty();
    if (data.length > 0) {
        $("#tblData").DataTable({
            data: data,
            "oLanguage": {
                "emptyTable": "No record exist."
            },
            "searching": true,
            "paging": false,
         //   "lengthMenu": [[-1, 100, 50, 10], ["All", 100, 50, 10]],
            columns: [
                {
                    'data': null,
                    'render': function (data, type, fill, meta) {
                        return meta.row + 1;
                    }
                },
                { "data": "SurveyorName" },
                { "data": "DistrictName" },
                { "data": "TehsilName" },
                { "data": "BlockName" },
                { "data": "GramSabhaNameE" },
                { "data": "Image1", className: "text-center" },
                { "data": "Image2", className: "text-center" },
                { "data": "ElectricityStatus" },
                { "data": "HandPupWaterStatus" },
                {
                    "data": "Whethertapwater",
                    render: function (data, type, row, meta) {
                        var radiobtn1 = "";
                        if (row.Whethertapwater == "Yes") {
                            radiobtn1 += '<input type="hidden" value="' + row.Id + '" id="' + row.Id + '" /><input type="radio" checked id="' + row.Id + 'radio1" name="' + row.Id + 'radio1"  value="Yes" /><label for="' + row.Id + 'radio" >Yes</label>&nbsp;&nbsp;' +
                                '<input type = "radio"  id="' + row.Id + 'radio1" name = "' + row.Id + 'radio1" value = "2" /><label for="' + row.Id + 'radio" >No</label>';
                        }
                        else if (row.Whethertapwater == "No") {
                            radiobtn1 += '<input type="hidden" value="' + row.Id + '" id="' + row.Id + '" /><input type="radio"   id="' + row.Id + 'radio1" name="' + row.Id + 'radio1"  value="Yes" /><label for="' + row.Id + 'radio" >Yes</label>&nbsp;&nbsp;' +
                                '<input type = "radio" checked  id="' + row.Id + 'radio1" name = "' + row.Id + 'radio1" value = "No" /><label for="' + row.Id + 'radio" >No</label>';
                        }
                        else {
                            radiobtn1 += '<input type="hidden" value="' + row.Id + '" id="' + row.Id + '" /><input type="radio"  id="' + row.Id + 'radio1" name="' + row.Id + 'radio1"  value="Yes" /><label for="' + row.Id + 'radio" >Yes</label>&nbsp;&nbsp;' +
                                '<input type = "radio"   id="' + row.Id + 'radio1" name = "' + row.Id + 'radio1" value = "No" /><label for="' + row.Id + 'radio" >No</label>';
                        }
                        return radiobtn1;
                    },
                    className: "text-center"
                },
                {
                    "data": "Whetherredmark",
                    render: function (data, type, row, meta) {
                       
                        var radiobtn1 = "";
                        if (row.Whetherredmark == "Yes") {
                            radiobtn1 += '<input type="hidden" value="' + row.Id + '" id="' + row.Id + '" /><input type="radio" checked id="' + row.Id + 'radio1" name="' + row.Id + 'radio2"  value="Yes" /><label for="' + row.Id + 'radio" >Yes</label>&nbsp;&nbsp;' +
                                '<input type = "radio"  id="' + row.Id + 'radio1" name = "' + row.Id + 'radio2" value = "No" /><label for="' + row.Id + 'radio" >No</label>';
                        }
                        else if (row.Whetherredmark == "No") {
                            radiobtn1 += '<input type="hidden" value="' + row.Id + '" id="' + row.Id + '" /><input type="radio" checked  id="' + row.Id + 'radio1" name="' + row.Id + 'radio2"  value="Yes" /><label for="' + row.Id + 'radio" >Yes</label>&nbsp;&nbsp;' +
                                '<input type = "radio" checked  id="' + row.Id + 'radio1" name = "' + row.Id + 'radio2" value = "No" /><label for="' + row.Id + 'radio" >No</label>';
                        }
                        else {
                            radiobtn1 += '<input type="hidden" value="' + row.Id + '" id="' + row.Id + '" /><input type="radio"  id="' + row.Id + 'radio1" name="' + row.Id + 'radio2"  value="Yes" /><label for="' + row.Id + 'radio" >Yes</label>&nbsp;&nbsp;' +
                                '<input type = "radio"   id="' + row.Id + 'radio1" name = "' + row.Id + 'radio2" value = "No" /><label for="' + row.Id + 'radio" >No</label>';
                        }
                        return radiobtn1;
                    },
                    className: "text-center"
                },
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $("td:nth-child(1)", nRow).html(nRow._DT_RowIndex + 1);
                return nRow;
            },
            responsive: true,
            "bDestroy": true,

        });


    }
    else {
        $("#tblData tbody").html("No record exist.");
    }

}
//$("body").on("change", "input[type=radio]", function () {
    
//});
$('#btnsub').click(function () {
    SaveAndUpdate();
});
function SaveAndUpdate() {
    $('#loader').css("display", "block");
    var JSONObject = new Array();
    var cnt3 = 0;
    $("#tblData tbody").each(function () {
        $(this).find('tr').each(function () {
            var obj = new Object();
            var cnt1 = 0;
            var cnt2 = 0;
            $(this).find('td').each(function () {
                var SurveyId = "";
                $(this).find('input[type=hidden]').each(function () {
                  
                    SurveyId = $(this).val();
                });
                $(this).find('input[name=' + SurveyId + 'radio1 ]').each(function () {
                    if (!$(this).is(':checked')) {
                        cnt1++;
                    }
                    if ($(this).val() != "" || $(this).val() != null) {
                        if ($(this).is(':checked')) {
                            obj['SurveyId'] = SurveyId;
                            obj['Whethertapwater'] = $(this).val();
                           
                        }

                    }
                    
                });
                $(this).find('input[name=' + SurveyId + 'radio2 ]').each(function () {
                    if (!$(this).is(':checked')) {
                        cnt2++;
                    }
                    if ($(this).val() != "" || $(this).val() != null ) {
                        if ($(this).is(':checked')) {
                            obj['Whetherredmark'] = $(this).val();
                        }
                    }

                   
                });
             
            });
            if (cnt1 != cnt2) {
                cnt3++;
            }
            if (obj.SurveyId != undefined || obj.SurveyId != null) {
                JSONObject.push(obj);
            }
           
          
        });

    });

    if (JSONObject.length == 0) {
        alert("Please Choose Yes/No for atleast one row");
        $('#loader').css("display", "none");
        return false;
    }
    let objd = JSON.stringify(JSONObject);
    var obj1 = new Object();
    obj1.objd = objd;
    var param = JSON.stringify(obj1);
    debugger
    if (cnt3 > 0) {
        alert("Please Choose both Yes/No for both (Whether tap water & Whether red mark)");
        $('#loader').css("display", "none");
        return false;
    }
    if (confirm("Are you sure ?") == true) {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: "/Master/InsertUpdateSurvey",
            data: param,
            dataType: 'json',
            success: function (data) {
                debugger
                if (data[0].ResultStatus == "t") {
                    $('#loader').css("display", "none");

                    alert('Saved/Updated Successfully.');
                    window.location.href = "/Master/UpdateSurveyordetails";
                }

                //if (parseInt(data.d) > 0) {
                //    showSuccess('Saved/Updated Successfully.');
                //    BindData();

                //}
                //else {
                //    showError(data.d);
                //}
            }
        });
    }
}
function GetFullImage(path, Image, Id) {
    $('#lblimg').html(Image);
    $('#imgprofile').attr('src', '/Upload/Surveyor/' + Id + '/' + path);
    $('#myModal').modal('show');

}
