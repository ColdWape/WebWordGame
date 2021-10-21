var userName = "q";

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

function showLoginMenu() {
    document.getElementById("registration").style.display = 'none';
    document.getElementById("login").style.display = 'inline';
    

}

function showRegistrationMenu() {
    document.getElementById("registration").style.display = 'inline';
    document.getElementById("login").style.display = 'none';

}