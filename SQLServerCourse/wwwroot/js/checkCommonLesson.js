function answersCheck(taskCorrectness) {
    var unrightAnswers = document.getElementsByClassName('answrunright');
    var rightAnswers = document.getElementsByClassName('answrright');

    for (let i = 0; i < taskCorrectness.length; i++) {
        if (taskCorrectness[i]) {
            rightAnswers[i].style.display = 'block';
        }
        else {
            unrightAnswers[i].style.display = 'block';
        }
    }
}