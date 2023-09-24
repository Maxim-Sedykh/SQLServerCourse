function answersCheck(tasksCorrectness) {
    var unrightAnswers = document.getElementsByClassName('answrunright');
    var rightAnswers = document.getElementsByClassName('answrright');

    for (let i = 0; i < tasksCorrectness.length; i++) {
        if (tasksCorrectness[i]) {
            rightAnswers[i].style.display = 'block';
        }
        else {
            unrightAnswers[i].style.display = 'block';
        }
    }
}

