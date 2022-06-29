
var seconds = 10;
function timerTest(username) {
    clearInterval(seconds_timer_id);
    seconds = 10;

    if (document.getElementById('forTakeUsername').value == username) {
        seconds = 10;

        var seconds_timer_id = setInterval(function () {

            if (seconds > 0) {
                seconds--;

                $("#sendSeconds").val(seconds);
                $("#timeLeft").val(seconds);

                document.querySelector('.sendTheTime').click();
                if (seconds < 6) {
                    //var audio = new Audio(); // Создаём новый элемент Audio
                    //audio.src = '~/sounds/lowTime.mp3'; // Указываем путь к звуку "клика"
                    //audio.autoplay = true; // Автоматически запускаем

                    document.getElementById('lowTimeAudio').play();
                }
                

                //$(".seconds").text(seconds);

            } else {


                clearInterval(seconds_timer_id);
                document.querySelector('.sendNextUserForm').click();
            }
        }, 1000);
    }
}
