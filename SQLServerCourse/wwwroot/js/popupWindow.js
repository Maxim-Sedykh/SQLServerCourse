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

    const response = await fetch('/PersonalProfile/UpdateInfo', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updateFormData)
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