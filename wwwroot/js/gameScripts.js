function addNewUser(userName, thePhoto)
{
    var gamerBlock = document.createElement('div');
    gamerBlock.classList.add("theGamer");
    gamerBlock.setAttribute("name", userName);

    var gamerAvatarBlock = document.createElement('div');
    gamerAvatarBlock.classList.add("gamersAvatarBlock");

    var gamerAvatar = document.createElement('img');
    gamerAvatar.setAttribute("src", thePhoto);
    gamerAvatar.classList.add("gamersAvatar");

    gamerAvatarBlock.appendChild(gamerAvatar);

    gamerBlock.appendChild(gamerAvatarBlock);

    var nameAndStatus = document.createElement('div');
    nameAndStatus.classList.add("nameAndStatus");

    var gamersName = document.createElement('div');
    gamersName.classList.add("gamersName")
    gamersName.innerHTML = userName;

    var gamersRank = document.createElement('div');
    gamersRank.classList.add("gamersRank")
    gamersRank.innerHTML = "Novice";


    var gamersScore = document.createElement('div');
    gamersScore.classList.add("gamersScore")
    gamersScore.innerHTML = "Score: 145656";

    nameAndStatus.appendChild(gamersName);
    nameAndStatus.appendChild(gamersRank);
    nameAndStatus.appendChild(gamersScore);

    gamerBlock.appendChild(nameAndStatus);

    document.getElementsByClassName("gamersBlock")[0].appendChild(gamerBlock);
}