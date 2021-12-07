var userName = "";

function showAuthTypeChoosing() {
    if (userName == "") {
        document.getElementById("startBtns").style.display = 'none';
        document.getElementById("gameTypeChoosing").style.display = 'none';
        document.getElementById("gameQuantityPeopleChoosing").style.display = 'none';
        document.getElementById("authTypeChoosingBtns").style.display = 'inline';

    }
    else showGameTypeChoosingMenu();

}



function showGameTypeChoosingMenu() {
    document.getElementById("startBtns").style.display = 'none';
    document.getElementById("gameTypeChoosing").style.display = 'inline';
    document.getElementById("gameQuantityPeopleChoosing").style.display = 'none';

}

function showMainMenu() {
    document.getElementById("startBtns").style.display = 'inline';
    document.getElementById("gameTypeChoosing").style.display = 'none';
    document.getElementById("gameQuantityPeopleChoosing").style.display = 'none';
}

function showGameQuantityPeopleChoosing() {
    document.getElementById("gameQuantityPeopleChoosing").style.display = 'inline';
    document.getElementById("gameTypeChoosing").style.display = 'none';
    document.getElementById("startBtns").style.display = 'none';

}


function showLoginMenu(userChooseLogIn) {
    if (userChooseLogIn) {
        document.getElementById("registration").style.display = 'none';

        document.getElementById("login").style.display = 'inline';

    }
    else {
        document.getElementById("registration").style.display = 'inline';

        document.getElementById("login").style.display = 'none';
    }
}

function goToThePageForBtns(inputLink) {
    document.location.href = inputLink;
}


function checkShowPasswordVisibility() {
    var $revealEye = $(this).parent().find(".reveal-eye");
    if (this.value) {
        $revealEye.addClass("is-visible");
    } else {
        $revealEye.removeClass("is-visible");
    }
}

$(document).ready(function () {
    var txtPassword = document.getElementById('Password');
    //var $hideAndSeekPassword = document.getElementById('hideAndSeekPasswordBlock');
    var $revealEye = $('<div class="reveal-eye"></div>')
    $(document.getElementById('forEyeBlock')).append($revealEye);
    $(txtPassword).on("keyup", checkShowPasswordVisibility)
    var revealEyeCondition = document.getElementsByClassName("reveal-eye");
    var showPassStatus = document.getElementById("TextForShowPass");
    var hidePassStatus = document.getElementById("TextForHidePass");
    $revealEye.on({
        mousedown: function () {
            if (txtPassword.getAttribute("type") == "text") {
                txtPassword.setAttribute("type", "password");
                //revealEyeCondition.style.background = background: url(C:\Users\ТАНКИСТ\source\repos\WebWordGame\wwwroot\images\ClosePass.png) 50% 50% no-repeat;
                showPassStatus.style.display ='block';
                hidePassStatus.style.display = 'none';

            }
            else {
                txtPassword.setAttribute("type", "text");
                //revealEyeCondition.style.background = background: url(C:\Users\ТАНКИСТ\source\repos\WebWordGame\wwwroot\images\OpenPass.png) 50% 50% no-repeat;
                showPassStatus.style.display = 'none';
                hidePassStatus.style.display = 'block';

            }
        },
        /*mouseup: function () {
            txtPassword.setAttribute("type", "password");
        }
       /* mouseout: function () {
            txtPassword.setAttribute("type", "password");
        }*/
    });
})


/*

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
document.getElementById("eyee").addEventListener("click", function (e) {
    var pwd = document.getElementById("Password");
    if (pwd.getAttribute("type") == "password") {
        pwd.setAttribute("type", "text");
    } else {
        pwd.setAttribute("type", "password");
    }
});*/


/*
//Проверка введенных данных при регистрации

var nameBox = document.getElementById("LoginName");
var emailBox = document.getElementById("Email");
var passwordBox = document.getElementById("Password");
var secondPasswordBox = document.getElementById("checkPassword");
var registerForm = document.getElementById("registrationForm");
registerForm.addEventListener('submit', function (event) {

    if (nameBox.nodeValue == "") {
        nameBox.nodeValue = "ПОЛЕ";
    event.preventDefault();
    return false;
}
});*/