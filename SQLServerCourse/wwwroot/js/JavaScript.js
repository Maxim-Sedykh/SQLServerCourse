document.getElementById('#userRoleId').select2({
    placeholder: "Выберите роль",
    minimumInputLength: 0,
    allowClear: true,
    ajax: {
        type: "POST",
        url: "/User/GetRoles",
        dataType: "json",
        processResults: function (result) {
            return {
                results: $.map(result, function (val, index) {
                    return {
                        id: index,
                        text: val
                    };
                }),
            };
        }
    }
});

    
