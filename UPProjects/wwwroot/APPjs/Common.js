function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (       
        (charCode < 48 || charCode > 57)
    )
        return false;
    return true;
}



$(document).ready(function () {    
    $('.NumberOnly').keypress(function (event) {
        return isNumber(event, this);
    });
    //$('.MobileNumonLy').change(function () {
        
    //    if ($('.MobileNumonLy').val() === '' || $('.MobileNumonLy').val().length !== 10) {
    //        alert("Please Enter Correct Mobile Number");
    //        $('.MobileNumonLy').val('');
    //        $('.MobileNumonLy').focus();
    //        return false;
    //    }
    //});
});

function checkPasswordStrength() {
    var number = /([0-9])/;
    var alphabets = /([a-zA-Z])/;
    var special_characters = /([~,!,@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
    if ($('#NewPasswordUser').val().length < 6 || $('#NewPasswordUser').val().length > 12) {

        alert("Password should be between 6 to 12 characters");
        return false;
    }

    if ($('#NewPasswordUser').val().match(number) && $('#NewPasswordUser').val().match(alphabets) && $('#NewPasswordUser').val().match(special_characters)) {
        console.log(1);
    }
    else {

        alert("Password should include alphabets, numbers and special characters.");
        return false;
    }
}
$(".newpass").click(function () {

    var passwordField = $('#NewPasswordUser');
    var passwordFieldType = passwordField.attr('type');
  
    if (passwordFieldType === 'password') {
        passwordField.attr('type', 'text');
        $(".newpass").removeClass('mdi mdi-eye');
        $(".newpass").addClass('mdi mdi-eye-off');
    }
    else {
        passwordField.attr('type', 'password');
        $(".newpass").removeClass('mdi mdi-eye-off');
        $(".newpass").addClass('mdi mdi-eye');

    }

});

$(".cpass").click(function () {

    var passwordField = $('#ConfirmPasswordUser');
    var passwordFieldType = passwordField.attr('type');
    if (passwordFieldType === 'password') {
        passwordField.attr('type', 'text');
        $(".cpass").removeClass('mdi mdi-eye');
        $(".cpass").addClass('mdi mdi-eye-off');

    }
    else {
        passwordField.attr('type', 'password');
        $(".cpass").removeClass('mdi mdi-eye-off');
        $(".cpass").addClass('mdi mdi-eye');

    }

});


function GetUpdatedRecords(action) {

    var url1 = "/Home/GetLastUpdatedRecord?ActionName=" + action.trim();
    $.getJSON(url1, function (data) {
        SetUpdatedDate(data);
    });

  

}
function SetUpdatedDate(data) {
    var myDate = new Date(data[0].updatedDate);
    var datetime = myDate.getDate() + "/"
        + (myDate.getMonth() + 1) + "/"
        + myDate.getFullYear() + " "
        + myDate.getHours() + ":"
        + myDate.getMinutes() + ":"
        + myDate.getSeconds();
    $("#LastUpdated").html(datetime);
}