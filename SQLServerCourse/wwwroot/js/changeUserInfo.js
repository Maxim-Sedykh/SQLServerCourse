document.getElementById("showChangeButton").addEventListener("click", (e) => {
    e.preventDefault();

    var input = document.getElementById("changeInput");
    var changeButton = document.getElementById("changePasswordButton");

    if (input.style.display === 'block') {
        input.style.display = 'none';
        changeButton.style.display = 'none';
    }
    else {
        input.style.display = 'block';
        changeButton.style.display = 'block';
    }
});

document.getElementById("updateInfoId").addEventListener("click", async (event) => {
    event.preventDefault();

    var updateFormData = {
        Id: parseInt(document.getElementById("Id").value),
        IsExamCompleted: Boolean(document.getElementById("IsExamCompleted").value),
        FinalGrade: parseFloat(document.getElementById("FinalGrade").value.replace(/,/g, '.')),
        Login: document.getElementById("Login").value,
        Name: document.getElementById("Name").value,
        Surname: document.getElementById("Surname").value,
        LessonsCompleted: parseInt(document.getElementById("LessonsCompleted").value)
    };
    var jsonUpdateFormData = JSON.stringify(updateFormData);

    const response = await fetch('/PersonalProfile/UpdateInfo', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: jsonUpdateFormData
    });

    const responseBody = await response.json();
    Swal.fire({
        title: 'Уведомление!',
        text: responseBody.description,
        icon: response.ok ? 'success' : 'error',
        confirmButtonText: 'Окей',
        background: '#333',
        color: 'white',
    });
});

document.getElementById("changePasswordButton").addEventListener("click", async (event) => {
    event.preventDefault();

    var changePassword = {
        NewPassword: document.getElementById("changeInput").value
    };
    var jsonСhangePassword = JSON.stringify(changePassword);

    const response = await fetch('/Account/ChangeUserPassword', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: jsonСhangePassword
    });

    const responseBody = await response.json();
    Swal.fire({
        title: 'Уведомление!',
        text: responseBody.description,
        icon: response.ok ? 'success' : 'error',
        confirmButtonText: 'Окей',
        background: '#2e0066',
        color: 'white',
    });
});