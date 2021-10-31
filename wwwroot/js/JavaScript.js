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