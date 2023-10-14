
// Timer countdown
var totalTime = document.getElementById("quiztime").innerHTML * 60; // Convert minutes to seconds
        var timeRemaining = $('#timeRemaining');
        var timeRemaining2 = $('#timeRemaining2');

        var timer = setInterval(function() {
          var minutes = Math.floor(totalTime / 60);
            var seconds = document.getElementById("quiztime").innerHTML * 60 % 60;

          timeRemaining.text(minutes.toString().padStart(2, '0') + ':' + seconds.toString().padStart(2, '0'));
          timeRemaining2.text(minutes.toString().padStart(2, '0') + ':' + seconds.toString().padStart(2, '0'));

          if (totalTime <= 0) {
            clearInterval(timer);
            $('#submitQuizButton').click(); // Automatically submit the quiz when time is up
          } else if (totalTime <= 60) {
            timeRemaining.addClass('bg-danger'); // Change time color to red when running low
            totalTime--;
          } else {
            totalTime--;
          }
        }, 1000);
          
     