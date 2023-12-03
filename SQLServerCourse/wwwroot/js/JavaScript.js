document.getElementById("updateUserData").addEventListener("click", async (event) => {
    event.preventDefault();
    debugger;
    userRole = document.getElementById("Role").value;
    var updateFormData = {
        Id: parseInt(document.getElementById("Id").value),
        Login: document.getElementById("Login").value,
        Role: document.getElementById("Role").value,
        Name: document.getElementById("Name").value,
        Surname: document.getElementById("Surname").value,
        IsEditAble: Boolean(document.getElementById("IsEditAble").value),
    }; 
    var jsonUpdateFormData = JSON.stringify(updateFormData);

    const response = await fetch('/User/UpdateInfo', {
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
