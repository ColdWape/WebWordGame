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
    //var txtConfirmPassword = document.getElementById('ConfirmPassword');

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
                //txtConfirmPassword.setAttribute("type", "password");

                revealEyeCondition[0].style.backgroundImage = "url('../images/OpennPass.png')";

                showPassStatus.style.display = 'block';
                hidePassStatus.style.display = 'none';

            }
            else {
                txtPassword.setAttribute("type", "text");
                //txtConfirmPassword.setAttribute("type", "text");

                revealEyeCondition[0].style.backgroundImage = "url('../images/ClosePass.png')";

                showPassStatus.style.display = 'none';
                hidePassStatus.style.display = 'block';

            }
        },
        
    });
})


//Выбор изображения
//function showImagesMenu() {
//    var choosingPhotoMenu = document.getElementsByClassName("choosingPhoto");
//    choosingPhotoMenu[0].style.display = 'inline';
//}
