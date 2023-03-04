function Function_OldPassword(){
    var x = document.getElementById("OldPassword");
    var y = document.getElementById("ChangeIcon")

    if (x.type === "password"){
        x.type = "text";
        y.classList.remove("fa-eye")
        y.classList.add("fa-eye-slash")
    } else{
        x.type ="password";
        y.classList.add("fa-eye")
        y.classList.remove("fa-eye-slash")
    }
 };
 function Function_NewPassword(){
    var x = document.getElementById("NewPassword");
    var y = document.getElementById("ChangeIconNew")

    if (x.type === "password"){
        x.type = "text";
        y.classList.remove("fa-eye")
        y.classList.add("fa-eye-slash")
    } else{
        x.type ="password";
        y.classList.add("fa-eye")
        y.classList.remove("fa-eye-slash")
    }
 };
 function Function_ReEnterPassword(){
    var x = document.getElementById("ReEnterPassword");
    var y = document.getElementById("ChangeIconReEnter")

    if (x.type === "password"){
        x.type = "text";
        y.classList.remove("fa-eye")
        y.classList.add("fa-eye-slash")
    } else{
        x.type ="password";
        y.classList.add("fa-eye")
        y.classList.remove("fa-eye-slash")
    }
 };