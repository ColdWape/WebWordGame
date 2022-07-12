
//var seconds = 10;
//function timerTest(username, actionToStopTimer) {
//    clearInterval(seconds_timer_id);
//    seconds = 10;

//    if (document.getElementById('forTakeUsername').value == username) {
//        seconds = 10;

//        var seconds_timer_id = setInterval(function () {

//            if (seconds > 0) {
//                seconds--;

//                $("#sendSeconds").val(seconds);
//                $("#timeLeft").val(seconds);

//                document.querySelector('.sendTheTime').click();
//                if (seconds < 6) {
//                    //var audio = new Audio(); // Создаём новый элемент Audio
//                    //audio.src = '~/sounds/lowTime.mp3'; // Указываем путь к звуку "клика"
//                    //audio.autoplay = true; // Автоматически запускаем

//                    document.getElementById('lowTimeAudio').play();
//                }


//                //$(".seconds").text(seconds);

//            } else {


//                clearInterval(seconds_timer_id);
//                document.querySelector('.sendNextUserForm').click();
//            }
//        }, 1000);
//    }
//}

var seconds = 30;
var seconds_timer_id;

function stopTheTimer() {
    clearInterval(seconds_timer_id);
}

function startTheTimer() {

    seconds = 30;
    seconds_timer_id = setInterval(function () {
        
        
        if (seconds > 1) {
            if (seconds == 30) {
                document.getElementById('timeToMove').play();
            }
            seconds--;

            $("#sendSeconds").val(seconds);
            $("#timeLeft").val(seconds);
            $(".seconds").text(seconds);

            if (seconds < 6) {

                //var audio = new Audio('~/sounds/lowTime.mp3');
                //audio.play();

               
                //audio.play();
                //var audio = new audio(); // создаём новый элемент audio
                //audio.src = '~/sounds/lowtime.mp3'; // указываем путь к звуку "клика"
                //audio.autoplay = true; // автоматически запускаем

                //document.getelementbyid('lowtimeaudio').play();
                document.getElementById('lowTimeAudio').play();
            }


            //$(".seconds").text(seconds);

        } else {

            clearInterval(seconds_timer_id);
            document.querySelector('.sendNextUserForm').click();
        }
    }, 1000);














    //clearInterval(seconds_timer_id);
    //seconds = 10;

    //if (document.getElementById('forTakeUsername').value == username) {
    //    seconds = 10;

    //    var seconds_timer_id = setInterval(function () {

    //        if (seconds > 0) {
    //            seconds--;

    //            $("#sendSeconds").val(seconds);
    //            $("#timeLeft").val(seconds);

    //            document.querySelector('.sendTheTime').click();
    //            if (seconds < 6) {
    //                //var audio = new Audio(); // Создаём новый элемент Audio
    //                //audio.src = '~/sounds/lowTime.mp3'; // Указываем путь к звуку "клика"
    //                //audio.autoplay = true; // Автоматически запускаем

    //                document.getElementById('lowTimeAudio').play();
    //            }


    //            //$(".seconds").text(seconds);

    //        } else {


    //            clearInterval(seconds_timer_id);
    //            document.querySelector('.sendNextUserForm').click();
    //        }
    //    }, 1000);
    //}
}

